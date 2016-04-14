using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;

using CrossPlatformLibrary.Geolocation;
using CrossPlatformLibrary.Maps.Clustering;
using CrossPlatformLibrary.Maps.Clustering.Algorithms;

using FluentAssertions;

using Xunit;
using Xunit.Abstractions;

namespace UnitTests.PushpinClusterer
{
    public class PerformanceTests
    {
        private readonly ITestOutputHelper output;

        public PerformanceTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        ////[Fact]
        ////public void PushpinClusterer_ShouldRenderPinsInDefinedAmountOfTime()
        ////{
        ////    // Arrange
        ////    double zoomLevel = 10;

        ////    var stopwatch = new Stopwatch();
        ////    int numberOfGeoObjects = 100000;
        ////    int clusteringDistance = 50;
        ////    var centerPoint = new Position(47.05016819999999d, 8.309307200000034d);
        ////    var visiblePins = GenerateRandomTestGeoObjects(numberOfGeoObjects, centerPoint, zoomLevel).Select(x => x.Value).ToList();

        ////    var boundingRectangle = LocationRect.CreateLocationRect(visiblePins.Select(x => x.Location));
        ////    var threshold = zoomLevel < 18 ? clusteringDistance : 0;
        ////    ILocationClusterer<IClusteredGeoObject> pushpinClusterer = new DistanceClusterer<IClusteredGeoObject>();

        ////    // Act
        ////    stopwatch.Start();
        ////    pushpinClusterer.Cluster(visiblePins, boundingRectangle, 5);
        ////    stopwatch.Stop();

        ////    // Assert
        ////    pushpinClusterer.Should().NotBeNull();
        ////    //pushpinClusterer.PushpinModels.Should().HaveCount(1); //not always
        ////    //pushpinClusterer.PushpinModels.First().ClusterCount.Should().Be(19875);
        ////    stopwatch.ElapsedMilliseconds.Should().BeLessOrEqualTo(50);
        ////    this.output.WriteLine("Test duration: {0}ms", stopwatch.ElapsedMilliseconds);
        ////}

        [Fact]
        public void LocationItemDistanceClusterer_ShouldRenderPinsInDefinedAmountOfTime()
        {
            // Arrange
            double zoomLevel = 10;

            var stopwatch = new Stopwatch();
            int numberOfGeoObjects = 100000;
            int clusteringDistance = 50;
            var centerPoint = new Position(47.05016819999999d, 8.309307200000034d);
            var visiblePins = GenerateRandomTestGeoObjects(numberOfGeoObjects, centerPoint, zoomLevel).ToList();

            var boundingRectangle = LocationRect.CreateLocationRect(visiblePins.Select(x => x.Location));

            ILocationClusterer<IClusteredGeoObject> pushpinClusterer = new RectangularClusterer<IClusteredGeoObject>();
 
            // Act
            stopwatch.Start();
            var clusters = pushpinClusterer.Cluster(visiblePins, boundingRectangle, zoomLevel);
            stopwatch.Stop();

            // Assert
            pushpinClusterer.Should().NotBeNull();
            //pushpinClusterer.PushpinModels.Should().HaveCount(1); //not always
            //pushpinClusterer.PushpinModels.First().ClusterCount.Should().Be(19875);
            stopwatch.ElapsedMilliseconds.Should().BeLessOrEqualTo(50);
            this.output.WriteLine("Test duration: {0}ms", stopwatch.ElapsedMilliseconds);
        }

        ////[Fact]
        ////public void QuadTreeClusterer_ShouldRenderPinsInDefinedAmountOfTime()
        ////{
        ////    // Arrange
        ////    double zoomLevel = 10;

        ////    var stopwatch = new Stopwatch();
        ////    int numberOfGeoObjects = 10000;
        ////    var centerPoint = new Position(47.05016819999999d, 8.309307200000034d);
        ////    var visiblePins = GenerateRandomTestGeoObjects(numberOfGeoObjects, centerPoint, zoomLevel);

        ////    var minX = visiblePins.Min(wrapper => wrapper.Point.X);
        ////    var maxX = visiblePins.Max(wrapper => wrapper.Point.X);
        ////    var minY = visiblePins.Min(wrapper => wrapper.Point.Y);
        ////    var maxY = visiblePins.Max(wrapper => wrapper.Point.Y);

        ////    var visibleArea = new RectangleF(minX, minY, maxX - minX, maxY - minY);
        ////    var quadTree = new QuadTree<TestGeoObject>(visibleArea, visiblePins);

        ////    // Act
        ////    stopwatch.Start();
        ////    var query = quadTree.Query(visibleArea);
        ////    stopwatch.Stop();

        ////    // Assert
        ////    query.Should().NotBeNull();
        ////    //pushpinClusterer.PushpinModels.Should().HaveCount(1); //not always
        ////    //pushpinClusterer.PushpinModels.First().ClusterCount.Should().Be(19875);
        ////    stopwatch.ElapsedMilliseconds.Should().BeLessOrEqualTo(50);
        ////    this.output.WriteLine("Test duration: {0}ms", stopwatch.ElapsedMilliseconds);
        ////}

        private static IEnumerable<IClusteredGeoObject> ReadCsv(string filename)
        {
            using (StreamReader sr = new StreamReader(filename))
            {
                string currentLine;
                // currentLine will be null when the StreamReader reaches the end of file
                while ((currentLine = sr.ReadLine()) != null)
                {
                    var lonlat = currentLine.Split(';');
                    var lat = Convert.ToDouble(lonlat[0]);
                    var lon = Convert.ToDouble(lonlat[1]);

                    yield return new TestGeoObject { Location = new Position(lat, lon) };
                }
            }
        } 

        private static IEnumerable<IClusteredGeoObject> GenerateRandomTestGeoObjects(double count, Position centerPoint, double zoomLevel)
        {
            var list = new List<IClusteredGeoObject>();
            var rng = new Random();

            for (int i = 0; i < count; i++)
            {
                double lat = rng.Next(-516400146, 630304598) / 1000000000d;
                double lon = rng.Next(-224464416, 341194152) / 1000000000d;

                var position = new Position(centerPoint.Latitude + lat, centerPoint.Longitude + lon);

                list.Add(new TestGeoObject { Location = position });
            }

            return list;
        }

        [DebuggerDisplay("Longitude={Location.Longitude}, Latitude={Location.Latitude}")]
        private class TestGeoObject : IClusteredGeoObject
        {
            public Position Location { get; set; }
        }
    }
}
