using System.Collections.Generic;

using CrossPlatformLibrary.Geolocation;

namespace CrossPlatformLibrary.Maps.PushpinClusterer
{
    public class PushpinContainer<T>
        where T : IClusteredGeoObject
    {
        private readonly List<T> pushpins = new List<T>();

        public PushpinContainer(T pushpin, Point screenLocation)
        {
            this.pushpins.Add(pushpin);
            this.ScreenLocation = screenLocation;
        }

        public Point ScreenLocation { get; private set; }

        public void Merge(PushpinContainer<T> pinContainer)
        {
            foreach (T pin in pinContainer.pushpins)
            {
                this.pushpins.Add(pin);
            }
        }

        public ClusteredPushpinItem<T> SelectContainerItem()
        {
            var model = new ClusteredPushpinItem<T>();
            model.AddRange(this.pushpins);
            return model;
        }
    }
}