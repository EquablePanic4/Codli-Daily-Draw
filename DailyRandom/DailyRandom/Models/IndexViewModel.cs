using CodliDevelopment;
using DailyRandom.Data.Tables;
using DailyRandom.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DailyRandom.Models
{
    public class IndexViewModel
    {
        #region Pola modelu

        public List<User> Users { get; set; }
        public List<Draw> Draws { get; set; }

        private CounterDictionary<string>[] dayOfWeekWinnerStats;

        #endregion

        #region Udawany konstruktor

        //ViewModel nie może zawierać konstruktora, w związku z czym, musimy go dodać na "chama"
        public void Initialize()
        {
            InitializeDayOfWeekStats();
        }

        #endregion

        #region Właściwości docelowe

        public List<User> todayOrder
        {
            get
            {
                var idList = Draws.Where(d => d.date == TimeX.DateToInt(DateTime.Now)).Select(d => d.userIdsOrder).FirstOrDefault();

                if (idList.Count == 0)
                    return null;

                var miniUsers = new List<User>();

                foreach (var id in idList)
                    miniUsers.Add(UserTranslator(id));

                return miniUsers;
            }
        }

        public List<DoubleGenericObject<User, int>> WinnersOfDayOfWeek(DayOfWeek dayOfWeek)
        {
            /**Zwraca listę uporządkowaną w kolejności od największej liczby wystąpień*/
            var dayOfWeekWinnerList = dayOfWeekWinnerStats[(int)dayOfWeek].ToList();
            var idList = dayOfWeekWinnerList.OrderByDescending(o => o.Value).ToList();
            var usrList = new List<DoubleGenericObject<User, int>>();
            foreach (var id in idList)
                usrList.Add(new DoubleGenericObject<User, int>() {Key = UserTranslator(id.Key), Value = id.Value });

            return usrList;
        }

        public List<DoubleGenericObject<User, int>> WinnersOfDayOfWeek(int dayOfWeek)
        {
            return WinnersOfDayOfWeek((DayOfWeek)dayOfWeek);
        }

        public List<DoubleGenericObject<User, int>> WinnersOfPairDays(bool pair)
        {
            /**Zwraca listę uporządkowaną w kolejności od największej liczby wystąpień*/
            //Wybieramy losowania wykonywane w dni parzyste lub nieparzyste
            var destinationList = new List<DoubleGenericObject<User, int>>();
            var drawsList = (from d in Draws where d.date % 2 == Convert.ToInt32(!pair) select d).ToList();
            var cdictionary = new CounterDictionary<string>();

            foreach (var d in drawsList)
                cdictionary.Add(d.userIdsOrder.ElementAt(0));

            var cacheList = cdictionary.ToList();
            foreach (var c in cacheList)
                destinationList.Add(new DoubleGenericObject<User, int>() { Key = UserTranslator(c.Key), Value = c.Value });

            return destinationList;
        }

        public List<DoubleGenericObject<User, int>> CasualWinners
        {
            get
            {
                //Teraz zliczamy kto ile razy wygrał
                var dict = new CounterDictionary<User>();
                foreach (var d in Draws)
                    dict.Add(UserTranslator(d.userIdsOrder.ElementAt(0)));

                return dict.ToList();
            }
        }

        public List<DoubleGenericObject<User, double>> ProcentageOfWin
        {
            get
            {
                var wins = CasualWinners;
                var endList = new List<DoubleGenericObject<User, double>>();
                var allChances = 0.00;
                foreach (var e in wins)
                    allChances += e.Value;

                foreach (var e in wins)
                    endList.Add(new DoubleGenericObject<User, double>() { Key = e.Key, Value = Math.Round((((double)e.Value) / allChances) * 100, 1) });

                return endList.OrderByDescending(o => o.Value).ToList();
            }
        }

        public List<DoubleGenericObject<User, double>> ProcentageOfLose
        {
            get
            {
                var loses = CasualLosers;
                var endList = new List<DoubleGenericObject<User, double>>();
                var allChances = 0.00;
                foreach (var e in loses)
                    allChances += e.Value;

                foreach (var e in loses)
                    endList.Add(new DoubleGenericObject<User, double>() { Key = e.Key, Value = Math.Round((((double)e.Value) / allChances) * 100, 1) });

                return endList.OrderByDescending(o => o.Value).ToList();
            }
        }

        public List<DoubleGenericObject<User, int>> CasualLosers
        {
            get
            {
                //Teraz zliczamy kto ile razy wygrał
                var dict = new CounterDictionary<User>();
                foreach (var d in Draws)
                    dict.Add(UserTranslator(d.userIdsOrder.ElementAt(d.userIdsOrder.Count - 1)));

                return dict.ToList();
            }
        }

        public List<DoubleGenericObject<User, double>> PredictNextWinner()
        {
            var allChances = 0.00;
            var tomorow = DateTime.Now.AddDays(1);
            var usersChances = new Dictionary<User, double>();

            var winnersOfPairityTomorrow = WinnersOfPairDays(TimeX.DateToInt(tomorow) % 2 == 0);
            var winnersOfDayOfWeek = WinnersOfDayOfWeek(tomorow.DayOfWeek);
            var casualWinners = CasualWinners;

            foreach (var w in winnersOfPairityTomorrow)
            {
                if (w.Key.enabled)
                {
                    usersChances.Add(w.Key, w.Value);
                    allChances += w.Value;
                }
            }

            foreach (var w in winnersOfDayOfWeek)
            {
                if (w.Key.enabled)
                {
                    if (usersChances.ContainsKey(w.Key))
                        usersChances[w.Key] += w.Value;
                    else
                        usersChances.Add(w.Key, w.Value);

                    allChances += w.Value;
                }
            }

            foreach (var w in casualWinners)
            {
                if (w.Key.enabled)
                {
                    if (usersChances.ContainsKey(w.Key))
                        usersChances[w.Key] += w.Value;
                    else
                        usersChances.Add(w.Key, w.Value);

                    allChances += w.Value;
                }
            }



            var list = new List<DoubleGenericObject<User, double>>();

            foreach (var w in usersChances)
                list.Add(new DoubleGenericObject<User, double>() { Key = w.Key, Value = Math.Round((w.Value / allChances) * 100, 1) });

            return list.OrderByDescending(d => d.Value).ToList();
        }

        public List<DoubleGenericObject<User, double>> PercentageOf(List<DoubleGenericObject<User, int>> integers)
        {
            var allEntities = 0.00;
            foreach (var entity in integers)
                allEntities += entity.Value;

            var toReturn = new List<DoubleGenericObject<User, double>>();
            foreach (var e in integers)
                toReturn.Add(new DoubleGenericObject<User, double>() { Key = e.Key, Value = Math.Round((((double)e.Value) / allEntities) * 100, 1) });

            return toReturn.OrderByDescending(o => o.Value).ToList();
        }

        public string DayOfWeekPolish(int dayOfWeek)
        {
            switch (dayOfWeek)
            {
                case 0:
                    return "Niedziela";

                case 1:
                    return "Poniedziałek";

                case 2:
                    return "Wtorek";

                case 3:
                    return "Środa";

                case 4:
                    return "Czwartek";

                case 5:
                    return "Piątek";

                case 6:
                    return "Sobota";

                default:
                    return "Dzień sądu ostatecznego";
            }
        }

        public string DayOfWeekPolish(DayOfWeek dayOfWeek)
        {
            return DayOfWeekPolish((int)dayOfWeek);
        }

        /*public List<DoubleGenericObject<User, int>> WinsNumber()
        {
            var lista = new List<DoubleGenericObject<User, int>>();
            var dict = new Dictionary<string, int>();
            foreach (var d in Draws)
            {
                if (dict.ContainsKey(d.userIdsOrder.ElementAt(0)))
                    ++dict[d.userIdsOrder.ElementAt(0)];
                else
                    dict.Add(d.userIdsOrder.ElementAt(0), 1);
            }

            foreach (var d in dict)
                lista.Add(new DoubleGenericObject<User, int>() { Key = Users.Where(u => u.userId.ToString() == d.Key).FirstOrDefault(), Value = d.Value });

            return lista.OrderByDescending(o => o.Value).ToList();
        }

        public List<DoubleGenericObject<User, int>> LosesNumber()
        {
            var lista = new List<DoubleGenericObject<User, int>>();
            var dict = new Dictionary<string, int>();
            foreach (var d in Draws)
            {
                if (dict.ContainsKey(d.userIdsOrder.ElementAt(d.userIdsOrder.Count - 1)))
                    ++dict[d.userIdsOrder.ElementAt(d.userIdsOrder.Count - 1)];
                else
                    dict.Add(d.userIdsOrder.ElementAt(d.userIdsOrder.Count - 1), 1);
            }

            foreach (var d in dict)
                lista.Add(new DoubleGenericObject<User, int>() { Key = Users.Where(u => u.userId.ToString() == d.Key).FirstOrDefault(), Value = d.Value });

            return lista.OrderByDescending(o => o.Value).ToList();
        }*/

        #endregion

        #region Metody prywatne

        private User UserTranslator(string id)
        {
            var guid = new Guid(id);
            return Users.Where(u => u.userId == guid).FirstOrDefault();
        }

        private void InitializeDayOfWeekStats()
        {
            //Musimy zrobić listę z listą wylosowanych dla każdego dnia tygodnia
            //0 - niedziela, 6 - sobota
            var arr = new CounterDictionary<string>[7]; // <- user id

            //Teraz musimy utworzyć instancję dla każdej pozycji w tablicy
            for (var i = 0; i < arr.Length; i++)
                arr[i] = new CounterDictionary<string>();

            //Następnie przechodzimy do analizy danych
            foreach (var d in Draws)
                arr[(int)TimeX.IntToDate(d.date).DayOfWeek].Add(d.userIdsOrder.ElementAt(0));

            dayOfWeekWinnerStats = arr;
        }

        #endregion
    }
}