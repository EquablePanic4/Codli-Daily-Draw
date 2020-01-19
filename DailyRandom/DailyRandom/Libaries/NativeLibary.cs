using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DailyRandom.Libaries
{
    public class NativeLibary
    {
        public static string GeneratePassword(int length)
        {
            string passwd = String.Empty;

            for (var i = 0; i < length; i++)
            {
                int[] limesValues = new int[2];

                var random = new Random();
                var mainValue = random.Next(0, 2); //0 - cyfra, 1 - mała litera, 2 - wielka litera

                switch (mainValue)
                {
                    case 0:
                        limesValues[0] = 48;
                        limesValues[1] = 57;
                        break;

                    case 1:
                        limesValues[0] = 97;
                        limesValues[1] = 122;
                        break;

                    case 2:
                        limesValues[0] = 65;
                        limesValues[1] = 90;
                        break;
                }

                passwd += (char)random.Next(limesValues[0], limesValues[1]);
            }

            return passwd;
        }
    }
}
