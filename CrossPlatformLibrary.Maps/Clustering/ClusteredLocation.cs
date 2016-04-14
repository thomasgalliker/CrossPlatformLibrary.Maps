using System.Collections.Generic;
using System.Linq;

using CrossPlatformLibrary.Geolocation;

namespace CrossPlatformLibrary.Maps.Clustering
{
    public abstract class ClusteredLocation<T> : IClusteredLocation<T> where T : IClusteredGeoObject
    {
        private T firstItem;
        private readonly List<T> clusteredItems;

        protected ClusteredLocation()
        {
            this.clusteredItems = new List<T>();
        }

        public void Add(T item)
        {
            this.clusteredItems.Add(item);

            if (this.firstItem == null)
            {
                this.firstItem = item;
            }
        }

        public int ClusterCount
        {
            get
            {
                return this.clusteredItems.Count;
            }
        }
      
        public T CurrentObject { get { return this.firstItem; } }

        public IEnumerable<T> ClusteredItems { get { return this.clusteredItems; } }

        public bool IsClustered { get { return this.ClusteredItems.Count() > 1; } }

        public abstract Position Location { get; }
    }
}
