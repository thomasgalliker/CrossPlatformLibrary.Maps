using System.Windows;

using Microsoft.Phone.Maps.Controls;

namespace CrossPlatformLibrary.Maps
{
    public static class MapExtensions
    {
        /// <summary>
        ///     Gets whether the given point is within the map bounds.
        /// </summary>
        /// <param name="map">The map.</param>
        /// <param name="point">The point.</param>
        /// <returns>The <see cref="bool" />.</returns>
        public static bool IsVisiblePoint(this Map map, Point point)
        {
            return point.X > 0 && point.X < map.ActualWidth && point.Y > 0 && point.Y < map.ActualHeight;
        }
    }
}