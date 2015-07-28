using System.Device.Location;
using System.Windows;
using System.Windows.Markup;

using Microsoft.Phone.Maps.Toolkit;

namespace CrossPlatformLibrary.Maps.Controls
{
    /// <summary>
    ///     Represents a pushpin on the map.
    /// </summary>
    [ContentProperty("Content")]
    public sealed class UserLocationMarker : MapChildControl
    {
        public static readonly DependencyProperty HorizontalAccuracyProperty = DependencyProperty.Register(
            "HorizontalAccuracy",
            typeof(double),
            typeof(UserLocationMarker),
            new PropertyMetadata(default(double)));

        /// <summary>
        ///     Initializes a new instance of the <see cref="Pushpin" /> class.
        /// </summary>
        public UserLocationMarker()
        {
            this.DefaultStyleKey = typeof(Pushpin);
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="UserLocationMarker" /> class.
        /// </summary>
        /// <param name="userLocation">The user location.</param>
        public UserLocationMarker(GeoCoordinate userLocation)
            : this()
        {
            this.GeoCoordinate = userLocation;
        }

        public double HorizontalAccuracy
        {
            get
            {
                return (double)this.GetValue(HorizontalAccuracyProperty);
            }
            set
            {
                this.SetValue(HorizontalAccuracyProperty, value);
            }
        }
    }
}