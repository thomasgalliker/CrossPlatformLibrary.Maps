using System;
using System.Reflection;

using CrossPlatformLibrary.Bootstrapping;
using CrossPlatformLibrary.ExceptionHandling.Handlers;
using CrossPlatformLibrary.IoC;
using CrossPlatformLibrary.IO;
using CrossPlatformLibrary.Tracing;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Views;

using MapsSample.Services;
using MapsSample.ViewModels;

namespace MapsSample.Bootstrapping
{
    public class MapsSampleBootstrapper : Bootstrapper
    {
        private readonly ITracer tracer;

        public MapsSampleBootstrapper()
        {
            this.tracer = Tracer.Create(this);
        }

        protected override void OnStartup()
        {
            this.tracer.Info("App is starting up...");
        }

        protected override void ConfigureContainer(ISimpleIoc container)
        {
            // Registering platform-independent services
            container.Register<IDataLoader, DataLoader>();

            // Registering view models
            //container.Register<MainViewModel>();
            container.Register<RestaurantMapViewModel>();
        }

        protected override Type GetExceptionHandlerType()
        {
            return typeof(TracingExceptionHandler);
        }

        protected override void OnShutdown()
        {
            this.tracer.Info("App is shutting down...");
        }
    }
}
