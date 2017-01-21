using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace TravellingSalesmanProblem
{
    class TourCalculator
    {
        private const int NumOfSteps = 100;
        private readonly Random _random = new Random();
        public Program.AlgorithmType AlgorithmType { get; }
        public int Mi { get; }
        public int Lambda { get; }
        public int NumOfCities { get;  }
        public IList<City> Cities { get; }

        public TourCalculator(Program.AlgorithmType algorithmType, int mi, int lambda, int numOfCities, IList<City> cities)
        {
            AlgorithmType = algorithmType;
            Mi = mi;
            Lambda = lambda;
            NumOfCities = numOfCities;
            Cities = cities.Take(numOfCities).ToList();
        }
        
        public async Task<Tour> Run(IProgress<int> progress)
        {
            Tour result_tour = await Task.Run<Tour>(() =>
            {
                // 1. wygeneruj P - populacje mi osobnikow
                var initialPopulation = new Population(Mi);
                initialPopulation.Initialize(Cities);
                for (var i = 0; i < NumOfSteps; i++)
                {
                    // 2. wylosuj z P lambda-elementowa tymczasowa populacje T
                    var tempPopulation = new Population(Lambda);
                    for (var j = 0; j < Lambda; j++)
                    {
                        tempPopulation.Tours.Add(initialPopulation.Tours[_random.Next(Mi)]);
                    }

                    // 3. reprodukuj z T lambda-elementowa populacje potomna R stosujac krzyzowanie i mutacje
                    var repPopulation = tempPopulation.Crossover();
                    repPopulation.Mutate();
                    // 4. utworz P jako mi osobnikow wybranych z P i R
                    Population populationToChoose;
                    if (AlgorithmType == Program.AlgorithmType.MiPlusLambda)
                    {
                        populationToChoose = new Population(Mi + Lambda)
                        {
                            Tours = initialPopulation.Tours.Concat(repPopulation.Tours).ToList()
                        };
                    }
                    else
                    {
                        populationToChoose = repPopulation;
                    }
                    initialPopulation.Tours = populationToChoose.Tours
                        .OrderBy(x => _random.Next())
                        .Take(Mi)
                        .ToList();

                    if (progress != null)
                        progress.Report(i * 100 / NumOfSteps);
                }
                return initialPopulation.GetFittest();
            });
            return result_tour;
        }
    }
}