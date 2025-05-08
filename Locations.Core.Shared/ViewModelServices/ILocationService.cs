using Locations.Core.Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locations.Core.Shared.ViewModelServices
{
    public interface ILocationService
    {
        public interface ILocationService
        {
            /// <summary>
            /// Event for error propagation
            /// </summary>
            event DataErrorEventHandler ErrorOccurred;

            /// <summary>
            /// Gets a location by ID
            /// </summary>
            Task<DataOperationResult<LocationViewModel>> GetLocationAsync(int id);

            /// <summary>
            /// Gets a location by coordinates
            /// </summary>
            Task<DataOperationResult<LocationViewModel>> GetLocationAsync(double latitude, double longitude);

            /// <summary>
            /// Gets all non-deleted locations
            /// </summary>
            Task<DataOperationResult<List<LocationViewModel>>> GetLocationsAsync();

            /// <summary>
            /// Saves a location with ID return
            /// </summary>
            Task<DataOperationResult<LocationViewModel>> SaveWithIDReturnAsync(LocationViewModel location);

            /// <summary>
            /// Saves a location with optional weather and return options
            /// </summary>
            Task<DataOperationResult<LocationViewModel>> SaveLocationAsync(
                LocationViewModel location,
                bool getWeather = false,
                bool returnNew = false);

            /// <summary>
            /// Updates a location
            /// </summary>
            Task<DataOperationResult<bool>> UpdateLocationAsync(LocationViewModel location);

            /// <summary>
            /// Deletes a location by ID
            /// </summary>
            Task<DataOperationResult<bool>> DeleteLocationAsync(int id);

            /// <summary>
            /// Deletes a location by coordinates
            /// </summary>
            Task<DataOperationResult<bool>> DeleteLocationAsync(double latitude, double longitude);

            /// <summary>
            /// Deletes a location by object
            /// </summary>
            Task<DataOperationResult<bool>> DeleteLocationAsync(LocationViewModel location);
        }
    }