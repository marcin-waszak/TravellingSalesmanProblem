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

        private static BindingList<Town> towns_;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Initialize();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm(ref towns_));
        }

        private static void Initialize()
        {
            towns_ = new BindingList<Town>();
        }
    }
}
