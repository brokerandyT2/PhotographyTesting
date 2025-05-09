using Location.Core.Helpers.AlertService;
using Location.Core.Helpers.LoggingService;
using Location.Photography.Business.DataAccess.Interfaces;
using Location.Photography.Data.Queries;
using Locations.Core.Shared;
using Locations.Core.Shared.ViewModels;
using ILoggerService = Locations.Core.Business.DataAccess.Interfaces.ILoggerService;
namespace Location.Photography.Business.DataAccess
{
    /*
    /// <summary>
    /// Service for managing photography location data
    /// </summary>
    public class LocationService : Locations.Core.Business.DataAccess.LocationsService, ILocationService<LocationViewModel>
    {
        // Dependencies
        private readonly ILoggerService _logger;
        private readonly SettingsService _settingsService;
        private readonly LocationsQuery<LocationViewModel> _locationsQuery;

        /// <summary>
        /// Creates a new LocationService with logger and email
        /// </summary>
        public LocationService(ILoggerService logger, string email) : base(
            
            logger)
        {
            _logger = logger;
            _settingsService = new SettingsService();
            _locationsQuery = new LocationsQuery<LocationViewModel>();

            ValidateEmailSetting();
        }

        /// <summary>
        /// Creates a new LocationService with default dependencies
        /// </summary>
        public LocationService() : base(
            new AlertService(),
            new Locations.Core.Business.LoggingService.LoggerService())
        {
            _logger = new Locations.Core.Business.LoggingService.LoggerService();
            _settingsService = new SettingsService();
            _locationsQuery = new LocationsQuery<LocationViewModel>();

            ValidateEmailSetting();
        }

        /// <summary>
        /// Validates that email setting exists
        /// </summary>
        private void ValidateEmailSetting()
        {
            try
            {
                var email = _settingsService.GetSettingByName(MagicStrings.Email)?.Value;
                if (string.IsNullOrEmpty(email))
                {
                    throw new ArgumentException("Email is not set. Cannot use encrypted database.");
                }
            }
            catch (Exception ex)
            {
                // Handle the error directly instead of using base class methods
                _logger?.LogError("Email validation error", ex);
                Console.WriteLine($"Error validating email setting: {ex.Message}");
            }
        }

        #region ILocationService Implementation

        /// <summary>
        /// Deletes a location
        /// </summary>
        public bool Delete(LocationViewModel model)
        {
            if (model == null)
                return false;

            try
            {
                // Implement using LocationsQuery directly
                var result = _locationsQuery.DeleteItem(model);
                return result == 1 ? true:false ;
            }
            catch (Exception ex)
            {
                _logger?.LogError($"Error deleting location: {ex.Message}", ex);
                return false;
            }
        }

        /// <summary>
        /// Deletes a location by ID
        /// </summary>
        public bool Delete(int id)
        {
            try
            {
                // Implement using LocationsQuery directly
                var result = _locationsQuery.DeleteItem(id);
                return result;
            }
            catch (Exception ex)
            {
                _logger?.LogError($"Error deleting location with ID {id}: {ex.Message}", ex);
                return false;
            }
        }

        /// <summary>
        /// Deletes a location by coordinates
        /// </summary>
        public bool Delete(double latitude, double longitude)
        {
            try
            {
                // First, find the location by coordinates
                var location = _locationsQuery.G(latitude, longitude);
                if (location != null && !location.IsError)
                {
                    // Then delete by ID
                    return Delete(location.Id);
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger?.LogError($"Error deleting location at ({latitude}, {longitude}): {ex.Message}", ex);
                return false;
            }
        }

        /// <summary>
        /// Gets a location by ID
        /// </summary>
        public LocationViewModel Get(int id)
        {
            try
            {
                // Implement using LocationsQuery directly
                var result = _locationsQuery.GetItem<LocationViewModel>(id);
                return result.IsError ? new LocationViewModel() : result;
            }
            catch (Exception ex)
            {
                _logger?.LogError($"Error getting location with ID {id}: {ex.Message}", ex);
                return new LocationViewModel();
            }
        }

        /// <summary>
        /// Saves a location
        /// </summary>
        public LocationViewModel Save(LocationViewModel model)
        {
            return Save(model, false);
        }

        /// <summary>
        /// Saves a location with option to return a new instance
        /// </summary>
        public LocationViewModel Save(LocationViewModel model, bool returnNew)
        {
            if (model == null)
                return new LocationViewModel();

            try
            {
                // Implement using LocationsQuery directly
                var result = _locationsQuery.SaveItem(model);

                if (returnNew)
                    return new LocationViewModel();

                return result.IsError ? new LocationViewModel() : result;
            }
            catch (Exception ex)
            {
                _logger?.LogError($"Error saving location: {ex.Message}", ex);
                return new LocationViewModel();
            }
        }

        #endregion
    }*/
}