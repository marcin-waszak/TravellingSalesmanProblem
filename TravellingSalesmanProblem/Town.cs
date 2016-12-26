using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravellingSalesmanProblem
{
    class Town
    {
        protected string name_;
        protected double latitude_;     // y coord // szerokosc
        protected double longitude_;    // x coord // dlugosc

        public Town(string[] input)
        {
            name_ = input[0];
            name_ = input[1];
            latitude_ = double.Parse(input[1], CultureInfo.InvariantCulture);
            longitude_ = double.Parse(input[2], CultureInfo.InvariantCulture);
        }

        public static double Distance(Town a, Town b)
        {
            const double km_per_degree = 111.196672;
            double delta = Math.Acos(Math.Sin(Rad(a.latitude_)) * Math.Sin(Rad(b.latitude_))
                + Math.Cos(Rad(a.latitude_)) * Math.Cos(Rad(b.latitude_)) * Math.Cos(Rad(a.longitude_ - b.longitude_)));
            return Deg(delta) * km_per_degree;
        }

        private static double Rad(double input)
        {
            return input * Math.PI / 180.0;
        }

        private static double Deg(double input)
        {
            return input * 180.0 / Math.PI;
        }

    }
}
