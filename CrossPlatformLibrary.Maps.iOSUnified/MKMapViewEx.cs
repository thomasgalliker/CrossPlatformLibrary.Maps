using CoreGraphics;

using MapKit;

using UIKit;

namespace CrossPlatformLibrary.Maps
{
    public class MKMapViewEx : MKMapView
    {
        public MKMapViewEx(CGRect bounds) : base(bounds)
        {
            this.GetViewForAnnotation += this.OnGetViewForAnnotation;
        }

        private MKAnnotationView OnGetViewForAnnotation(MKMapView mapView, IMKAnnotation annotation)
        {
            var mkUserLocation = annotation as MKUserLocation;
            if (mkUserLocation != null)
            {
                return this.GetViewForUserAnnotation(mapView, mkUserLocation);
            }

            var pinMapAnnotation = annotation as PinMapAnnotation;
            if (pinMapAnnotation != null)
            {
                return this.GetViewForPinMapAnnotation(mapView, pinMapAnnotation);
            }

            var clusterMapAnno = annotation as ClusterMapAnnotation;
            if (clusterMapAnno != null)
            {
                return this.GetViewForClusterMapAnnotation(mapView, clusterMapAnno);
            }

            return null;
        }

        private MKAnnotationView GetViewForPinMapAnnotation(MKMapView mapView, PinMapAnnotation annotation)
        {
            const string AnnotationId = "pinAnnotation";
            var annotationView = mapView.DequeueReusableAnnotation(AnnotationId) as MKPinAnnotationView;
            if (annotationView == null)
            {
                annotationView = new MKPinAnnotationView(annotation, AnnotationId);
            }

            annotationView.PinColor = MKPinAnnotationColor.Red;
            annotationView.CanShowCallout = true;
            annotationView.Draggable = false;
            annotationView.RightCalloutAccessoryView = UIButton.FromType(UIButtonType.DetailDisclosure);

            return annotationView;
        }

        private MKAnnotationView GetViewForClusterMapAnnotation(MKMapView mapView, ClusterMapAnnotation annotation)
        {
            const string AnnotationId = "clusterAnnotation";
            ////var annotationView = mapView.DequeueReusableAnnotation(AnnotationId) as ClusterMapAnnotationView;
            ////if (annotationView == null)
            //{
                var annotationView = new ClusterMapAnnotationView(annotation, AnnotationId);
                annotationView.Diameter = 24;
                annotationView.Color = UIColor.Magenta;
            //}

            annotationView.Annotation = annotation;

            return annotationView;
        }

        private MKAnnotationView GetViewForUserAnnotation(MKMapView mapView, MKUserLocation annotation)
        {
            ////const string AnnotationId = "userAnnotation";
            ////var annotationView = mapView.DequeueReusableAnnotation(AnnotationId) as MKPinAnnotationView;
            ////if (annotationView == null)
            ////{
            ////    annotationView = new MKPinAnnotationView(annotation, AnnotationId);
            ////}

            ////annotationView.PinColor = MKPinAnnotationColor.Green;
            ////annotationView.CanShowCallout = true;
            ////annotationView.Draggable = true;
            ////annotationView.RightCalloutAccessoryView = UIButton.FromType(UIButtonType.DetailDisclosure);

            ////return annotationView;
            return null;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            this.GetViewForAnnotation -= this.OnGetViewForAnnotation;
        }
    }
}