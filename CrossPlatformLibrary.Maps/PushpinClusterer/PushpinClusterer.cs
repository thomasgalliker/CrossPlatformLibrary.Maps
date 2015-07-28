using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using CrossPlatformLibrary.Geolocation;

namespace CrossPlatformLibrary.Maps.PushpinClusterer
{
    public sealed class PushpinClusterer<T> //: FrameworkElement 
        where T : IClusteredGeoObject
    {
        private readonly double distanceThreshold;
        private readonly IEnumerable<T> pins;
        private readonly Func<Position, bool> checkIsVisiblePoint;
        private readonly Func<Position, Point> getViewportPoint;

        public PushpinClusterer(IEnumerable<T> pins, double distanceTreshold, Func<Position, bool> checkIsVisiblePoint, Func<Position, Point> getViewportPoint)
        {
            ////this.map.ViewChanged += (s, e) => this.RenderPins();    //TODO GATH: Trigger cluster refreshes from here! (should be possible)
            ////this.map.ResolveCompleted += (sender, args) => { };

            this.pins = pins;
            this.distanceThreshold = distanceTreshold;
            this.checkIsVisiblePoint = checkIsVisiblePoint;
            this.getViewportPoint = getViewportPoint;

            this.PushpinModels = new ObservableCollection<ClusteredPushpinItem<T>>();
        }

        /// <summary>
        ///     The clustering completed.
        /// </summary>
        public event EventHandler<EventArgs> ClusteringCompleted;

        /// <summary>
        ///     Gets or sets the pushpin models.
        /// </summary>
        public ObservableCollection<ClusteredPushpinItem<T>> PushpinModels { get; set; }

        /// <summary>
        ///     Re-render the pushpins based on the current zoom level.
        /// </summary>
        /// <param name="zoomLevel"></param>
        public void RenderPins(double zoomLevel)
        {
            this.PushpinModels.Clear();
            var distanceFactor = zoomLevel < 18 ? this.distanceThreshold : 0;
            var pinsToAdd = new List<PushpinContainer<T>>();

            // consider each pin in turn
            var visiblePushpins = this.pins.Where(p => this.checkIsVisiblePoint(p.Location)).ToList();
            foreach (T pin in visiblePushpins)
            {
                var newPinContainer = new PushpinContainer<T>(pin, this.getViewportPoint(pin.Location));

                bool addNewPin = true;

                // determine how close they are to existing pins
                foreach (var pinContainer in pinsToAdd)
                {
                    addNewPin = true;
                    double distance = pinContainer.ScreenLocation.GetDistanceTo(newPinContainer.ScreenLocation);

                    // if the distance threshold is exceeded, do not add this pin, instead
                    // add it to a cluster
                    if (distance < distanceFactor)
                    {
                        pinContainer.Merge(newPinContainer);
                        addNewPin = false;
                        break;
                    }
                }

                if (addNewPin)
                {
                    pinsToAdd.Add(newPinContainer);
                }
            }

            foreach (var projectedPin in pinsToAdd)
            {
                this.PushpinModels.Add(projectedPin.SelectContainerItem());
            }

            this.OnClusteringCompleted();
        }

        private void OnClusteringCompleted()
        {
            var handler = this.ClusteringCompleted;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }
    }
}