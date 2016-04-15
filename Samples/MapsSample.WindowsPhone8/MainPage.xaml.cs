using System;
using System.Windows;

using CrossPlatformLibrary.Tracing;

using Microsoft.Phone.Controls;
using Microsoft.Phone.Maps;
using Microsoft.Phone.Maps.Controls;

namespace MapsSample.WindowsPhone8
{
    public partial class MainPage : PhoneApplicationPage
    {
        private ITracer tracer;

        public MainPage()
        {
            this.InitializeComponent();
            this.tracer = Tracer.Create(this);
        }

        private void OnLoadedSetTokenIds(object sender, RoutedEventArgs e)
        {
            //MapsSettings.ApplicationContext.ApplicationId = "....";
            //MapsSettings.ApplicationContext.AuthenticationToken = "....";
        }

        private void Map_OnResolveCompleted(object sender, MapResolveCompletedEventArgs e)
        {
            this.tracer.Error("Map_OnResolveCompleted");
        }
    }
}