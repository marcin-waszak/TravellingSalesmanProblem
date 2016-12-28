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

        public Tour(int size)
        {
            Cities = new List<City>(size);
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

        private double GetDistance()
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

        // TODO check it, it is just a dummy implementation
        public Tour Crossover(Tour parent2)
        {
            var child = new Tour(Cities.Count);
            var random = new Random();

            var startPos = random.Next(Cities.Count);
            var endPos = random.Next(Cities.Count);

            for (var i = 0; i < Cities.Count; i++)
            {
                if (startPos <= endPos && i >= startPos && i <= endPos)
                {
                    child.Cities.Add(Cities[i]);
                }
                else if (startPos > endPos)
                {
                    if (i >= startPos || i <= endPos)
                    {
                        child.Cities.Add(Cities[i]);
                    }
                }
                else
                {
                    child.Cities.Add(parent2.Cities[i]);
                }
            }

            return child;
        }
    }
}