using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravellingSalesmanProblem
{
    public class City
    {
        public string Name { get; }
        public double Latitude { get; }
        public double Longitude { get; }

        public City(string name, double latitude, double longitude)
        {
            Name = name;
            Latitude = latitude;
            Longitude = longitude;
        }

        public City(string[] input)
        {
            Name = input[0];
            Latitude = double.Parse(input[1], CultureInfo.InvariantCulture);
            Longitude = double.Parse(input[2], CultureInfo.InvariantCulture);
        }

        public double GetDistanceTo(City target)
        {
            return Distance(this, target);
        }

        // Static methods

        public static double Distance(City a, City b)
        {
            const double km_per_degree = 111.196672;
            double delta = Math.Acos(Math.Sin(Rad(a.Latitude)) * Math.Sin(Rad(b.Latitude))
                + Math.Cos(Rad(a.Latitude)) * Math.Cos(Rad(b.Latitude)) * Math.Cos(Rad(a.Longitude - b.Longitude)));
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
