using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TravellingSalesmanProblem
{
    class TourCalculator
    {
        private const int NumOfSteps = 100;
        private readonly Random _random = new Random();
        public AlgorithmType AlgorithmType { get; }
        public int Mi { get; }
        public int Lambda { get; }
        private bool Elitism { get; }
        public int NumOfCities { get;  }
        public IList<City> Cities { get; }

        public TourCalculator(AlgorithmType algorithmType, int mi, int lambda, bool elitism, int numOfCities, IList<City> cities)
        {
            AlgorithmType = algorithmType;
            Mi = mi;
            Lambda = lambda;
            Elitism = elitism;
            NumOfCities = numOfCities;
            Cities = cities.Take(numOfCities).ToList();
        }
        
        public async Task<Tour> Run(IProgress<int> progress)
        {
            var resultTour = await Task.Run(() =>
            {
                // 1. wygeneruj P - populacje mi osobnikow
                var initialPopulation = new Population(Mi);
                initialPopulation.Initialize(Cities);
                for (var i = 0; i < NumOfSteps; i++)
                {
                    // Strategia Elitarna - zachowaj najlepszego osobnika
                    var elite = initialPopulation.GetFittest();

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
                    if (AlgorithmType == AlgorithmType.MiPlusLambda)
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

                    // Strategia Elitarna - zachowaj najlepszego osobnika z poprzedniej populacji w kolejnej:
                    if (Elitism)
                    {
                        initialPopulation.Tours = populationToChoose.Tours
                            .OrderBy(x => _random.Next())
                            .Take(Mi - 1)
                            .ToList();
                        initialPopulation.Tours.Add(elite);
                    }
                    else
                    {
                        initialPopulation.Tours = populationToChoose.Tours
                            .OrderBy(x => _random.Next())
                            .Take(Mi)
                            .ToList();
                    }

                    progress?.Report(i * 100 / NumOfSteps);
                }
                return initialPopulation.GetFittest();
            });
            return resultTour;
        }
    }
}