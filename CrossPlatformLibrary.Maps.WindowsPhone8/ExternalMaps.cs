﻿using System.Device.Location;

using Microsoft.Phone.Tasks;

namespace CrossPlatformLibrary.Maps
{
    public class ExternalMaps : IExternalMaps
    {
        /// <summary>
        ///     Navigate to specific latitude and longitude.
        /// </summary>
        /// <param name="name">Label to display</param>
        /// <param name="latitude">Lat</param>
        /// <param name="longitude">Long</param>
        /// <param name="navigationType">Type of navigation</param>
        public async void NavigateTo(string name, double latitude, double longitude, NavigationType navigationType = NavigationType.Default)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                name = string.Empty;
            }

            ////// Get the values required to specify the destination.
            ////var driveOrWalk = navigationType == NavigationType.Walking ? "ms-walk-to" : "ms-drive-to";

            ////// Assemble the Uri to launch.
            ////var uri =
            ////    new Uri(
            ////        driveOrWalk + ":?destination.latitude=" + latitude.ToString(CultureInfo.InvariantCulture) + "&destination.longitude=" + longitude.ToString(CultureInfo.InvariantCulture)
            ////        + "&destination.name=" + name);

            ////// Launch the Uri.
            ////var success = await Launcher.LaunchUriAsync(uri);
            ////if (success)
            ////{
            ////    return;
            ////}

            var mapsDirectionsTask = new MapsDirectionsTask();

            // You can specify a label and a geocoordinate for the end point.
            var location = new GeoCoordinate(latitude, longitude);
            var lml = new LabeledMapLocation(name, location);
            mapsDirectionsTask.End = lml;

            mapsDirectionsTask.Show();
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
        public void NavigateTo(string name, string street, string city, string state, string zip, string country, string countryCode, NavigationType navigationType = NavigationType.Default)
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

            var mapsDirectionsTask = new MapsDirectionsTask();

            // If you set the geocoordinate parameter to null, the label parameter is used as a search term.
            var lml = new LabeledMapLocation(string.Format("{0}%20{1},%20{2}%20{3}%20{4}", street, city, state, zip, country), null);

            mapsDirectionsTask.End = lml;

            // If mapsDirectionsTask.Start is not set, the user's current location is used as the start point.

            mapsDirectionsTask.Show();
        }
    }
}