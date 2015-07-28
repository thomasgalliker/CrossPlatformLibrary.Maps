using System;

namespace CrossPlatformLibrary.Maps
{
    public static class MapCartographicModeExtensions
    {
        //TODO GATH: use GoogleMap.MapType*
        private const int MapTypeNormal = 1;
        private const int MapTypeSatellite = 2;
        private const int MapTypeTerrain = 3;
        private const int MapTypeHybrid = 4;

        public static int ToGoogleMapType(this MapCartographicMode mapCartographicMode)
        {
            switch (mapCartographicMode)
            {
                case MapCartographicMode.Aerial:
                    return MapTypeSatellite;

                case MapCartographicMode.Hybrid:
                    return MapTypeHybrid;

                case MapCartographicMode.Road:
                    return MapTypeNormal;

                case MapCartographicMode.Terrain:
                    return MapTypeTerrain;

                default:
                    throw new ArgumentException("mapCartographicMode");
            }
        }
    }
}