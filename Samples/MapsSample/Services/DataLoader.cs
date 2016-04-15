using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

using CrossPlatformLibrary.IO;

using MapsSample.Model;

namespace MapsSample.Services
{
    public class DataLoader : IDataLoader
    {
        public Task<List<Restaurant>> GetAllRestaurants()
        {
            return Task.Factory.StartNew(
                () =>
                    {
                        var assembly = this.GetType().GetTypeInfo().Assembly;
                        var restaurantsXmlContent = ResourceLoader.GetEmbeddedResourceString(assembly, "." + "cachedRestaurants.xml");
                        return restaurantsXmlContent.DeserializeFromXml<List<Restaurant>>();
                    });
        }
    }
}