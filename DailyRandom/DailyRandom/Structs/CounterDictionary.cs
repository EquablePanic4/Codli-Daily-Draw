using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DailyRandom.Structs
{
    public class CounterDictionary<TKey>
    {
        private readonly Dictionary<TKey, int> dictionary;

        public CounterDictionary() => dictionary = new Dictionary<TKey, int>();

        public void Add(TKey key)
        {
            //Dodajemy wartość, lub jeżeli istnieje, to tylko inkrementujemy licznik.
            if (dictionary.ContainsKey(key))
                dictionary[key] = dictionary[key] + 1;
            else
                dictionary.Add(key, 1);
        }

        public int Get(TKey key)
        {
            //Jeżeli nie ma takiej wartości zwracamy zero
            if (!dictionary.ContainsKey(key))
                return 0;

            return dictionary[key];
        }

        public List<DoubleGenericObject<TKey, int>> ToList()
        {
            var list = new List<DoubleGenericObject<TKey, int>>();
            foreach (var k in dictionary)
                list.Add(new DoubleGenericObject<TKey, int>() { Key = k.Key, Value = k.Value });

            return list;
        }
    }

    public class DoubleGenericObject<K, V>
    {
        public K Key { get; set; }
        public V Value { get; set; }

        public override bool Equals(object obj)
        {
            try
            {
                var dgo = (DoubleGenericObject<K, V>)obj;
                return Key.Equals(dgo.Key);
            }

            catch
            {
                return false;
            }
        }
    }
}
