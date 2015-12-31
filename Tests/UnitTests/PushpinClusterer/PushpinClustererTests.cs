using System;
using System.Collections.Generic;
using System.Diagnostics;

using CrossPlatformLibrary.Geolocation;
using CrossPlatformLibrary.Maps.PushpinClusterer;

using FluentAssertions;

using Xunit;
using Xunit.Abstractions;

namespace UnitTests.PushpinClusterer
{
    public class PushpinClustererTests
    {
        private readonly ITestOutputHelper output;

        public PushpinClustererTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void ShouldRenderPinsInDefinedAmountOfTime()
        {
            // Arrange
            var stopwatch = new Stopwatch();
            int numberOfGeoObjects = 20000;
            int clusteringDistance = 50;
            var centerPoint = new Position(0.003d, 0.003d);
            double margin = 0.005d;
            var pins = GenerateTestGeoObjects(numberOfGeoObjects);
            var visiblePins = GetVisiblePoints(centerPoint, margin, pins);

            var pushpinClusterer = new PushpinClusterer<IClusteredGeoObject>(
                visiblePins,
                clusteringDistance);

            // Act
            stopwatch.Start();
            pushpinClusterer.RenderPins(0);
            stopwatch.Stop();

            // Assert
            pushpinClusterer.Should().NotBeNull();
            pushpinClusterer.PushpinModels.Should().HaveCount(1); //not always
            stopwatch.ElapsedMilliseconds.Should().BeLessOrEqualTo(50);
            this.output.WriteLine("Test duration: {0}ms", stopwatch.ElapsedMilliseconds);
        }

        private static IEnumerable<PositionToPointWrapper<IClusteredGeoObject>> GetVisiblePoints(Position centerPoint, double margin, IEnumerable<IClusteredGeoObject> clusteredGeoObjects)
        {
            foreach (var clusteredGeoObject in clusteredGeoObjects)
            {
                var isVisible = Math.Abs(clusteredGeoObject.Location.Latitude - centerPoint.Latitude) < margin &&
                                Math.Abs(clusteredGeoObject.Location.Longitude - centerPoint.Longitude) < margin;
                if (isVisible)
                {
                    yield return new PositionToPointWrapper<IClusteredGeoObject>(clusteredGeoObject, new Point(0, 0));
                }
            }
        }

        private static List<TestGeoObject> GenerateTestGeoObjects(double n)
        {
            var list = new List<TestGeoObject>();
            for (int i = 1; i <= n; i++)
            {
                list.Add(new TestGeoObject { Location = new Position(1 / (double)i, 1 / (double)i) });
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
