using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravellingSalesmanProblem
{
    class Location
    {
        protected string name_;
        protected double latitude_;     // y coord // szerokosc
        protected double longitude_;    // x coord // dlugosc

        public Location(string[] input)
        {
            name_ = input[0];
            name_ = input[1];
            latitude_ = double.Parse(input[1], CultureInfo.InvariantCulture);
            longitude_ = double.Parse(input[2], CultureInfo.InvariantCulture);
        }

        public double Distance(Location a, Location b)
        {
            const double km_per_degree = 111.196672;
            double delta = Math.Acos(Math.Sin(a.latitude_) * Math.Sin(b.latitude_)
                + Math.Cos(a.latitude_) * Math.Cos(b.latitude_) * Math.Cos(a.longitude_ - b.longitude_));
            return delta * km_per_degree;
        }
    }
}
