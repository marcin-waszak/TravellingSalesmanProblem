using System;
using System.Collections.Generic;
using System.IO;

namespace TravellingSalesmanProblem
{
    public static class Utilities
    {
        public static void ParseFile(string filename, CitiesCollection cities)
        {
            StreamReader my_reader = new StreamReader(filename);

            while (!my_reader.EndOfStream)
            {
                var line = my_reader.ReadLine();
                var values = line.Split(';');

                cities.Add(new City(values));
            }

            my_reader.Close();
        }

        public static void Shuffle<T>(this IList<T> list)
        {
            var n = list.Count;
            var rnd = new Random();
            while (n > 1)
            {
                var k = rnd.Next(0, n);
                --n;
                var value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}
