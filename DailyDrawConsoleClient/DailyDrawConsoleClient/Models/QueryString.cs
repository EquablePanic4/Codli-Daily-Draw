using System;
using System.Collections.Generic;
using System.Text;

namespace DailyDrawConsoleClient.Models
{
    class QueryString
    {
        public QueryString(string _value)
        {
            value = _value;
        }

        public string value { get; set; }
    }
}
