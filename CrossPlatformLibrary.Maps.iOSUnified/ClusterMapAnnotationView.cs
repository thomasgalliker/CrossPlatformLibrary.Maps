using System;
using System.Drawing;

using CoreGraphics;

using Foundation;

using MapKit;

using UIKit;

namespace CrossPlatformLibrary.Maps
{
    public sealed class ClusterMapAnnotationView : MKAnnotationView
    {
        private float diameter1;
        private readonly CGColor color1 = UIColor.Gray.CGColor;

        private float diameter2;
        private readonly CGColor color2 = UIColor.Black.CGColor;

        private float diameter3;
        private CGColor color3 = UIColor.Clear.CGColor;

        public ClusterMapAnnotationView(IMKAnnotation annotation, string reuseIdentifier)
            : base(annotation, reuseIdentifier)
        {
            this.Color = UIColor.Magenta;
            this.Diameter = 24;

            this.BackgroundColor = UIColor.Clear;
           
        }

        public float Diameter
        {
            get
            {
                return this.diameter1;
            }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException("Diameter", "Diameter has to be greater than 0.");
                }

                this.diameter1 = value;
                this.diameter2 = this.Diameter * 0.78947368421052631578947368421053f;
                this.diameter3 = this.diameter2 * 0.93333333333333333333333333333333f;

                var frame = this.Frame;
                frame.Size = new SizeF(this.diameter1, this.diameter1);
                this.Frame = frame;

                var r1 = this.diameter1 / 2;
                this.CenterOffset = new PointF(r1, r1);
            }
        }

        public UIColor Color
        {
            get
            {
                return UIColor.FromCGColor(this.color3);
            }
            set
            {
                this.color3 = value == null ? UIColor.Clear.CGColor : value.CGColor;
            }
        }

        ////public override NSObject Annotation
        ////{
        ////    get
        ////    {
        ////        return base.Annotation;
        ////    }
        ////    set
        ////    {
        ////        base.Annotation = value;
        ////        this.Redraw();
        ////    }
        ////}

        ////private void Redraw()
        ////{
        ////    this.Draw(this.Frame);
        ////}

        public override void Draw(CGRect rect)
        {
            base.Draw(rect);

            var annotation = this.Annotation as ClusterMapAnnotation;
            if (annotation == null)
            {
                return;
            }

            using (var context = UIGraphics.GetCurrentContext())
            {
                context.SetLineWidth(1.0f);
                context.SetFillColor(this.color1);
                context.SetAlpha(0.5f);
                context.FillEllipseInRect(new RectangleF(0f, 0f, this.Diameter, this.Diameter));

                context.SetFillColor(this.color2);
                context.SetAlpha(1.0f);
                context.FillEllipseInRect(new RectangleF((this.Diameter - this.diameter2) / 2, (this.Diameter - this.diameter2) / 2, this.diameter2, this.diameter2));

                context.SetFillColor(this.Color.CGColor);
                context.SetAlpha(1.0f);
                context.FillEllipseInRect(new RectangleF((this.Diameter - this.diameter3) / 2, (this.Diameter - this.diameter3) / 2, this.diameter3, this.diameter3));

                NSString titleString = new NSString(annotation.Title);
                UIColor.White.SetColor();
                UIFont font = UIFont.BoldSystemFontOfSize(10.0f);
                titleString.DrawString(new RectangleF(((float)this.CenterOffset.X) / 2, ((float)this.CenterOffset.Y) / 2, this.diameter3, this.diameter3), font);
                titleString.Dispose();
            }
        }
    }
}