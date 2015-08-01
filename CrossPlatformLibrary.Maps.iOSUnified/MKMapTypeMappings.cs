using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MapKit;
using UIKit;
using CrossPlatformLibrary.Utils;

namespace CrossPlatformLibrary.Maps
{
    public class MKMapTypeMappings : IEnumerable<MKMapTypeMapping>
    {
        private readonly IList<MKMapTypeMapping> list;
        private readonly UISegmentedControl uiSegmentedControl;
        private int counter;

        public MKMapTypeMappings(UISegmentedControl uiSegmentedControl)
        {
            Guard.ArgumentNotNull(() => uiSegmentedControl);

            this.uiSegmentedControl = uiSegmentedControl;
            this.uiSegmentedControl.ValueChanged += this.OnValueChanged;

            this.list = new List<MKMapTypeMapping>();
        }

        public event EventHandler<MKMapType> ValueChanged;

        public void Add(MapCartographicMode mapType, string text)
        {
            var item = new MKMapTypeMapping(mapType.ToMKMapType(), text, this.counter++);
            this.list.Add(item);
            this.uiSegmentedControl.InsertSegment(item.Title, item.Position, false);

            if (this.uiSegmentedControl.SelectedSegment == -1)
            {
                this.uiSegmentedControl.SelectedSegment = 0;
            }
        }

        private MKMapType GetMapType(int selectedSegment)
        {
            var item = this.list.SingleOrDefault(x => x.Position == selectedSegment);
            if (item != null)
            {
                return item.MapType;
            }

            throw new InvalidOperationException(string.Format("Could not find selected segment <{0}>. Use Add method to add segments before calling GetMapType.", selectedSegment));
        }

        public void Clear()
        {
            this.counter = 0;
            this.list.Clear();
            this.uiSegmentedControl.RemoveAllSegments();
        }

        public IEnumerator<MKMapTypeMapping> GetEnumerator()
        {
            return this.list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)this.list).GetEnumerator();
        }

        private void OnValueChanged(object sender, EventArgs eventArgs)
        {
            var handler = this.ValueChanged;
            if (handler != null)
            {
                var mapType = this.GetMapType((int)this.uiSegmentedControl.SelectedSegment);
                handler(this, mapType);
            }
        }
    }

    public class MKMapTypeMapping
    {
        public MKMapTypeMapping(MKMapType mapType, string title, int position)
        {
            this.Position = position;
            this.Title = title;
            this.MapType = mapType;
        }

        public int Position { get; private set; }

        public string Title { get; private set; }

        public MKMapType MapType { get; private set; }
    }
}