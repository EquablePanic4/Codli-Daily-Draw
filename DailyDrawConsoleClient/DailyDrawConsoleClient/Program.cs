using DailyDrawConsoleClient.Models;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using TextCopy;

namespace DailyDrawConsoleClient
{
    class Program
    {
        private const string ApiUri = "https://losowanie.dynamicevent.pl";
        private readonly static string configFile;
        private static string AuthKey;
        private static bool authKeyValid = false;
        private readonly static string AppDataPah;

        static Program()
        {
            AppDataPah = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Codli\\DailyDrawConsole");
            configFile = Path.Combine(AppDataPah, "config.cdi");
        }

        static void Main(string[] args)
        {
            Console.Title = "Daily Draw Console [v1.0]";
            Console.WriteLine("Konsola zarządzania aplikacją Daily Draw\r\n");
            Console.WriteLine("Proszę czekać... trwa uwierzytelnianie...");

            //Teraz sprawdzamy czy istnieje plik konfiguracyjny - jest to zwykły plik tekstowy, a jego zawartością jest tylko klucz klienta (szyfrowanie lub XML byłby przerostem formy nad treścią)
            if (!File.Exists(configFile))
            {
                //Plik konfiguracyjny nie istnieje, więc musimy o niego poprosić
                InitializeAuthKey();
            }

            else
            {
                //W przeciwnym wypadku sprawdzamy jego poprawność
                AuthKey = File.ReadAllText(configFile);
                if (!ValidateAuthKey(AuthKey))
                    InitializeAuthKey();

                else
                    Console.WriteLine("Uwierzytelnianie przebiegło pomyślnie!\r\n");
            }

            Menu();
        }

        private static void Menu()
        {
            //Formalności mamy za sobą, więc wyświetlamy menu
            Console.Clear();
            Console.WriteLine("1) Zarejestruj klienta");
            Console.WriteLine("2) Dodaj nowego użytkownika");
            Console.WriteLine("3) Usuń użytkownika");
            Console.WriteLine("4) Symuluj losowanie");

            Console.Write("\r\n\r\nTwój wybór: ");
            var choice = Console.ReadKey();

            switch (choice.KeyChar)
            {
                case '1':
                    RegisterClient();
                    break;

                case '2':
                    AddUser();
                    break;

                case '3':
                    RemoveUser();
                    break;

                case '4':
                    SimulateDrawing();
                    break;
            }

            Menu();
        }

        #region Metody funkcjonalne

        private async static void RegisterClient()
        {
            Console.Clear();
            Console.Write("\r\nPodaj opis nowego klienta: ");
            var desc = Console.ReadLine();

            Console.WriteLine("\r\nWysyłanie żądania...");
            var response = PostQuery("/Api/RegisterClient", new AuthorizedQS(AuthKey, desc));

            try
            {
                var responseArr = response.Content.Split(":");

                if (responseArr.Length != 2)
                    throw new Exception();

                if (responseArr[0] == "true")
                {
                    Console.WriteLine("Pomyślnie zarejestrowano nowego klienta!");
                    Console.WriteLine($"Kod autoryzacji: {responseArr[1]}");
                    await Clipboard.SetTextAsync(responseArr[1]);
                    Console.WriteLine("\r\nDodano kod do schowka!");
                }
            }

            catch
            {
                Console.WriteLine("Wystąpił nieznany błąd przy rejestracji klienta.");
            }

            Console.ReadLine();
        }

        private static void AddUser()
        {
            Console.Clear();
            Console.Write("\r\nImię nowego użytkownika: ");
            var _forname = Console.ReadLine();
            Console.Write("\r\nNazwisko nowego użytkownika: ");
            var _surname = Console.ReadLine();

            Console.WriteLine("\r\nWysyłanie żądania do interfejsu API...");

            var response = PostQuery("/Api/AddUser", new AddUserModel() { authKey = AuthKey, forname = _forname, surname = _surname });
            if (response.Content == "true")
                Console.WriteLine($"Z powodzeniem zarejestrowano użytkownika {_forname} {_surname}");
            else
                Console.WriteLine("Wystąpił nieznany błąd przy dodawaniu nowego użytkownika!");

            Console.ReadLine();
        }

        private static void RemoveUser()
        {
            Console.Clear();
            Console.WriteLine("Pobieranie listy użytkowników...");
            var response = GetQuery("/Api/ListAllUsers", $"authKey={AuthKey}");

            //Próbujemy ją sparsować do JSONa...
            var jsonList = (List<User>) JsonConvert.DeserializeObject(response.Content, typeof(List<User>));

            if (jsonList == null)
            {
                Console.WriteLine("Wystąpił nieznany błąd!");
                Console.ReadLine();
                return;
            }

            Console.WriteLine("\r\nWybierz liczbę odpowiadającą użytkownikowi do usunięcia: ");

            for (var i = 0; i < jsonList.Count; i++)
                Console.WriteLine($"{i + 1}) {jsonList[i].forname} {jsonList[i].surname}");

            Console.Write("\r\nTwój wybór: ");
            var choice = Int32.Parse(Console.ReadLine());

            //Teraz wysyłamy żądanie o jego usunięcie
            Console.WriteLine("\r\nŻądam usunięcia tego użytkownika...");
            var responsePost = PostQuery("/Api/RemoveUser", new AuthorizedQS(AuthKey, jsonList[choice - 1].userId));
            if (responsePost.Content == "true")
                Console.WriteLine($"Pomyślnie usunięto użytkownika {jsonList[choice - 1].forname} {jsonList[choice - 1].surname}");
            else
                Console.WriteLine("Wystąpił nieznany błąd przy usuwaniu użytkownika!");
            Console.ReadLine();
        }

        private static void SimulateDrawing()
        {
            Console.Clear();
            Console.Write("\r\nIle dni wstecz chcesz uruchomić symulację? ");
            var days = Int32.Parse(Console.ReadLine());

            if (days < 0)
                days *= -1;

            if (days == 0)
            {
                Console.WriteLine("\r\nWprowadzono niepoprawną wartość!");
                Console.ReadLine();
                return;
            }

            Console.WriteLine("\r\nWysyłanie żądania do serwera...");
            var response = GetQuery("/Api/SimulateDrawing", $"authKey={AuthKey}&days={days}");
            if (response.Content == "true")
                Console.WriteLine("Pomyślnie dokonano symulacji na zadaną wartość!");
            else
                Console.WriteLine("Wystąpił błąd podczas symulacji!");
            Console.ReadLine();
        }

        #endregion

        #region Metody uwierzytelniające

        private static void InitializeAuthKey()
        {
            //Jeżeli plik konfiguracyjny nie istnieje, żądamy o klucz do aplikacji
            for (; ; )
            {
                Console.WriteLine("Podaj klucz autoryzujący:");
                var key = Console.ReadLine(); //Kluczem jest GUID, należy go podać razem z myślnikami

                if (ValidateAuthKey(key))
                {
                    authKeyValid = true;
                    AuthKey = key;

                    //Oraz zapisujemy klucz do pliku
                    if (!Directory.Exists(AppDataPah))
                        Directory.CreateDirectory(AppDataPah);

                    File.WriteAllText(configFile, key);
                    break;
                }

                Console.WriteLine("\r\nWprowadzony klucz jest niepoprawny!");
            }
        }

        private static bool ValidateAuthKey(string key)
        {
            //Sprawdzamy poprawność klucza
            var restClient = new RestClient(ApiUri);
            var authRequest = new RestRequest("Api/ValidateAuthKey");
            authRequest.AddJsonBody(new QueryString(key));
            authRequest.Method = Method.POST;

            var response = restClient.Execute(authRequest);

            if (response.Content == "true")
                return true;
            else
                return false;
        }

        #endregion

        #region Metody pomocnicze

        private static IRestResponse PostQuery(string uri, object json)
        {
            var restClient = new RestClient(ApiUri);
            var request = new RestRequest(uri);
            request.AddJsonBody(json);
            request.Method = Method.POST;

            return restClient.Execute(request);
        }

        private static IRestResponse GetQuery(string uri, string queryString)
        {
            if (uri[uri.Length - 1] != '/')
                uri += "/";

            var restClient = new RestClient(ApiUri);
            var request = new RestRequest($"{uri}?{queryString}");
            request.Method = Method.GET;

            return restClient.Execute(request);
        }

        #endregion
    }
}
