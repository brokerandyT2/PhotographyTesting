using Locations.Core.Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Locations.Core.Shared.ViewModelServices
{
    /// <summary>
    /// Service interface for location-related operations
    /// </summary>
    public interface ILocationService
    {
        /// <summary>
        /// Event for error propagation
        /// </summary>
        event EventHandler<OperationErrorEventArgs> ErrorOccurred;

        /// <summary>
        /// Gets a location by ID
        /// </summary>
        Task<OperationResult<LocationViewModel>> GetLocationAsync(int id);

        /// <summary>
        /// Gets a location by coordinates
        /// </summary>
        Task<OperationResult<LocationViewModel>> GetLocationAsync(double latitude, double longitude);

        /// <summary>
        /// Gets all non-deleted locations
        /// </summary>
        Task<OperationResult<List<LocationViewModel>>> GetLocationsAsync();

        /// <summary>
        /// Saves a location with ID return
        /// </summary>
        Task<OperationResult<LocationViewModel>> SaveWithIDReturnAsync(LocationViewModel location);

        /// <summary>
        /// Saves a location with optional weather and return options
        /// </summary>
        Task<OperationResult<LocationViewModel>> SaveLocationAsync(
            LocationViewModel location,
            bool getWeather = false,
            bool returnNew = false);

        /// <summary>
        /// Updates a location
        /// </summary>
        Task<OperationResult<bool>> UpdateLocationAsync(LocationViewModel location);

        /// <summary>
        /// Deletes a location by ID
        /// </summary>
        Task<OperationResult<bool>> DeleteLocationAsync(int id);

        /// <summary>
        /// Deletes a location by coordinates
        /// </summary>
        Task<OperationResult<bool>> DeleteLocationAsync(double latitude, double longitude);

        /// <summary>
        /// Deletes a location by object
        /// </summary>
        Task<OperationResult<bool>> DeleteLocationAsync(LocationViewModel location);
    }
}