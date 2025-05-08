using Locations.Core.Business.DataAccess.Interfaces;
using Locations.Core.Shared.ViewModels;
using Locations.Core.Shared.ViewModelServices;
namespace Locations.Core.Business.DataAccess;

/// <summary>
/// Interface for location management service
/// </summary>
public interface ILocationService: IBaseInterface
{


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