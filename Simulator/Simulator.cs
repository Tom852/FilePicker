using FilePicker;
using System;
using System.Collections.Generic;

namespace Simulator
{
    public class Simulator
    {
        private FileChooser fc = new FileChooser();
        private const int MAXRUNS = 10_000;
        private SortedDictionary<EnumCategory, int> results = new SortedDictionary<EnumCategory, int>();

        public void Run()
        {
            for (int i = 0; i < MAXRUNS; i++)
            {
                var item = fc.Select();
                var cat = item.Category.Cat;
                AddOne(cat);
            }

            Print();
            Console.ReadKey();
        }

        private void Print()
        {
            foreach (var kvp in results)
            {
                string left = kvp.Key.ToString().PadRight(20);
                Console.Write(left);
                Console.Write(": ");
                Console.Write(kvp.Value);
                Console.Write("\n");
            }
        }

        private void AddOne(EnumCategory cat)
        {
            var val = GetOrCreate(cat);
            results[cat] = ++val;
        }

        private int GetOrCreate(EnumCategory key)
        {
            int val;

            if (!results.TryGetValue(key, out val))
            {
                val = 0;
                results.Add(key, val);
            }

            return val;
        }
    }
}