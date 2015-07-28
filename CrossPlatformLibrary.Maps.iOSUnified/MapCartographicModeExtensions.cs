using System;

using MapKit;

namespace CrossPlatformLibrary.Maps
{
    public static class MapCartographicModeExtensions
    {
        public static MKMapType ToMKMapType(this MapCartographicMode mapCartographicMode)
        {
            switch (mapCartographicMode)
            {
                case MapCartographicMode.Road:
                    return MKMapType.Standard;

                case MapCartographicMode.Aerial:
                case MapCartographicMode.Terrain:
                    return MKMapType.Satellite;

                case MapCartographicMode.Hybrid:
                    return MKMapType.Hybrid;

                default:
                    throw new ArgumentException("mapCartographicMode");
            }
        }

        public static MapCartographicMode ToMapCartographicMode(this MKMapType mapType)
        {
            switch (mapType)
            {
                case MKMapType.Standard:
                    return MapCartographicMode.Road;

                case MKMapType.Satellite:
                    return MapCartographicMode.Terrain;

                case MKMapType.Hybrid:
                    return MapCartographicMode.Hybrid;

                default:
                    throw new ArgumentException("mapType");
            }
        }
    }
}