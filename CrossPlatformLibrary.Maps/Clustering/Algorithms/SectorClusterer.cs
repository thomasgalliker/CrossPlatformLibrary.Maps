using System.Collections.Generic;

using CrossPlatformLibrary.Geolocation;

namespace CrossPlatformLibrary.Maps.Clustering.Algorithms
{
    public class SectorClusterer<T> : ILocationClusterer<T>
        where T : IClusteredGeoObject
    {
        public IEnumerable<IClusteredLocation<T>> Cluster(IEnumerable<T> items, LocationRect boundingRectangle, double sectorOrdinal)
        {
            var clusters = new List<ClusteredLocationRect<T>>();
            var sectorRects = this.GetSectorRects(boundingRectangle, sectorOrdinal);

            foreach (var item in items.IsInBoundary(boundingRectangle))
            {
                foreach (var sector in sectorRects)
                {
                    if (sector.Contains(item.Location))
                    {
                        var cluster = this.GetClusterForThisSector(clusters, sector);
                        cluster.Add(item);

                        break;
                    }
                }
            }

            return clusters;
        }

        private ClusteredLocation<T> GetClusterForThisSector(List<ClusteredLocationRect<T>> clusters, LocationRect sector)
        {
            foreach (var cluster in clusters)
            {
                if (cluster.LocationRect.Intersects(sector))
                {
                    return cluster;
                }
            }

            var newCluster = new ClusteredLocationRect<T>(sector);
            clusters.Add(newCluster);

            return newCluster;
        }

        private IEnumerable<LocationRect> GetSectorRects(LocationRect boundingRectangle, double sectorOrdinal)
        {
            var sectors = new List<LocationRect>();

            var strideX = (boundingRectangle.West - boundingRectangle.East) / sectorOrdinal;
            var strideY = (boundingRectangle.North - boundingRectangle.South) / sectorOrdinal;

            for (int x = 0; x < sectorOrdinal; x++)
            {
                for (int y = 0; y < sectorOrdinal; y++)
                {
                    sectors.Add(
                        new LocationRect(
                            boundingRectangle.North - (y * strideY),
                            boundingRectangle.West - (x * strideX),
                            boundingRectangle.North - ((y + 1) * strideY),
                            boundingRectangle.West - ((x + 1) * strideX)));
                }
            }

            return sectors;
        }
    }
}