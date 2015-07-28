using System;

namespace CrossPlatformLibrary.Maps.PushpinClusterer
{
    public struct Point
    {
        public Point(double x, double y)
            : this()
        {
            this.X = x;
            this.Y = y;
        }

        public double X { get; set; }

        public double Y { get; set; }

        public double GetDistanceTo(Point anotherPoint)
        {
            return Math.Sqrt(Math.Pow(this.X - anotherPoint.X, 2) + Math.Pow(this.Y - anotherPoint.Y, 2));
        }
    }
}