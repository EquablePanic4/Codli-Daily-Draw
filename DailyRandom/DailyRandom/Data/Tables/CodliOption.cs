using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DailyRandom.Data.Tables
{
    public class CodliOption
    {
        [Key]
        public string name { get; set; }
        public string value { get; set; }
    }
}
