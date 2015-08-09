using CrossPlatformLibrary.IoC;

namespace CrossPlatformLibrary.Maps
{
    public class ContainerExtension : IContainerExtension
    {
        public void Initialize(ISimpleIoc container)
        {
            container.RegisterPlatformSpecific<IExternalMaps>();
        }
    }
}
