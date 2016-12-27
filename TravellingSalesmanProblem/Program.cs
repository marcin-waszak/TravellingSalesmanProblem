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
        };

        private static BindingList<City> _towns;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Initialize();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm(ref _towns));
        }

        private static void Initialize()
        {
            _towns = new BindingList<City>();
        }

        public static void Algorithm(AlgorithmType algorithmTypeType, int mi, int lambda,
            int numberOfTowns, IList<City> towns)
        {
            var tourCalculator = new TourCalculator(algorithmTypeType, mi, lambda, numberOfTowns, towns);
            tourCalculator.Run();
        }
    }
}
