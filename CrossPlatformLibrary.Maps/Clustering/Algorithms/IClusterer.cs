using System.Collections.Generic;

using CrossPlatformLibrary.Geolocation;

namespace CrossPlatformLibrary.Maps.Clustering.Algorithms
{
    public interface ILocationClusterer<T> where T : IClusteredGeoObject
    {
        IEnumerable<IClusteredLocation<T>> Cluster(IEnumerable<T> items, LocationRect boundingRectangle, double threshold);
    }
}
