using System;
using System.Collections;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

using CrossPlatformLibrary.Geolocation;
using CrossPlatformLibrary.Maps.Controls;
using CrossPlatformLibrary.Maps.PushpinClusterer;

using Microsoft.Phone.Maps.Controls;

using Point = CrossPlatformLibrary.Maps.PushpinClusterer.Point;

namespace CrossPlatformLibrary.Maps
{
    /// <summary>
    ///     Class MapPushPinDependency.
    ///     Source: http://stackoverflow.com/questions/16417978/mvvm-windows-phone-8-adding-a-collection-of-pushpins-to-a-map.
    /// </summary>
    public static class MapPushPinDependency
    {
        private static readonly int CurrentLocationLayerIndex = 0;
        private static readonly int ItemsSourceLayerIndex = 1;

        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.RegisterAttached(
            "ItemsSource",
            typeof(IEnumerable<IClusteredGeoObject>),
            typeof(MapPushPinDependency),
            new PropertyMetadata(OnItemsSourcePropertyChanged));

        public static readonly DependencyProperty CurrentLocationProperty = DependencyProperty.RegisterAttached(
            "CurrentLocation",
            typeof(GeoCoordinate),
            typeof(MapPushPinDependency),
            new PropertyMetadata(OnCurrentLocationPropertyChanged));

        public static readonly DependencyProperty ClusterTemplateProperty = DependencyProperty.RegisterAttached(
            "ClusterTemplate",
            typeof(DataTemplate),
            typeof(MapPushPinDependency),
            new PropertyMetadata(default(DataTemplate)));

        public static readonly DependencyProperty PushpinTemplateProperty = DependencyProperty.RegisterAttached(
            "PushpinTemplate",
            typeof(DataTemplate),
            typeof(MapPushPinDependency),
            new PropertyMetadata(default(DataTemplate)));

        public static readonly DependencyProperty CurrentLocationPushpinTemplateProperty = DependencyProperty.RegisterAttached(
            "CurrentLocationPushpinTemplate",
            typeof(DataTemplate),
            typeof(MapPushPinDependency),
            new PropertyMetadata(default(DataTemplate)));

        public static readonly DependencyProperty ClusteringDistanceProperty = DependencyProperty.RegisterAttached(
            "ClusteringDistance",
            typeof(int),
            typeof(MapPushPinDependency),
            new PropertyMetadata(default(int)));

        private static void OnItemsSourcePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Task.Factory.StartNew(() => UpdateItemsSource(d, e), CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private static void UpdateItemsSource(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var map = d as Map;
            var newPushPinCollection = e.NewValue as IEnumerable<IClusteredGeoObject>;
            if (newPushPinCollection != null && newPushPinCollection.Any() && map != null)
            {
                // Read dependency properties
                var pushpinTemplate = GetPushpinTemplate(d);
                var clusterPushpinTemplate = GetClusterTemplate(d);
                var clusteringDistance = GetClusteringDistance(d);

                // Perform clustering
                Func<Position, bool> checkIsVisiblePoint =
                    position => position != null && map.IsVisiblePoint(map.ConvertGeoCoordinateToViewportPoint(position.ToGeoCoordinate()));

                Func<Position, Point> getViewportPoint = position =>
                    {
                        var p = map.ConvertGeoCoordinateToViewportPoint(position.ToGeoCoordinate());
                        return new Point(p.X, p.Y);
                    };

                var clusterer = new PushpinClusterer<IClusteredGeoObject>(newPushPinCollection, clusteringDistance, checkIsVisiblePoint, getViewportPoint);
                clusterer.RenderPins(map.ZoomLevel);

                // Update map on UI
                map.Dispatcher.BeginInvoke(
                    () =>
                        {
                            // Remove layer which hosts pushpins
                            var clusteredPushpinLayer = map.Layers.FirstOrDefault(x => x.All(y => y.Content is ClusteredPushpinItem<IClusteredGeoObject>));
                            map.Layers.Remove(clusteredPushpinLayer);

                            // Create new layer
                            var layer = new MapLayer();

                            // The descending-by-latitude ordering makes sure the pins are not overlapping eachother
                            Position lastGeoCoordinate = Position.Unknown;
                            foreach (var pushPinModel in clusterer.PushpinModels.OrderByDescending(p => p.Location.Latitude))
                            {
                                var mapOverlay = new MapOverlay
                                {
                                    GeoCoordinate = new GeoCoordinate(pushPinModel.Location.Latitude, pushPinModel.Location.Longitude),
                                    Content = pushPinModel
                                };

                                if (pushPinModel.IsClustered)
                                {
                                    mapOverlay.ContentTemplate = clusterPushpinTemplate;
                                    mapOverlay.PositionOrigin = new System.Windows.Point(0.5, 0.5);
                                }
                                else
                                {
                                    mapOverlay.ContentTemplate = pushpinTemplate;

                                    // In case two/more pins have exactly the same GPS position, we add a random offset to longitude/latitude.
                                    if (pushPinModel.Location == lastGeoCoordinate)
                                    {
                                        mapOverlay.GeoCoordinate = lastGeoCoordinate.WithRandomOffset().ToGeoCoordinate();
                                    }
                                    lastGeoCoordinate = pushPinModel.Location;
                                }

                                layer.Add(mapOverlay);
                            }

                            // Finally, add all items (pushpins, clusters and current location) to the map.
                            if (layer.Any())
                            {
                                map.Layers.Insert(ItemsSourceLayerIndex, layer);
                            }
                        });
            }
        }

        private static void OnCurrentLocationPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Task.Factory.StartNew(() => UpdateCurrentLocation(d, e), CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private static void UpdateCurrentLocation(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var map = d as Map;
            var currentGeoCoordinate = e.NewValue as GeoCoordinate;
            if (currentGeoCoordinate != null && map != null)
            {
                DataTemplate currentLocationPushpinTemplate = GetCurrentLocationPushpinTemplate(d);

                map.Dispatcher.BeginInvoke(
                    () =>
                        {
                            // Remove layer which hosts pushpins
                            var removeLayer = map.Layers.FirstOrDefault(x => x.All(y => y.Content is UserLocationMarker));
                            map.Layers.Remove(removeLayer);

                            // Create new layer with current location pin
                            var layer = new MapLayer
                                            {
                                                new MapOverlay
                                                    {
                                                        GeoCoordinate = currentGeoCoordinate,
                                                        Content = new UserLocationMarker(currentGeoCoordinate),
                                                        ContentTemplate = currentLocationPushpinTemplate,
                                                        PositionOrigin = new System.Windows.Point(0.5, 0.5)
                                                    }
                                            };

                            if (layer.Any())
                            {
                                map.Layers.Insert(CurrentLocationLayerIndex, layer);
                            }
                        });
            }
        }

        #region Getters and Setters

        /// <summary>
        ///     Gets the items source.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>The IEnumerable.</returns>
        public static IEnumerable GetItemsSource(DependencyObject obj)
        {
            return (IEnumerable)obj.GetValue(ItemsSourceProperty);
        }

        /// <summary>
        ///     Sets the items source.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="value">The value.</param>
        public static void SetItemsSource(DependencyObject obj, IEnumerable value)
        {
            obj.SetValue(ItemsSourceProperty, value);
        }

        /// <summary>
        ///     Gets the current location.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>The current GeoCoordinate.</returns>
        public static GeoCoordinate GetCurrentLocation(DependencyObject obj)
        {
            return (GeoCoordinate)obj.GetValue(CurrentLocationProperty);
        }

        /// <summary>
        ///     Sets the current location.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="value">The value.</param>
        public static void SetCurrentLocation(DependencyObject obj, GeoCoordinate value)
        {
            obj.SetValue(CurrentLocationProperty, value);
        }

        /// <summary>
        ///     Sets the cluster template.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="value">The value.</param>
        public static void SetClusterTemplate(DependencyObject obj, DataTemplate value)
        {
            obj.SetValue(ClusterTemplateProperty, value);
        }

        /// <summary>
        ///     Gets the cluster template.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>The DataTemplate.</returns>
        public static DataTemplate GetClusterTemplate(DependencyObject obj)
        {
            return (DataTemplate)obj.GetValue(ClusterTemplateProperty);
        }

        /// <summary>
        ///     Sets the pushpin template.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="value">The value.</param>
        public static void SetPushpinTemplate(DependencyObject obj, DataTemplate value)
        {
            obj.SetValue(PushpinTemplateProperty, value);
        }

        /// <summary>
        ///     Gets the pushpin template.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>The DataTemplate.</returns>
        public static DataTemplate GetPushpinTemplate(DependencyObject obj)
        {
            return (DataTemplate)obj.GetValue(PushpinTemplateProperty);
        }

        /// <summary>
        ///     Sets the current location pushpin template.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="value">The value.</param>
        public static void SetCurrentLocationPushpinTemplate(DependencyObject obj, DataTemplate value)
        {
            obj.SetValue(CurrentLocationPushpinTemplateProperty, value);
        }

        /// <summary>
        ///     Geturrents the location pushpin template.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>The DataTemplate.</returns>
        public static DataTemplate GetCurrentLocationPushpinTemplate(DependencyObject obj)
        {
            return (DataTemplate)obj.GetValue(CurrentLocationPushpinTemplateProperty);
        }

        /// <summary>
        ///     Sets the clustering distance.
        ///     If set to zero, clustering is turned off.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="value">The value.</param>
        public static void SetClusteringDistance(DependencyObject obj, int value)
        {
            obj.SetValue(ClusteringDistanceProperty, value);
        }

        /// <summary>
        ///     Gets the clustering distance.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>The clustering distance.</returns>
        public static int GetClusteringDistance(DependencyObject obj)
        {
            return (int)obj.GetValue(ClusteringDistanceProperty);
        }

        #endregion
    }
}