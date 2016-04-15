using System;
using System.Linq;
using System.Threading.Tasks;

using CrossPlatformLibrary.Geolocation;
using CrossPlatformLibrary.Maps;
using CrossPlatformLibrary.Tracing;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

using Guards;

using MapsSample.Services;

using ObservableView;
using ObservableView.Extensions;

namespace MapsSample.ViewModels
{
    public class RestaurantMapViewModel : ViewModelBase
    {
        private readonly ITracer tracer;
        private readonly ILocationService locationService;

        private const double DefaultLocationZoomLevel = 12d; // The ZoomLevel which is used when current position is updated.
        private double zoomLevel = DefaultLocationZoomLevel;
        private bool mapResolved = false; // Is used to indicate that the map has been touched while reloading the current location.
        private Position centerPoint;

        private bool isUpdating;
        private RelayCommand mapResolveCompletedCommand;
        private Position currentLocation;
        private readonly IDataLoader dataLoader;

        public ObservableView<MapItemViewModel> Restaurants { get; }

        public bool UpdatingVisibility
        {
            get
            {
                return (this.IsUpdating || this.Restaurants == null || !this.Restaurants.Source.Any());
            }
        }

        public bool IsUpdating
        {
            get
            {
                return this.isUpdating;
            }
            set
            {
                this.Set(() => this.IsUpdating, ref this.isUpdating, value);
                this.RaisePropertyChanged(() => this.UpdatingVisibility);
            }
        }

        public RestaurantMapViewModel(ITracer tracer, IDataLoader dataLoader, ILocationService locationService)
        {
            Guard.ArgumentNotNull(tracer, "tracer");
            Guard.ArgumentNotNull(dataLoader, "dataLoader");
            Guard.ArgumentNotNull(locationService, "locationService");

            this.tracer = tracer;
            this.dataLoader = dataLoader;
            this.locationService = locationService;

            this.Restaurants = new ObservableView<MapItemViewModel>();
            this.CurrentLocation = new Position(47.05016819999999d, 8.309307200000034d);
            this.CenterPoint = this.CurrentLocation;

            this.InitializeRestaurantsAndLocation();
        }

        private async void InitializeRestaurantsAndLocation()
        {
            await this.UpdateRestaurantItems();
        }

        public RelayCommand MapResolveCompletedCommand
        {
            get
            {
                return this.mapResolveCompletedCommand ?? (this.mapResolveCompletedCommand = new RelayCommand(this.OnMapResolveCompleted));
            }
        }

        private void OnMapResolveCompleted()
        {
            this.tracer.Error("OnMapResolveCompleted (VM)");

            this.mapResolved = true;
            this.Restaurants.Refresh();

            this.mapResolved = false;
        }

        private async Task UpdateRestaurantItems()
        {
            this.IsUpdating = true;

            var restaurants = await this.dataLoader.GetAllRestaurants();
            this.Restaurants.Source.Clear();
            this.Restaurants.Source = restaurants.Select(restaurant => new MapItemViewModel(restaurant)).ToObservableCollection();

            this.IsUpdating = false;
        }

        public Position CurrentLocation
        {
            get
            {
                return this.currentLocation;
            }
            set
            {
                this.currentLocation = value;
                this.RaisePropertyChanged(() => this.CurrentLocation);
                this.RaisePropertyChanged(() => this.HorizontalAccuracy);
            }
        }

        public Position CenterPoint
        {
            get
            {
                return this.centerPoint;
            }
            set
            {
                if (value != this.centerPoint)
                {
                    this.centerPoint = value;
                    this.RaisePropertyChanged(() => this.CenterPoint);
                }
            }
        }

        public double HorizontalAccuracy
        {
            get
            {
                return this.CurrentLocation.GetAccuracyRadius(this.ZoomLevel);
            }
        }

        public double ZoomLevel
        {
            get
            {
                return this.zoomLevel;
            }
            set
            {
                if (Math.Abs(value - this.zoomLevel) > 0.001)
                {
                    this.zoomLevel = value;
                    this.RaisePropertyChanged(() => this.ZoomLevel);
                    this.RaisePropertyChanged(() => this.HorizontalAccuracy);
                }
            }
        }

        public MapCartographicMode CartographicMode
        {
            get
            {
                return MapCartographicMode.Road;
            }
        }
    }
}