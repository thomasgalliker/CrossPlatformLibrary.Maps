using System.Collections.Generic;
using System.Linq;

using CrossPlatformLibrary.Geolocation;

namespace CrossPlatformLibrary.Maps.PushpinClusterer
{
    public class ClusteredPushpinItem<T> //: ObservableObject
        where T : IClusteredGeoObject
    {
        private readonly List<T> geoObjects;

        public ClusteredPushpinItem()
        {
            this.geoObjects = new List<T>();
        }

        public int ClusterCount
        {
            get
            {
                return this.geoObjects.Count;
            }
        }

        public IEnumerable<T> ClusteredItems
        {
            get
            {
                return this.geoObjects;
            }
        }

        public T CurrentObject
        {
            get
            {
                return this.geoObjects.Any() ? this.geoObjects.FirstOrDefault() : default(T);
            }
        }

        public bool IsClustered
        {
            get
            {
                return this.geoObjects.Count > 1;
            }
        }

        public Position Location
        {
            get
            {
                if (this.IsClustered)
                {
                    return this.geoObjects.Select(x => x.Location).GetCentrePoint();
                }

                return this.geoObjects.Any() ? this.geoObjects.First().Location : Position.Unknown;
            }
        }

        public void Add(T locObject)
        {
            this.geoObjects.Add(locObject);
        }

        public void AddRange(IEnumerable<T> locObject)
        {
            this.geoObjects.AddRange(locObject);
        }
    }
}