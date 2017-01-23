using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace TravellingSalesmanProblem
{
    static class TestProgram
    {
        private static CitiesCollection _cities;
        private const bool Elitism = true;
        private const int NumOfSteps = 1000;
        private const double MutationRate = 0.05;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static int Main(string[] args)
        {
            Initialize();

            if (args.Length != 2)
            {
                System.Console.WriteLine("Syntax: <TravellingSalesmanProblem [locations_file]> <numberOfCities>");
                return 1;
            }

            string filename = args[0];
            Utilities.ParseFile(filename, _cities);

            TestTours(AlgorithmType.MiCommaLambda, MutationRate, Elitism, NumOfSteps, int.Parse(args[1]), _cities.Towns);

            return 0;
        }

        private static void TestTours(AlgorithmType algorithmType, double MutationRate, bool Elitism, int NumOfSteps, int numOfCities, IList<City> cities)
        {
            Console.WriteLine("numOfCities;mi;lambda;shortestDistance;averageDistance");

            if (numOfCities == 10)
            {
                for (int i = 5; i <= 15; i = i + 1)
                {
                    for (int j = i + 1; j <= 2 * i; j = j + 1)
                    {
                        PrintDistances(algorithmType, i, j, 0.2, Elitism, NumOfSteps, numOfCities, cities);
                    }
                }
            }
            
            if (numOfCities == 20)
            {
                for (int i = 10; i <= 100; i = i + 10)
                {
                    for (int j = i + 1; j <= 2 * i; j = j + 10)
                    {
                        PrintDistances(algorithmType, i, j, 0.1, Elitism, NumOfSteps, numOfCities, cities);
                    }
                }
                for (int i = 100; i <= 500; i = i + 100)
                {
                    PrintDistances(algorithmType, i, i + 1, 0.1, Elitism, NumOfSteps, numOfCities, cities);
                }
            }

            if (numOfCities == 30)
            {
                for (int i = 10; i <= 100; i = i + 10)
                {
                    for (int j = i + 1; j <= 2 * i; j = j + 10)
                    {
                        PrintDistances(algorithmType, i, j, 0.065, Elitism, NumOfSteps, numOfCities, cities);
                    }
                }

                for (int i = 100; i <= 500; i = i + 100)
                {
                     PrintDistances(algorithmType, i, i + 1, 0.065, Elitism, NumOfSteps, numOfCities, cities);
                }
            }
        }

        private static void PrintDistances(AlgorithmType algorithmType, int mi, int lambda, double MutationRate, bool Elitism, int NumOfSteps, int numOfCities, IList<City> cities)
        {
            double shortestDistance = double.MaxValue;
            double averageDistance = 0;

            for (int n = 0; n < 4; n++)
            {
                var tourCalculator = new TourCalculator(algorithmType, mi, lambda, MutationRate, Elitism, NumOfSteps, numOfCities, cities);
                Tour tour = tourCalculator.Run(null).Result;
                float distance = tour.GetDistance();

                if (distance < shortestDistance)
                {
                    shortestDistance = distance;
                }

                averageDistance += distance;
            }
            averageDistance /= 4;
            Console.WriteLine(numOfCities + ";" + mi + ";" + lambda + ";" + shortestDistance + ";" + averageDistance);
        }

        private static void Initialize()
        {
            _cities = new CitiesCollection();
        }
    }

}
