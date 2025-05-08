using Location.Core.Helpers.LoggingService;
using Location.Photography.Business.DataAccess.Interfaces;
using Location.Photography.Data.Queries;
using Locations.Core.Business.AlertService;
using Locations.Core.Business.DataAccess;
using Locations.Core.Business.Logging.Interfaces;
using Locations.Core.Shared;
using Locations.Core.Shared.Customizations.Alerts.Implementation;
using Locations.Core.Shared.ViewModels;
using SQLite;
using System;
using System.Threading.Tasks;
// Import the error source from AlertService
using AlertServiceErrorSource = Location.Core.Helpers.AlertService.ErrorSource;

namespace Location.Photography.Business.DataAccess
{
    /// <summary>
    /// Service for managing photography location data
    /// </summary>
    public class LocationService : Locations.Core.Business.DataAccess.LocationsService, ILocationService<LocationViewModel>
    {
        // Dependencies
        private readonly ILoggerService _logger;
        private readonly SettingsService _settingsService;

        /// <summary>
        /// Creates a new LocationService with logger and email
        /// </summary>
        /// <param name="logger">Logger service instance</param>
        /// <param name="email">User email for encrypted storage</param>
        public LocationService(ILoggerService logger, string email) : base(
            new AlertService(), // Create alert service for base class
            logger)
        {
            _logger = logger;
            _settingsService = new SettingsService();

            ValidateEmailSetting();
        }

        /// <summary>
        /// Creates a new LocationService with default dependencies
        /// </summary>
        public LocationService() : base(
            new AlertService(),
            new Locations.Core.Business.LoggingService.LoggerService())
        {
            _settingsService = new SettingsService();

            ValidateEmailSetting();
        }

        /// <summary>
        /// Validates that email setting exists
        /// </summary>
        private void ValidateEmailSetting()
        {
            try
            {
                var email = _settingsService.GetSettingByName(MagicStrings.Email).Value;
                if (string.IsNullOrEmpty(email))
                {
                    throw new ArgumentException("Email is not set. Cannot use encrypted database.");
                }
            }
            catch (Exception ex)
            {
                // Use base class method with proper error source
                base.RaiseError(AlertServiceErrorSource.Validation, ex);

                // Also log locally
                _logger?.LogError("Email validation error", ex);
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
                var task = DeleteLocationAsync(model);
                task.Wait();
                return task.Result.IsSuccess;
            }
            catch (Exception ex)
            {
                // Use base class method with proper error source
                base.RaiseError(AlertServiceErrorSource.Unknown, ex);
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
                var task = DeleteLocationAsync(id);
                task.Wait();
                return task.Result.IsSuccess;
            }
            catch (Exception ex)
            {
                // Use base class method with proper error source
                base.RaiseError(AlertServiceErrorSource.Unknown, ex);
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
                var task = DeleteLocationAsync(latitude, longitude);
                task.Wait();
                return task.Result.IsSuccess;
            }
            catch (Exception ex)
            {
                // Use base class method with proper error source
                base.RaiseError(AlertServiceErrorSource.Unknown, ex);
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
                var task = GetLocationAsync(id);
                task.Wait();
                return task.Result.IsSuccess ? task.Result.Data : new LocationViewModel();
            }
            catch (Exception ex)
            {
                // Use base class method with proper error source
                base.RaiseError(AlertServiceErrorSource.Unknown, ex);
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
                var task = SaveLocationAsync(model, false, returnNew);
                task.Wait();
                return task.Result.IsSuccess ? task.Result.Data : new LocationViewModel();
            }
            catch (Exception ex)
            {
                // Use base class method with proper error source
                base.RaiseError(AlertServiceErrorSource.Unknown, ex);
                return new LocationViewModel();
            }
        }

        #endregion
    }
}