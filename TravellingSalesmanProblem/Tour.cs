using System;
using System.Collections.Generic;

namespace TravellingSalesmanProblem
{
    public class Tour
    {
        public IList<City> Cities { get; }

        public Tour()
        {
            Cities = new List<City>();
        }

        public Tour(IList<City> cities)
        {
            Cities = new List<City>();
            foreach (var city in cities)
            {
                Cities.Add(city);
            }
        }

        public double GetFitness()
        {
            return 1 / GetDistance();
        }

        public double GetDistance()
        {
            double distance = 0;
            for (var i = 0; i < Cities.Count; i++)
            {
                var from = Cities[i];
                var destinationCity = i + 1 < Cities.Count ? Cities[i + 1] : Cities[0];
                distance += from.GetDistanceTo(destinationCity);
            }
            return distance;
        }

        public void Shuffle()
        {
            Cities.Shuffle();
        }

        public Tour Crossover(Tour parent2)
        {
            var child = new Tour();
            var random = new Random();

            var startPos = random.Next(Cities.Count);
            var endPos = random.Next(Cities.Count);

            for (var i = 0; i < Cities.Count; i++)
            {
                child.Cities.Insert(i, null);
            }

            for (var i = 0; i < Cities.Count; i++)
            {
                if (startPos < endPos && i > startPos && i < endPos)
                {
                    child.Cities[i] = Cities[i];
                }
                else if (startPos > endPos)
                {
                    if (!(i < startPos && i > endPos))
                    {
                        child.Cities[i] = Cities[i];
                    }
                }
            }

            for (var i = 0; i < parent2.Cities.Count; i++)
            {
                if (child.Cities.Contains(parent2.Cities[i])) continue;

                for (var ii = 0; ii < Cities.Count; ii++)
                {
                    if (child.Cities[ii] != null) continue;
                    child.Cities[ii] = parent2.Cities[i];
                    break;
                }
            }

            return child;
        }
    }
}