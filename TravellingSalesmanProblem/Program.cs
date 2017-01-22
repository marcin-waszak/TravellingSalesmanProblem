using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TravellingSalesmanProblem
{
    public enum AlgorithmType
    {
        MiPlusLambda,
        MiCommaLambda
    }

    static class Program
    {
        private static CitiesCollection _cities;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Initialize();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm(ref _cities));
        }

        private static void Initialize()
        {
            _cities = new CitiesCollection();
        }

        public static Task<Tour> Algorithm(AlgorithmType algorithmType, int mi, int lambda, double mutationRate,
            bool elitism, int numberOfSteps, int numberOfTowns, IList<City> towns, IProgress<int> progress)
        {
            var tourCalculator = new TourCalculator(algorithmType, mi, lambda, mutationRate, elitism, numberOfSteps, numberOfTowns, towns);
            return tourCalculator.Run(progress);
        }
    }
}
