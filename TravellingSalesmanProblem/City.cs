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
        public string Name { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }

        public City(string name, float latitude, float longitude)
        {
            Name = name;
            Latitude = latitude;
            Longitude = longitude;
        }

        public City(string[] input)
        {
            Name = input[0];
            Latitude = float.Parse(input[1], CultureInfo.InvariantCulture);
            Longitude = float.Parse(input[2], CultureInfo.InvariantCulture);
        }

        public double GetDistanceTo(City target)
        {
            return Distance(this, target);

        }

        // Static methods
        public static float Distance(City a, City b)
        {
            const float km_per_degree = 111.196672f;
            float delta = (float)Math.Acos(Math.Sin(Rad(a.Latitude)) * Math.Sin(Rad(b.Latitude))
                + Math.Cos(Rad(a.Latitude)) * Math.Cos(Rad(b.Latitude)) * Math.Cos(Rad(a.Longitude - b.Longitude)));
            return Deg(delta) * km_per_degree;
        }

        private static float Rad(float input)
        {
            return input * (float)Math.PI / 180.0f;
        }

        private static float Deg(float input)
        {
            return input * 180.0f / (float) Math.PI;
        }
    }
}
