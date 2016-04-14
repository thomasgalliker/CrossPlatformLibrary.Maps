using System.Collections.Generic;

using CrossPlatformLibrary.Geolocation;

namespace CrossPlatformLibrary.Maps.Clustering
{
    public interface IClusteredLocation<T> where T : IClusteredGeoObject
    {
        bool IsClustered { get; }

        int ClusterCount { get; }

        void Add(T item);

        IEnumerable<T> ClusteredItems { get; }

        T CurrentObject { get; }

        Position Location { get; }
    }
}