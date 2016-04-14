using CrossPlatformLibrary.Geolocation;

namespace CrossPlatformLibrary.Maps.Clustering
{
    public class ClusteredLocationRect<T> : ClusteredLocation<T>
        where T : IClusteredGeoObject
    {
        public ClusteredLocationRect(LocationRect locationRect)
        {
            this.LocationRect = locationRect;
        }

        public override Position Location
        {
            get
            {
                if (this.IsClustered)
                {
                    return this.LocationRect.Center;
                }

                return this.CurrentObject.Location;
            }
        }

        public LocationRect LocationRect { get; private set; }
    }
}