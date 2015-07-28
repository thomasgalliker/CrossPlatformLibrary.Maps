using System;

using MapKit;

namespace CrossPlatformLibrary.Maps
{
    public static class MKMapViewExtensions
    {
        public static int GetZoom(this MKMapView mapView)
        {
            //Original code: Int(log2(360 * (Double(self.frame.size.width/256) / self.region.span.longitudeDelta)) + 1);
            return (int)Math.Log((360 * ((mapView.Frame.Size.Width / 256) / mapView.Region.Span.LongitudeDelta)) + 1, 2);
        }

        public static void SetZoom(this MKMapView mapView, int zoomLevel, bool animated = false)
        {
            var span = new MKCoordinateSpan(0, 360 / Math.Pow(2, zoomLevel) * mapView.Frame.Size.Width / 256);
            mapView.SetRegion(new MKCoordinateRegion(mapView.CenterCoordinate, span), animated);
        }
    }
}