using Locations.Core.Business.DataAccess.Base;
using Locations.Core.Shared.ViewModels;
using System.Threading.Tasks;

namespace Locations.Core.Business.DataAccess.Interfaces
{
    /// <summary>
    /// Interface for weather service operations
    /// </summary>
    /// <typeparam name="T">The view model type for weather</typeparam>
    public interface IWeatherService<T> : IServiceBase<T> where T : class, new()
    {
        /// <summary>
        /// Gets weather for a specific location
        /// </summary>
        /// <param name="locationId">The location ID</param>
        /// <returns>The weather view model</returns>
        Task<Locations.Core.Shared.ViewModels.OperationResult<T>> GetWeatherForLocationAsync(int locationId);

        /// <summary>
        /// Gets weather by geographic coordinates
        /// </summary>
        /// <param name="latitude">The latitude coordinate</param>
        /// <param name="longitude">The longitude coordinate</param>
        /// <returns>The weather view model</returns>
        Task<Locations.Core.Shared.ViewModels.OperationResult<T>> GetWeatherByCoordinatesAsync(double latitude, double longitude);

        /// <summary>
        /// Gets forecast for a location for a specified number of days
        /// </summary>
        /// <param name="locationId">The location ID</param>
        /// <param name="days">Number of days to forecast</param>
        /// <returns>The forecast data</returns>
        Task<Locations.Core.Shared.ViewModels.OperationResult<string>> GetForecastForLocationAsync(int locationId, int days = 5);
    }
}