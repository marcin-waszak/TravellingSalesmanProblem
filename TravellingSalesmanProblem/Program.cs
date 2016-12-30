using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TravellingSalesmanProblem
{
    static class Program
    {
        public enum AlgorithmType {
            MiPlusLambda,
            MiCommaLambda
        }

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

        public static Tour Algorithm(AlgorithmType algorithmTypeType, int mi, int lambda,
            int numberOfTowns, IList<City> towns)
        {
            var tourCalculator = new TourCalculator(algorithmTypeType, mi, lambda, numberOfTowns, towns);
            return tourCalculator.Run();
        }
    }
}
