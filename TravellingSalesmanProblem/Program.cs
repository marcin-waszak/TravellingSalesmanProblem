﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TravellingSalesmanProblem
{
    static class Program
    {
        public enum Algoritms {
            MiPlusLambda,
            MiCommaLambda
        };

        private static TownCollection _towns;

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
            _towns = new TownCollection();
        }
    }
}
