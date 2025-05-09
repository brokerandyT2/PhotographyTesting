using Locations.Core.Business.DataAccess.Base;
using Locations.Core.Shared.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Locations.Core.Business.DataAccess.Interfaces
{
    /// <summary>
    /// Interface for location service operations
    /// </summary>
    /// <typeparam name="T">The view model type for locations</typeparam>
    public interface ILocationService<T> : IServiceBase<T> where T : class, new()
    {
        /// <summary>
        /// Gets a location by its coordinates
        /// </summary>
        /// <param name="latitude">The latitude coordinate</param>
        /// <param name="longitude">The longitude coordinate</param>
        /// <returns>The location if found</returns>
        T GetLocationByCoordinates(double latitude, double longitude);

        /// <summary>
        /// Gets all locations with full details including weather
        /// </summary>
        /// <returns>A list of location view models with complete details</returns>
        Task<List<T>> GetLocationsWithDetailsAsync();

        /// <summary>
        /// Gets nearby locations within a radius
        /// </summary>
        /// <param name="latitude">Center latitude</param>
        /// <param name="longitude">Center longitude</param>
        /// <param name="radiusKm">Search radius in kilometers</param>
        /// <returns>List of locations within the radius</returns>
        Task<List<T>> GetNearbyLocationsAsync(double latitude, double longitude, double radiusKm = 10);
    }
}