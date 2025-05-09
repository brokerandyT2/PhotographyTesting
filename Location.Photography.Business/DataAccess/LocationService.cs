using Location.Core.Helpers.AlertService;
using Locations.Core.Business.DataAccess.Interfaces;
using Locations.Core.Data.Models;
using Locations.Core.Data.Queries.Interfaces;
using Locations.Core.Shared.ViewModels;
using DataErrorEventArgs = Locations.Core.Data.Models.DataErrorEventArgs;
using ILoggerService = Locations.Core.Business.DataAccess.Interfaces.ILoggerService;

namespace Location.Photography.Business.DataAccess
{
    /// <summary>
    /// Implementation of ILocationService for photography locations
    /// </summary>
    public class LocationService : ILocationService<LocationViewModel>
    {
        private readonly Locations.Core.Business.DataAccess.Services.LocationService<LocationViewModel> _coreLocationService;
        private readonly ILoggerService _logger;
        private readonly IAlertService _alertService;

        /// <summary>
        /// Initializes a new instance of the LocationService class
        /// </summary>
        /// <param name="locationRepository">Repository for location data</param>
        /// <param name="alertService">Service for user alerts</param>
        /// <param name="loggerService">Service for logging</param>
        public LocationService(
            ILocationRepository locationRepository,
            IAlertService alertService,
            ILoggerService loggerService)
        {
            _alertService = alertService ?? throw new ArgumentNullException(nameof(alertService));
            _logger = loggerService ?? throw new ArgumentNullException(nameof(loggerService));
            _coreLocationService = new Locations.Core.Business.DataAccess.Services.LocationService<LocationViewModel>(
                locationRepository,
                alertService,
                loggerService);

            // Forward events from the core service
            _coreLocationService.ErrorOccurred += CoreService_ErrorOccurred;
        }

        /// <summary>
        /// Event triggered when an error occurs in the service
        /// </summary>
        public event DataErrorEventHandler ErrorOccurred;

        /// <summary>
        /// Handles errors from the core service and forwards them
        /// </summary>
        private void CoreService_ErrorOccurred(object sender, DataErrorEventArgs e)
        {
            OnErrorOccurred(e);
        }

        /// <summary>
        /// Raises the error event
        /// </summary>
        public void OnErrorOccurred(DataErrorEventArgs e)
        {
            ErrorOccurred?.Invoke(this, e);
        }

        /// <summary>
        /// Gets a location by its coordinates
        /// </summary>
        public LocationViewModel GetLocationByCoordinates(double latitude, double longitude)
        {
            try
            {
                return _coreLocationService.GetLocationByCoordinates(latitude, longitude);
            }
            catch (Exception ex)
            {
                var error = new DataErrorEventArgs(ErrorSource.Unknown,
                    $"Error retrieving location at coordinates ({latitude}, {longitude}): {ex.Message}", ex);
                OnErrorOccurred(error);
                return new LocationViewModel();
            }
        }

        /// <summary>
        /// Gets all locations with full details including weather
        /// </summary>
        public async Task<List<LocationViewModel>> GetLocationsWithDetailsAsync()
        {
            try
            {
                return await _coreLocationService.GetLocationsWithDetailsAsync();
            }
            catch (Exception ex)
            {
                var error = new DataErrorEventArgs(ErrorSource.Unknown,
                    $"Error retrieving locations with details: {ex.Message}", ex);
                OnErrorOccurred(error);
                return new List<LocationViewModel>();
            }
        }

        /// <summary>
        /// Gets nearby locations within a radius
        /// </summary>
        public async Task<List<LocationViewModel>> GetNearbyLocationsAsync(double latitude, double longitude, double radiusKm = 10)
        {
            try
            {
                return await _coreLocationService.GetNearbyLocationsAsync(latitude, longitude, radiusKm);
            }
            catch (Exception ex)
            {
                var error = new DataErrorEventArgs(ErrorSource.Unknown,
                    $"Error retrieving nearby locations: {ex.Message}", ex);
                OnErrorOccurred(error);
                return new List<LocationViewModel>();
            }
        }

        /// <summary>
        /// Gets an entity by ID
        /// </summary>
        public async Task<DataOperationResult<LocationViewModel>> GetByIdAsync(int id)
        {
            return await _coreLocationService.GetByIdAsync(id);
        }

        /// <summary>
        /// Gets all entities
        /// </summary>
        public async Task<DataOperationResult<List<LocationViewModel>>> GetAllAsync()
        {
            return await _coreLocationService.GetAllAsync();
        }

        /// <summary>
        /// Saves an entity
        /// </summary>
        public async Task<DataOperationResult<LocationViewModel>> SaveAsync(LocationViewModel entity)
        {
            return await _coreLocationService.SaveAsync(entity);
        }

        /// <summary>
        /// Updates an existing entity
        /// </summary>
        public async Task<DataOperationResult<bool>> UpdateAsync(LocationViewModel entity)
        {
            return await _coreLocationService.UpdateAsync(entity);
        }

        /// <summary>
        /// Deletes an entity by ID
        /// </summary>
        public async Task<DataOperationResult<bool>> DeleteAsync(int id)
        {
            return await _coreLocationService.DeleteAsync(id);
        }

        /// <summary>
        /// Deletes an entity
        /// </summary>
        public async Task<DataOperationResult<bool>> DeleteAsync(LocationViewModel entity)
        {
            return await _coreLocationService.DeleteAsync(entity);
        }
    }
}