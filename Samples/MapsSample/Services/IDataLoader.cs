using System.Collections.Generic;
using System.Threading.Tasks;

using MapsSample.Model;

namespace MapsSample.Services
{
    public interface IDataLoader
    {
        Task<List<Restaurant>> GetAllRestaurants();
    }
}