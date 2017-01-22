using System;
using System.Collections.Generic;

namespace TravellingSalesmanProblem
{
    public class Population
    {
        public IList<Tour> Tours { get; set; }
        private readonly int _size;
        private const double MutationRate = 0.015;
        private const int TournamentSize = 5;
        private readonly Random _random = new Random();

        public Population(int size)
        {
            Tours = new List<Tour>(size);
            _size = size;
        }

        public void Initialize(IList<City> cities)
        {
            for (var i = 0; i < _size; i++)
            {
                var newTour = new Tour(cities);
                newTour.Shuffle();
                Tours.Add(newTour);
            }
        }

        public Tour GetFittest()
        {
            var fittest = Tours[0];
            foreach (var tour in Tours)
            {
                if (fittest.GetFitness() <= tour.GetFitness())
                {
                    fittest = tour;
                }
            }
            return fittest;
        }

        public Population Crossover()
        {
            var newPopulation = new Population(_size);
            for (var i = 0; i < _size; i++)
            {
                var parent1 = SelectParent();
                var parent2 = SelectParent();
                var child = parent1.Crossover(parent2);
                newPopulation.Tours.Add(child);
            }
            return newPopulation;
        }

        public void Mutate()
        {
            foreach (var tour in Tours)
            {
                for (var i = 0; i < tour.Cities.Count; i++)
                {
                    if (_random.NextDouble() >= MutationRate) continue;

                    var j = _random.Next(tour.Cities.Count);

                    var tempCity = tour.Cities[j];
                    tour.Cities[j] = tour.Cities[i];
                    tour.Cities[i] = tempCity;
                }
            }
        }

        private Tour SelectParent()
        {
            var tournament = new Population(TournamentSize);
            for (var i = 0; i < TournamentSize; i++)
            {
                var randomIdx = _random.Next(_size);
                tournament.Tours.Add(Tours[randomIdx]);
            }
            return tournament.GetFittest();
        }
    }
}