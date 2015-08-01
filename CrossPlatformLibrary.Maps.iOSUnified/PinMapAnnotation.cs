using CoreLocation;

using CrossPlatformLibrary.Geolocation;

using MapKit;

using CrossPlatformLibrary.Utils;

namespace CrossPlatformLibrary.Maps
{
    public class PinMapAnnotation : MKAnnotation
    {
        private readonly string title;
        private readonly string subtitle;

        private CLLocationCoordinate2D coordinate;

        public override CLLocationCoordinate2D Coordinate
        {
            get
            {
                return this.coordinate;
            }
        }

        public override string Title
        { get { return this.title; } }

        public override string Subtitle
        { get { return this.subtitle; } }

        public PinMapAnnotation(Position position, string title, string subtitle = "")
        {
            Guard.ArgumentNotNull(() => position);

            this.Position = position;
            this.title = title;
            this.subtitle = subtitle;
        }

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