using System;
using System.Collections.Generic;
using System.Text;

namespace DailyDrawConsoleClient.Models
{
    class AuthorizedQS
    {
        public AuthorizedQS()
        {

        }

        public AuthorizedQS(string _authKey, string _value)
        {
            authKey = _authKey;
            value = _value;
        }

        public string authKey { get; set; }
        public string value { get; set; }
    }
}
