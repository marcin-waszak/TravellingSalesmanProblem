using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravellingSalesmanProblem
{
    public class TownCollection
    {
        public float MaxLatitude { get; private set; }
        public float MinLatitude { get; private set; }
        public float MaxLongitude { get; private set; }
        public float MinLongitude { get; private set; }
        public int Count { get; private set; }

        private List<Town> _towns;

        public TownCollection()
        {
            _towns = new List<Town>();
            MaxLatitude = float.MinValue;
            MinLatitude = float.MaxValue;
            MaxLongitude = float.MinValue;
            MinLongitude = float.MaxValue;
        }

        public void Add(Town town)
        {
            Add(town.Name, town.Latitude, town.Longitude);
        }

        public void Add(string name, float latitude, float longitude)
        {
            _towns.Add(new Town(name, latitude, longitude));
            Count = _towns.Count;

            MaxLatitude = Math.Max(latitude, MaxLatitude);
            MinLatitude = Math.Min(latitude, MinLatitude);
            MaxLongitude = Math.Max(longitude, MaxLongitude);
            MinLongitude = Math.Min(longitude, MinLongitude);
        }

        public void FlipLatitude()
        {
            for (int i = 0; i < _towns.Count; ++i)
                _towns[i].Latitude *= -1.0f;

            float temp = MaxLatitude;
            MaxLatitude = -MinLatitude;
            MinLatitude = -temp;
        }

        public void CancelOffset()
        {
            for (int i = 0; i < _towns.Count; ++i)
            {
                _towns[i].Latitude -= MinLatitude;
                _towns[i].Longitude -= MinLongitude;
            }

            MaxLongitude -= MinLongitude;
            MaxLatitude -= MinLatitude;
            MinLongitude = 0.0f;
            MinLatitude = 0.0f;
        }

        public void ScaleLatitude(float scale)
        {
            for (int i = 0; i < _towns.Count; ++i)
                _towns[i].Latitude *= scale;

            MinLatitude *= scale;
            MaxLatitude *= scale;
        }

        public void Clear()
        {
            _towns.Clear();
        }

        public Town this[int key]
        {
            get
            {
                return _towns[key];
            }
            set
            {
                _towns[key] = value;
            }
        }
    };
}
