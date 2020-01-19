using System;
using System.Collections.Generic;
using System.Text;

namespace DailyDrawConsoleClient.Models
{
    class User
    {
        public string userId { get; set; }
        public string forname { get; set; }
        public string surname { get; set; }
        public bool enabled { get; set; }

        public User()
        {

        }

        public User(string _forname, string _surname)
        {
            forname = _forname;
            surname = _surname;
            enabled = true;
        }
    }
}
