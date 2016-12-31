using System;
using System.Collections.Generic;

namespace TravellingSalesmanProblem
{
    public static class Utilities
    {
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