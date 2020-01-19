/*Codli-Powered*/
using System.IO;
using System;
using System.Net;
using System.Net.Mail;

namespace CodliDevelopment
{
    static class WriteText
    {
        public static void LineByLine(string[] lines, string path)
        {
            int Conte = 0;

            using (StreamWriter CT = File.CreateText(path))
            {
                foreach (string Element in lines)
                {
                    if (Conte != lines.Length - 1)
                        CT.WriteLine(Element);
                    else
                        CT.Write(Element);

                    Conte++;
                }
            }
        }

        public static void AddLinesToExistingFile(string[] lines, string path)
        {
            File.AppendAllLines(path, lines);
        }

        public static void AddTextToExistingFile(string Text, string path)
        {
            File.AppendAllText(path, Text);
        }

        public static void Simple(string text, string path)
        {
            using (StreamWriter CT = File.CreateText(path))
            {
                CT.Write(text.Replace("\r", null));
            }
        }
    }
    static class FileStatus
    {
        public static bool IsUsed(FileInfo FilePath)
        {
            FileStream stream = null;

            try
            {
                stream = FilePath.Open(FileMode.Open, FileAccess.Read, FileShare.None);
            }
            catch (IOException)
            {
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }
            //Plik jest nieużywany przez nic innego.
            return false;
        }
    }
    static class ChangeCharSize
    {
        public static char GetBigFromString(string abc)
        {
            if (char.IsUpper(abc[0]) == true)
                return abc[0];
            else
                return char.ToUpper(abc[0]);
        }

        public static char GetSmallFromString(string abc)
        {
            if (char.IsLower(abc[0]) == true)
                return abc[0];
            else
                return char.ToLower(abc[0]);
        }

        public static bool IsFirstLetterBig(string abc)
        {
            if (char.IsUpper(abc[0]) == true)
                return true;
            else
                return false;
        }
    }
    static class Endecoder
    {
        public static int Duration(string LicenseType)
        {
            string Typ = String.Empty;

            if (LicenseType.Contains("Type=") == true)
                Typ = LicenseType.Replace("Type=", null);
            else
                Typ = LicenseType;

            switch (Typ)
            {
                case "A":
                    return 3;
                case "F":
                    return 7;
                case "5":
                    return 14;
                case "G":
                    return 30;
                case "X":
                    return 60;

                default:
                    return 0;
            }
        }
    }
    public static class TimeX
    {
        public static string GetCurrentDay()
        {
            return string.Format("{0:dd-MM-yyyy}", DateTime.Now);
        }

        public static string GetCurrentTime()
        {
            return string.Format("{0:dd-MM-yyyy_hh-mm-ss-tt}", DateTime.Now);
        }

        public enum DaysInMonth
        {
            January = 31,
            Febuary = 28,
            March = 31,
            April = 30,
            May = 31,
            June = 30,
            July = 31,
            August = 31,
            September = 30,
            October = 31,
            November = 30,
            December = 31
        }

        public static DateTime IntToDate(int data)
        {
            string Conte = data.ToString();
            string r = Conte[0].ToString() + Conte[1].ToString() + Conte[2].ToString() + Conte[3].ToString();
            string m = Conte[4].ToString() + Conte[5].ToString();
            string d = Conte[6].ToString() + Conte[7].ToString();

            return new DateTime(int.Parse(r), int.Parse(m), int.Parse(d));
        }

        public static string DateToSmartString(DateTime Data)
        {
            string ToReturn = Data.Day.ToString();
            ToReturn += ". ";

            switch (Data.Month)
            {
                case 1:
                    ToReturn += "stycznia";
                    break;

                case 2:
                    ToReturn += "lutego";
                    break;

                case 3:
                    ToReturn += "marca";
                    break;

                case 4:
                    ToReturn += "kwietnia";
                    break;

                case 5:
                    ToReturn += "maja";
                    break;

                case 6:
                    ToReturn += "czerwca";
                    break;

                case 7:
                    ToReturn += "lipca";
                    break;

                case 8:
                    ToReturn += "sierpnia";
                    break;

                case 9:
                    ToReturn += "września";
                    break;

                case 10:
                    ToReturn += "października";
                    break;

                case 11:
                    ToReturn += "listopada";
                    break;

                case 12:
                    ToReturn += "grudnia";
                    break;
            }

            ToReturn += " ";
            ToReturn += Data.Year.ToString();

            return ToReturn;
        }

        public static string IntDateToSmartString(int Data)
        {
            var dateStr = Data.ToString();
            if (dateStr.Length != 8)
                return "Brak daty";

            var date = IntToDate(Data);

            return DateToSmartString(date);
        }

        public static int DateToInt(DateTime data)
        {
            return int.Parse(data.ToString("yyyyMMdd"));
        }

        public static string DateIntToReadableString(int data)
        {
            string cache = data.ToString();

            if (cache.Length != 8)
                return "Brak daty";

            char[] Array = cache.ToCharArray();
            string ToReturn = String.Empty;

            if (Array[6] != '0')
                ToReturn += Array[6];

            ToReturn += Array[7];

            cache = Array[4].ToString() + Array[5].ToString();
            int month = int.Parse(cache);

            ToReturn += ". " + MonthNumToName(month);
            ToReturn += " ";

            ToReturn += Array[0].ToString() + Array[1].ToString() + Array[2].ToString() + Array[3].ToString();

            return ToReturn;
        }

        public static string MonthNumToName(int numer)
        {
            switch (numer)
            {
                case 1:
                    return "stycznia";

                case 2:
                    return "lutego";

                case 3:
                    return "marca";

                case 4:
                    return "kwietnia";

                case 5:
                    return "maja";

                case 6:
                    return "czerwca";

                case 7:
                    return "lipca";

                case 8:
                    return "sierpnia";

                case 9:
                    return "września";

                case 10:
                    return "października";

                case 11:
                    return "listopada";

                case 12:
                    return "grudnia";

                default:
                    return "Invalid";
            }
        }

        public static string[] LastCurrentNextMonth(int CurrentMonthNum)
        {
            string[] ToReturn = new string[3];

            if (CurrentMonthNum == 12)
            {
                ToReturn[0] = "11";
                ToReturn[1] = "12";
                ToReturn[2] = "01";

                return ToReturn;
            }

            if (CurrentMonthNum == 1)
            {
                ToReturn[0] = "12";
                ToReturn[1] = "01";
                ToReturn[2] = "02";

                return ToReturn;
            }

            int Conte = 0;

            int[] integer = new int[3];
            integer[0] = CurrentMonthNum - 1;
            integer[1] = CurrentMonthNum;
            integer[2] = CurrentMonthNum + 1;

            while (Conte != 3)
            {
                ToReturn[Conte] = integer[Conte].ToString();

                if (ToReturn[Conte].Length == 1)
                    ToReturn[Conte] = "0" + ToReturn[Conte];

                Conte++;
            }

            return ToReturn;
        }

        public static string IntToShortString(int SeesHardDate)
        {
            var data = IntToDate(SeesHardDate);

            return data.Day + "-" + data.Month + "-" + data.Year;
        }

        public static int GetSHDataToday()
        {
            return DateToInt(DateTime.Now);
        }

        public static DateTime GetDateOfBirthFromPesel(string pesel)
        {
            //A teraz policzymy datę urodzenia na podstawie peselu - do 2299 roku
            string peselYear = pesel[0].ToString() + pesel[1].ToString();
            string peselMonth = pesel[2].ToString() + pesel[3].ToString();
            string peselDay = pesel[4].ToString() + pesel[5].ToString();

            //Aby przystąpić do następnego etapu, miesiąc musi stać się liczbą
            int monthInt = int.Parse(peselMonth);
            int trueMonth = 1;

            //Teraz określimy z jakiego wieku pochodzi posiadacz peselu
            string yearPrefix = String.Empty;

            if (monthInt > 0 && monthInt <= 12)
            {
                yearPrefix = "19";
                trueMonth = monthInt;
            }

            else
            {
                if (monthInt >= 21 && monthInt <= 32)
                {
                    yearPrefix = "20";
                    trueMonth = monthInt - 20;
                }

                else
                {
                    if (monthInt >= 41 && monthInt <= 52)
                    {
                        yearPrefix = "21";
                        trueMonth = monthInt - 40;
                    }

                    else
                    {
                        if (monthInt >= 61 && monthInt <= 72)
                        {
                            yearPrefix = "22";
                            trueMonth = monthInt - 60;
                        }

                        else
                        {
                            if (monthInt >= 81 && monthInt <= 92)
                            {
                                yearPrefix = "18";
                                trueMonth = monthInt - 80;
                            }

                            else
                                throw new Exception();
                        }
                    }
                }
            }

            //Mamy już prefix oraz miesiąc - do obliczania dnia nie są potrzebne wzory :)
            var dataUrodzenia = new DateTime(int.Parse(yearPrefix + peselYear), trueMonth, int.Parse(peselDay));

            return dataUrodzenia;
        }

        public static int GetSHDataDateOfBirthFromPesel(string pesel)
        {
            //A teraz policzymy datę urodzenia na podstawie peselu - do 2299 roku
            string peselYear = pesel[0].ToString() + pesel[1].ToString();
            string peselMonth = pesel[2].ToString() + pesel[3].ToString();
            string peselDay = pesel[4].ToString() + pesel[5].ToString();

            //Aby przystąpić do następnego etapu, miesiąc musi stać się liczbą
            int monthInt = int.Parse(peselMonth);
            int trueMonth = 1;

            //Teraz określimy z jakiego wieku pochodzi posiadacz peselu
            string yearPrefix = String.Empty;

            if (monthInt > 0 && monthInt <= 12)
            {
                yearPrefix = "19";
                trueMonth = monthInt;
            }

            else
            {
                if (monthInt >= 21 && monthInt <= 32)
                {
                    yearPrefix = "20";
                    trueMonth = monthInt - 20;
                }

                else
                {
                    if (monthInt >= 41 && monthInt <= 52)
                    {
                        yearPrefix = "21";
                        trueMonth = monthInt - 40;
                    }

                    else
                    {
                        if (monthInt >= 61 && monthInt <= 72)
                        {
                            yearPrefix = "22";
                            trueMonth = monthInt - 60;
                        }

                        else
                        {
                            if (monthInt >= 81 && monthInt <= 92)
                            {
                                yearPrefix = "18";
                                trueMonth = monthInt - 80;
                            }

                            else
                                throw new Exception();
                        }
                    }
                }
            }

            //Mamy już prefix oraz miesiąc - do obliczania dnia nie są potrzebne wzory :)
            var dataUrodzenia = new DateTime(int.Parse(yearPrefix + peselYear), trueMonth, int.Parse(peselDay));

            return DateToInt(dataUrodzenia);
        }

        public static int DaysLeftToDate(int SHDataStart, int SHDataEnd)
        {
            return DaysLeftToDate(IntToDate(SHDataStart), IntToDate(SHDataEnd));
        }

        public static int DaysLeftToDate(DateTime dataStart, DateTime dataEnd)
        {
            return Convert.ToInt32((dataEnd - dataStart).TotalDays);
        }

        public static int DateToIntTime(DateTime dateTime)
        {
            var strHour = dateTime.Hour.ToString();
            var strMinute = dateTime.Minute.ToString();
            var strSecond = dateTime.Second.ToString();

            if (strHour.Length < 2)
                strHour = "0" + strHour;

            if (strMinute.Length < 2)
                strMinute = "0" + strMinute;

            if (strSecond.Length < 2)
                strSecond = "0" + strSecond;

            return int.Parse(strHour + strMinute + strSecond);
        }

        public static DateTime IntTimeToDate(int time)
        {
            var str = time.ToString();
            var hour = int.Parse(str[0].ToString() + str[1].ToString());
            var minute = int.Parse(str[2].ToString() + str[3].ToString());
            var second = int.Parse(str[4].ToString() + str[5].ToString());

            return new DateTime(2012, 5, 19, hour, minute, second);
        }

        public static bool IsStringIntDateValid(string integerData)
        {
            int validInteger = 0;
            var result = Int32.TryParse(integerData, out validInteger);
            if (result == true)
            {
                //Sprawdzamy czy pasuje do schematu liczby 8-cyfrowej
                if (validInteger >= 10000000 && validInteger <= 99999999)
                    return true;
            }

            return false;
        }
    }
    static class VariableX
    {
        public static string[] SplitStringByString(string VariableToSplit, string KeyVariable)
        {
            return VariableToSplit.Split(new string[] { KeyVariable }, StringSplitOptions.None);
            /*
            //Obliczanie współczynnika pętli
            int Granica = KeyVariable.Length - 1;
            Granica = VariableToSplit.Length - Granica;

            //Tablica którą zwróci nasza metoda
            string[] EndArray = new string[0];
            string TemporarryS = String.Empty; //Ciąg pomocniczy

            //Pętla while
            int Conte = 0; //Quantyfikator
            int i = 0; //Quantyfikator zagnieżdżony wewnętrznie
            char[] Znaki = KeyVariable.ToCharArray();
            char[] MiniArray = new char[KeyVariable.Length];

            while (Conte != Granica)
            {
                //Generator tablicy znaków o rozmiarze zmiennej kluczowej
                while (i != KeyVariable.Length)
                {
                    MiniArray[i] = VariableToSplit[Conte + i];
                    i++;
                }

                //Porównywarka wygenerowanej tablicy znaków z zmienną kluczową
                i = 0; //Zerowanie quantyfikatora i, aby mógł zostać ponownie użyty.
                bool Wariograf = true;
                foreach (char E in Znaki)
                {
                    if (E != MiniArray[i])
                    {
                        Wariograf = false;
                        break;
                    }

                    i++;
                }

                i = 0; //Znów resetujemy stan quantyfikatora

                //Okazało się że znaleziono słowo klucz, więc teraz trzeba podzielić
                if (Wariograf == true)
                {
                    //Resetowanie zmiennej pomocniczej
                    TemporarryS = String.Empty;

                    while (i != Conte)
                    {
                        TemporarryS += VariableToSplit[i];
                        i++;
                    }

                    //Usuwanie z naszej zmiennej do podzielenia już podzielonych wyrazów;
                    VariableToSplit = VariableToSplit.Replace(TemporarryS + KeyVariable, null);

                    //Zmiana wartości Quantyfikatora pierwszego stopnia oraz zmiennej granicznej
                    Granica = Granica - TemporarryS.Length - KeyVariable.Length;
                    Conte = 0;
                    i = 0;

                    //Dodawanie zmiennej TemporarryS do tablicy wyjściowej
                    Array.Resize(ref EndArray, EndArray.Length + 1);
                    EndArray[EndArray.Length - 1] = TemporarryS;
                }

                Conte++;
            }

            return EndArray;*/
        }

        public static string InkrementujStringa(string liczba)
        {
            if (liczba == null)
                return "1";

            int Courtois = int.Parse(liczba);
            Courtois++;

            return Courtois.ToString();
        }

        public static string SemiHash(string valueToHash)
        {
            var semiHashed = String.Empty;
            const string fourDots = "····";
            if (valueToHash.Length >= 7)
                semiHashed = valueToHash[0].ToString() + valueToHash[1].ToString() + fourDots + valueToHash[valueToHash.Length - 2].ToString() + valueToHash[valueToHash.Length - 1].ToString();
            else
                semiHashed = fourDots;

            return semiHashed;
        }
        
    }
    static class ConnectionX
    {
        public static bool SendEmailViaGoogle(string Login, string Password, string ReciverMail, string Message, string Subject)
        {
            string email = Login;
            string password = Password;
            try
            {
                var loginInfo = new NetworkCredential(email, password);
                var msg = new MailMessage();
                var smtpClient = new SmtpClient("equablepanni.nazwa.pl", 465);

                msg.From = new MailAddress(email);
                msg.To.Add(new MailAddress(ReciverMail));
                msg.Subject = Subject;
                msg.Body = Message;
                msg.IsBodyHtml = true;

                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = loginInfo;
                smtpClient.Send(msg);
            }

            catch
            {
                return false;
            }

            return true;
        }

        public static bool SendEmailCoreEngine(string sender, string pass, string host, int port, string message, string title, string[] recievers, bool htmlBody, bool EnableSsl)
        {
            try
            {
                SmtpClient client = new SmtpClient(host);
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(sender, pass);
                client.EnableSsl = EnableSsl;
                client.Port = port;

                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(sender);

                foreach (string rec in recievers)
                    mailMessage.To.Add(rec);

                mailMessage.Body = message;
                mailMessage.IsBodyHtml = htmlBody;
                mailMessage.Subject = title;
                client.Send(mailMessage);
            }

            catch
            {
                return false;
            }

            return true;
        }
    }
    static class WebUtilitiesX
    {
        public static string ConvertDualArrayToJson(string[,] arr)
        {
            char q = '"';
            string json = "{";
            int Conte = 0;
            int arrLength = arr.GetLength(0);

            while (Conte < arrLength)
            {
                json += q + arr[Conte, 0] + q;
                json += ':';
                json += q + arr[Conte, 1] + q;

                if (Conte < (arrLength - 1))
                    json += ',';

                Conte++;
            }

            json += '}';

            return json;
        }

        public static string ConvertStringArrayToJsonArray(string[] arr)
        {
            string JSON = "[";
            int Conte = 0;

            foreach (var e in arr)
            {
                JSON += "'" + e + "'";

                if (Conte != arr.Length - 1)
                    JSON += ',';

                Conte++;
            }

            return JSON;
        }
    }
}
