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

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static int Main(string[] args)
        {
            Initialize();

            if (args.Length == 0)
            {
                System.Console.WriteLine("Syntax: TravellingSalesmanProblem [locations_file]");
                return 1;
            }

            string filename = args[0];
            Utilities.ParseFile(filename, _cities);

            var tourCalculator = new TourCalculator(AlgorithmType.MiPlusLambda, 2000, 3000, 10, _cities.Towns);

            Tour tour = tourCalculator.Run(null).Result;
            float distance = tour.GetDistance();

            Console.WriteLine("The distance: " + distance + " km");

            return 0;
        }

        private static void Initialize()
        {
            _cities = new CitiesCollection();
        }
    }
}
