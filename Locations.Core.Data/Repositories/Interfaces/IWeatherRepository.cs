using System.Threading.Tasks;
using Locations.Core.Data.Models;
using Locations.Core.Data.Queries.Interfaces;
using Locations.Core.Shared.ViewModels;

namespace Locations.Core.Data.Queries.Interfaces
{
    /// <summary>
    /// Repository interface for weather operations
    /// </summary>
    public interface IWeatherRepository : IRepository<WeatherViewModel>
    {
        /// <summary>
        /// Gets weather data by coordinates
        /// </summary>
        Task<DataOperationResult<WeatherViewModel>> GetByCoordinatesAsync(double latitude, double longitude);

        /// <summary>
        /// Gets weather data by coordinates or returns a new instance if not found
        /// </summary>
        Task<DataOperationResult<WeatherViewModel>> GetByCoordinatesOrDefaultAsync(double latitude, double longitude);
        Task<DataOperationResult<WeatherViewModel>> GetByLocationIdAsync(int locationId);
    }
}