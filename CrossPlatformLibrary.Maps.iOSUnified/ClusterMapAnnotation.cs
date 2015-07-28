using CoreLocation;

using CrossPlatformLibrary.Geolocation;

using MapKit;

using Xamarin.Utils;

namespace CrossPlatformLibrary.Maps
{
    public class ClusterMapAnnotation : MKAnnotation
    {
        private readonly string title;
        private CLLocationCoordinate2D coordinate;

        public override CLLocationCoordinate2D Coordinate
        {
            get
            {
                return this.coordinate;
            }
        }

        public ClusterMapAnnotation(Position position, string title)
        {
            Guard.ArgumentNotNull(() => position);

            this.Position = position;
            this.title = title;
        }

        public override string Title
        { get { return this.title; } }


        public Position Position
        {
            get
            {
                return new Position(this.Coordinate.Latitude, this.Coordinate.Longitude);
            }
            set
            {
                this.coordinate = new CLLocationCoordinate2D(value.Latitude, value.Longitude);
            }
        }
    }
}