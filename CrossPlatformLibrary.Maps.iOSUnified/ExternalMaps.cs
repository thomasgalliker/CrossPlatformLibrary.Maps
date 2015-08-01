#if __UNIFIED__
using CoreLocation;
using Foundation;
using MapKit;
#else
using MonoTouch.CoreLocation;
using MonoTouch.Foundation;
using MonoTouch.MapKit;
#endif
using CrossPlatformLibrary.Tracing;

namespace CrossPlatformLibrary.Maps
{
    public class ExternalMaps : IExternalMaps
    {
        private readonly ITracer tracer;

        public ExternalMaps()
        {
            this.tracer = Tracer.Create(this);
        }

        /// <summary>
        ///     Navigate to specific latitude and longitude.
        /// </summary>
        /// <param name="name">Label to display</param>
        /// <param name="latitude">Lat</param>
        /// <param name="longitude">Long</param>
        /// <param name="navigationType">Type of navigation</param>
        public void NavigateTo(string name, double latitude, double longitude, NavigationType navigationType = NavigationType.Default)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                name = string.Empty;
            }

            NSDictionary dictionary = null;
            var mapItem = new MKMapItem(new MKPlacemark(new CLLocationCoordinate2D(latitude, longitude), dictionary));
            mapItem.Name = name;

            MKLaunchOptions launchOptions = null;
            if (navigationType != NavigationType.Default)
            {
                launchOptions = new MKLaunchOptions { DirectionsMode = navigationType == NavigationType.Driving ? MKDirectionsMode.Driving : MKDirectionsMode.Walking };
            }

            var mapItems = new[] { mapItem };
            MKMapItem.OpenMaps(mapItems, launchOptions);
        }

        /// <summary>
        ///     Navigate to an address
        /// </summary>
        /// <param name="name">Label to display</param>
        /// <param name="street">Street</param>
        /// <param name="city">City</param>
        /// <param name="state">Sate</param>
        /// <param name="zip">Zip</param>
        /// <param name="country">Country</param>
        /// <param name="countryCode">Country Code if applicable</param>
        /// <param name="navigationType">Navigation type</param>
        public async void NavigateTo(string name, string street, string city, string state, string zip, string country, string countryCode, NavigationType navigationType = NavigationType.Default)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                name = string.Empty;
            }

            if (string.IsNullOrWhiteSpace(street))
            {
                street = string.Empty;
            }

            if (string.IsNullOrWhiteSpace(city))
            {
                city = string.Empty;
            }

            if (string.IsNullOrWhiteSpace(state))
            {
                state = string.Empty;
            }

            if (string.IsNullOrWhiteSpace(zip))
            {
                zip = string.Empty;
            }

            if (string.IsNullOrWhiteSpace(country))
            {
                country = string.Empty;
            }

            var placemarkAddress = new MKPlacemarkAddress { City = city,
                                                            ////Country = country,
                                                            ////State = state,
                                                            ////Street = street,
                                                            ////Zip = zip,
                                                            ////CountryCode = countryCode
                                                          };

            var coder = new CLGeocoder();
            var placemarks = await coder.GeocodeAddressAsync(placemarkAddress);

            if (placemarks.Length == 0)
            {
                this.tracer.Info("Unable to get geocode address from address");
                return;
            }

            CLPlacemark placemark = placemarks[0];

            var mapItem = new MKMapItem(new MKPlacemark(placemark.Location.Coordinate, placemarkAddress));
            mapItem.Name = name;

            MKLaunchOptions launchOptions = null;
            if (navigationType != NavigationType.Default)
            {
                launchOptions = new MKLaunchOptions { DirectionsMode = navigationType == NavigationType.Driving ? MKDirectionsMode.Driving : MKDirectionsMode.Walking };
            }

            var mapItems = new[] { mapItem };
            MKMapItem.OpenMaps(mapItems, launchOptions);
        }
    }

    public class MKPlacemarkAddress : NSDictionary
    {
        public string City
        {
            get
            {
                return this["city"].ToString();
            }
            set
            {
                this["city"] = FromObject(value);
            }
        }
    }
}