using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodliDevelopment;
using DailyRandom.Data;
using DailyRandom.Data.Tables;
using DailyRandom.Libaries;
using DailyRandom.Models;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace DailyRandom.Controllers
{
    public class ApiController : Controller
    {
        #region Konstruktor i deklaracje
        private ApplicationDBContext db;

        public ApiController(ApplicationDBContext _db)
        {
            db = _db;
        }

        #endregion

        #region Metody POST

        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] AddUserModel ajaxModel)
        {
            if (!IsClientRegistered(ajaxModel.authKey))
                return StatusCode(403);

            //Na początku sprawdzamy czy nie ma już osoby o takim imieniu i nazwisku
            var user1 = (from a in db.Users where (a.forname.ToUpper() + a.surname.ToUpper()) == (ajaxModel.forname.ToUpper() + ajaxModel.surname.ToUpper()) select a).FirstOrDefault();

            if (user1 != null)
            {
                //Sprawdzamy czy ten użytkownik jest włączony
                if (!user1.enabled)
                {
                    user1.enabled = true;
                    db.Users.Update(user1);
                    await db.SaveChangesAsync();

                    return Content("true");
                }

                else
                    return Content("false");
            }

            var user = new User(ajaxModel.forname, ajaxModel.surname);
            db.Users.Add(user);
            await db.SaveChangesAsync();

            return Content("true");
        }

        [HttpPost]
        public async Task<IActionResult> ValidateAuthKey([FromBody] QueryString ajaxModel)
        {
            if ((from a in db.ApplicationClients where a.applicationClientId.ToString() == ajaxModel.value select a).Count() > 0)
                return Content("true");

            return Content("false");
        }

        [HttpPost]
        public async Task<IActionResult> RegisterClient([FromBody] AuthorizedQS ajaxModel)
        {
            if (ajaxModel.value.Length == 0)
                return Content("false");

            var client = new ApplicationClient() { ClientDescription = ajaxModel.value };
            db.ApplicationClients.Add(client);
            await db.SaveChangesAsync();

            return Content("true:" + client.applicationClientId);
        }

        [HttpPost]
        public async Task<IActionResult> RemoveUser([FromBody] AuthorizedQS ajaxModel)
        {
            if (!IsClientRegistered(ajaxModel.authKey))
                return Content("false");

            var guid = new Guid(ajaxModel.value);
            var user = (from a in db.Users where a.userId == guid select a).FirstOrDefault();

            if (user.enabled)
            {
                user.enabled = false;
                db.Users.Update(user);
                await db.SaveChangesAsync();

                return Content("true");
            }

            return Content("false");
        }

        #endregion

        #region Metody HTTP Get

        [HttpGet]
        public async Task<IActionResult> ListAllUsers(string authKey)
        {
            if (!IsClientRegistered(authKey))
                return Content("false");

            var allUsers = (from a in db.Users where a.enabled select a).ToList();
            return Json(allUsers);
        }

        [HttpGet]
        public async Task<IActionResult> SimulateDrawing(string authKey, int days)
        {
            if (!IsClientRegistered(authKey) || days == 0)
                return Content("false");

            if (days > 0)
                days *= -1;

            //Obliczanie daty początkowej //jeżeli days = 1, chodzi o wczoraj
            var startDt = DateTime.Now.AddDays(days);
            var today = TimeX.DateToInt(DateTime.Now);
            var start = TimeX.DateToInt(startDt);

            //Najpierw usuwamy z bazy danych wszystkie rekordy mieszczące się w przedziale symulacji
            var toRemove = (from a in db.Draws where a.date >= start && a.date < today select a).ToList();
            foreach (var r in toRemove)
                db.Draws.Remove(r);

            //Teraz losujemy po kolei... :)
            var users = (from u in db.Users where u.enabled select u).ToList();
            var draws = new List<Draw>();
            while (start < today)
            {
                draws.Add(new Draw(start, new List<User>(users)));
                startDt = startDt.AddDays(1);
                start = TimeX.DateToInt(startDt);
            }

            //Dodajemy nowe rekordy do bazy danych
            foreach (var e in draws)
                db.Draws.Add(e);

            //Zapisujemy zmiany
            await db.SaveChangesAsync();
            return Content("true");
        }

        [HttpGet]
        public async Task<IActionResult> DrawToday(string authKey)
        {
            if (!IsClientRegistered(authKey))
                return Content("false");

            var today = TimeX.DateToInt(DateTime.Now);
            var todayDrawCnt = (from d in db.Draws where d.date == today select d).Count();
            if (todayDrawCnt > 0)
                return Content("false");

            var draw = new Draw((from u in db.Users where u.enabled select u).ToList());
            db.Draws.Add(draw);
            await db.SaveChangesAsync();
            
            //Teraz zapisujemy naszą bazę danych do pliku JSON

            return Content("true");
        }

        #endregion

        #region Metody prywatne

        private bool IsClientRegistered(string id)
        {
            var guid = new Guid(id);
            var counter = (from a in db.ApplicationClients where a.applicationClientId == guid select a.ClientDescription).Count();

            return (counter > 0);
        }

        #endregion
    }
}