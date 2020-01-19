using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DailyRandom.Data.Tables
{
    public class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid userId { get; set; }
        public string forname { get; set; }
        public string surname { get; set; }
        public bool enabled { get; set; }

        public string fullName
        {
            get
            {
                return forname + " " + surname;
            }
        }

        public User()
        {
            
        }

        public User(string _forname, string _surname)
        {
            forname = _forname;
            surname = _surname;
            enabled = true;
        }

        public override bool Equals(object obj)
        {
            try
            {
                var parsed = (User)obj;
                return parsed.userId == userId;
            }

            catch
            {
                return false;
            }
        }
    }
}
