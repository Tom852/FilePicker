using FilePicker;
using System;
using System.Collections.Generic;

namespace FileCounter
{
    internal class Program
    {
        private static readonly SortedDictionary<EnumCategory, int> Histogram = new SortedDictionary<EnumCategory, int>();

        private static void Main(string[] args)
        {
            Count();
            Print();
            Console.ReadKey();
        }

        private static void Count()
        {
            var fs = new FileChooser().MyFiles;
            foreach (var file in fs)
            {
                var rep = new FileRepresentation(file);
                var cat = rep.Category.Cat;
                AddOne(cat);
            }
        }

        private static void Print()
        {
            Console.Write("Cat".PadRight(15));
            Console.Write(": ");
            Console.Write("Count".PadRight(6));
            Console.Write(" ~ ");
            Console.Write("Reroll Chance For Uniform Distribution");
            Console.Write("\n");
            Console.WriteLine("===================================================================");

            double baseVal = Histogram[EnumCategory.A_1Month];
            foreach (var kvp in Histogram)
            {
                string left = kvp.Key.ToString().PadRight(15);
                Console.Write(left);
                Console.Write(": ");
                Console.Write(kvp.Value.ToString().PadRight(6));
                Console.Write(" ~ ");
                double rerollsug = 1.0 - (baseVal / kvp.Value);
                Console.Write(rerollsug);
                Console.Write("\n");
            }
        }

        private static void AddOne(EnumCategory cat)
        {
            bool success = Histogram.TryGetValue(cat, out int value);
            if (success)
            {
                Histogram[cat] = ++value;
            }
            else
            {
                Histogram.Add(cat, 1);
            }
        }
    }
}