using CodliDevelopment;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DailyRandom.Data.Tables
{
    public class Draw
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid drawId { get; set; }
        public List<string> userIdsOrder { get; set; }
        public int date { get; set; }

        public Draw()
        {

        }

        public Draw(int _date, List<User> allUsers)
        {
            //Najpierw losujemy kolejność
            userIdsOrder = new List<string>();
            var random = new Random();
            while (allUsers.Count > 0)
            {
                var index = random.Next(0, allUsers.Count);
                if (index == allUsers.Count)
                    index--;

                userIdsOrder.Add(allUsers[index].userId.ToString());
                allUsers.RemoveAt(index);
            }

            date = _date;
        }

        public Draw(List<User> allUsers)
        {
            //Najpierw losujemy kolejność
            userIdsOrder = new List<string>();
            var random = new Random();
            while (allUsers.Count > 0)
            {
                var index = random.Next(0, allUsers.Count);
                if (index == allUsers.Count)
                    index--;

                userIdsOrder.Add(allUsers[index].userId.ToString());
                allUsers.RemoveAt(index);
            }

            date = TimeX.DateToInt(DateTime.Now);
        }
    }
}
