using System.Windows;
using System.Windows.Markup;

using Microsoft.Phone.Maps.Toolkit;

namespace CrossPlatformLibrary.Maps.Controls
{
    /// <summary>
    ///     Represents a pushpin on the map.
    /// </summary>
    [ContentProperty("Content")]
    public sealed class PushpinCluster : MapChildControl
    {
        public static readonly DependencyProperty ClusterDiameterProperty = DependencyProperty.Register("ClusterDiameter", typeof(double), typeof(PushpinCluster), null);

        public double ClusterDiameter
        {
            get
            {
                return (double)this.GetValue(ClusterDiameterProperty);
            }
            set
            {
                this.SetValue(ClusterDiameterProperty, value);
            }
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="PushpinCluster" /> class.
        /// </summary>
        public PushpinCluster()
        {
            this.DefaultStyleKey = typeof(Pushpin);
        }
    }
}