using CoreLocation;

using CrossPlatformLibrary.Geolocation;

using Xamarin.Utils;

namespace CrossPlatformLibrary.Maps
{
    public static class PositionExtensions
    {
        public static CLLocationCoordinate2D ToCLLocationCoordinate2D(this Position position)
        {
            Guard.ArgumentNotNull(() => position);

            return new CLLocationCoordinate2D(position.Latitude, position.Longitude);
        }
    }
}