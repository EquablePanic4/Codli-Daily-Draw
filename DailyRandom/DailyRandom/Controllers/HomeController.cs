using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DailyRandom.Models;
using DailyRandom.Data;
using CodliDevelopment;

namespace DailyRandom.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private ApplicationDBContext db;

        public HomeController(ILogger<HomeController> logger, ApplicationDBContext _db)
        {
            _logger = logger;
            db = _db;
        }

        public async Task<IActionResult> Index()
        {
            var today = TimeX.DateToInt(DateTime.Now);
            var todayDraw = (from a in db.Draws where a.date == today select a).FirstOrDefault();

            if (todayDraw == null)
            {
                todayDraw = new Data.Tables.Draw((from all in db.Users where all.enabled == true select all).ToList());
                db.Draws.Add(todayDraw);
                await db.SaveChangesAsync();
            }

            return View(new IndexViewModel() {Draws = db.Draws.ToList(), Users = db.Users.ToList() });
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
