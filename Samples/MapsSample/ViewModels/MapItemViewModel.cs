using CrossPlatformLibrary.Geolocation;

using MapsSample.Model;

namespace MapsSample.ViewModels
{
    public class MapItemViewModel : IClusteredGeoObject
    {
        public MapItemViewModel(Restaurant restaurant)
        {
            this.Location = restaurant.Location;
        }

        public Position Location { get; set; }
    }
}