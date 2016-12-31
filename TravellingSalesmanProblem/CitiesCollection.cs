using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravellingSalesmanProblem
{
    public class CitiesCollection
    {
        public float MaxLatitude { get; private set; }
        public float MinLatitude { get; private set; }
        public float MaxLongitude { get; private set; }
        public float MinLongitude { get; private set; }
        public int Count { get; private set; }
        public List<City> Towns { get; }

        public CitiesCollection()
        {
            Towns = new List<City>();
            ResetExtremes();
        }

        public void Add(City city)
        {
            Add(city.Name, city.Latitude, city.Longitude);
        }

        public void Add(string name, float latitude, float longitude)
        {
            Towns.Add(new City(name, latitude, longitude));
            Count = Towns.Count;

            MaxLatitude = Math.Max(latitude, MaxLatitude);
            MinLatitude = Math.Min(latitude, MinLatitude);
            MaxLongitude = Math.Max(longitude, MaxLongitude);
            MinLongitude = Math.Min(longitude, MinLongitude);
        }

        public void FlipLatitude()
        {
            for (int i = 0; i < Towns.Count; ++i)
                Towns[i].Latitude *= -1.0f;

            float temp = MaxLatitude;
            MaxLatitude = -MinLatitude;
            MinLatitude = -temp;
        }

        public void CancelOffset()
        {
            for (int i = 0; i < Towns.Count; ++i)
            {
                Towns[i].Latitude -= MinLatitude;
                Towns[i].Longitude -= MinLongitude;
            }

            MaxLongitude -= MinLongitude;
            MaxLatitude -= MinLatitude;
            MinLongitude = 0.0f;
            MinLatitude = 0.0f;
        }

        public void ScaleLatitude(float scale)
        {
            for (int i = 0; i < Towns.Count; ++i)
                Towns[i].Latitude *= scale;

            MinLatitude *= scale;
            MaxLatitude *= scale;
        }

        private void ResetExtremes()
        {
            MaxLatitude = float.MinValue;
            MinLatitude = float.MaxValue;
            MaxLongitude = float.MinValue;
            MinLongitude = float.MaxValue;
        }

        public void Clear()
        {
            Towns.Clear();
            ResetExtremes();
        }

        public City this[int key]
        {
            get
            {
                return Towns[key];
            }
            set
            {
                Towns[key] = value;
            }
        }
    };
}
