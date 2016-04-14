using System;
using System.Collections.Generic;

using CrossPlatformLibrary.Geolocation;

namespace CrossPlatformLibrary.Maps.Clustering.Algorithms
{
    public class RectangularClusterer<T> : ILocationClusterer<T>
        where T : IClusteredGeoObject
    {
        private const double EquatorialLatitudeMetersPerDegree = 111319.5431527d;
        private static readonly double pixelWidthOfCluster = 40d;

        public IEnumerable<IClusteredLocation<T>> Cluster(IEnumerable<T> items, LocationRect boundingRectangle, double zoomLevel)
        {
            var thresholdLat = this.EnsureLatitude(GetThresholdDegreesAtZoomLevel(zoomLevel, 0d));
            var thresholdLon = this.EnsureLongitude(GetThresholdDegreesAtZoomLevel(zoomLevel, boundingRectangle.Center.Latitude));

            var clusters = new List<ClusteredLocationRect<T>>();

            foreach (var item in items.IsInBoundary(boundingRectangle))
            {
                var cluster = this.GetClusterForThisPoint(clusters, item.Location, thresholdLat, thresholdLon);

                cluster.Add(item);
            }

            return clusters;
        }

        private static double GetThresholdDegreesAtZoomLevel(double zoomLevel, double latitude)
        {
            // see: http://msdn.microsoft.com/en-us/library/aa940990.aspx
            var latitudeInRadian = latitude * Math.PI / 180d;
            var meters = pixelWidthOfCluster * 156543.04d * Math.Cos(latitudeInRadian) / Math.Pow(2d, zoomLevel);
            var degrees = meters / EquatorialLatitudeMetersPerDegree;

            return degrees;
        }

        private ClusteredLocation<T> GetClusterForThisPoint(List<ClusteredLocationRect<T>> clusters, Position location, double thresholdLat, double thresholdLon)
        {
            foreach (var cluster in clusters)
            {
                if (cluster.LocationRect.Contains(location))
                {
                    return cluster;
                }
            }

            var clusterRect = new LocationRect();
            var nw = new Position(this.EnsureLatitude(location.Latitude + thresholdLat), this.EnsureLongitude(location.Longitude - thresholdLon));
            var se = new Position(this.EnsureLatitude(location.Latitude - thresholdLat), this.EnsureLongitude(location.Longitude + thresholdLon));

            clusterRect.Northwest = nw;
            clusterRect.Southeast = se;

            var newCluster = new ClusteredLocationRect<T>(clusterRect);

            clusters.Add(newCluster);

            return newCluster;
        }

        private double EnsureLatitude(double degrees)
        {
            if (degrees > 90d)
            {
                return 180d - degrees;
            }

            if (degrees < -90d)
            {
                return -180d - degrees;
            }

            return degrees;
        }

        private double EnsureLongitude(double degrees)
        {
            if (degrees > 180d)
            {
                return 360d - degrees;
            }

            if (degrees < -180d)
            {
                return -360d - degrees;
            }

            return degrees;
        }
    }
}