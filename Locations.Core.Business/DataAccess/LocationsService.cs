using Location.Core.Helpers.AlertService;
using Location.Core.Helpers.LoggingService;
using Locations.Core.Business.DataAccess.Interfaces;
using Locations.Core.Business.GeoLocation;
using Locations.Core.Business.Weather;
using Locations.Core.Data.Queries;
using Locations.Core.Shared;
using Locations.Core.Shared.ViewModels;
using Locations.Core.Business.DataAccess.
namespace Locations.Core.Business.DataAccess
{
    /// <summary>
    /// Service for managing location data with error bubbling
    /// </summary>
    public class LocationsService : ServiceBase<LocationViewModel>, ILocationService
    {
        private readonly LocationQuery _locationQuery;
        private readonly IAlertService _alertService;
        private readonly ILoggerService _loggerService;

        // Event for error bubbling to UI layer
        // Events for integration with our error bubbling system
        public event EventHandler<DataErrorEventArgs>? ErrorOccurred;

        /// <summary>
        /// Constructor with dependency injection
        /// </summary>
        public LocationsService(
            IAlertService alertService,
            ILoggerService loggerService)
        {
            _alertService = alertService ?? throw new ArgumentNullException(nameof(alertService));
            _loggerService = loggerService ?? throw new ArgumentNullException(nameof(loggerService));

            // Create location query with the same services
            _locationQuery = new LocationQuery(alertService, loggerService);

            // Subscribe to data layer errors
            _locationQuery.ErrorOccurred += OnQueryErrorOccurred;
           
        }
        
        /// <summary>
        /// Handler for data layer errors
        /// </summary>
        private void OnQueryErrorOccurred(object sender, DataErrorEventArgs e)
        {
            // Log the error at business layer level
            _loggerService.LogError($"Business layer received error: {e.Source} - {e.Message}", e.Exception);

            // Forward the error to UI layer
            OnErrorOccurred(e);

            // Show user-friendly alerts for critical errors
            if (e.Source == ErrorSource.Database)
            {
                _alertService.DisplayError("Database Error",
                    "There was a problem accessing location data. Please try again.", "OK");
            }
        }

        /// <summary>
        /// Raises the ErrorOccurred event
        /// </summary>


        /// <summary>
        /// Gets a location by ID
        /// </summary>
        public async Task<DataOperationResult<LocationViewModel>> GetLocationAsync(int id)
        {
            try
            {
                return await _locationQuery.GetItemAsync<LocationViewModel>(id);
            }
            catch (Exception ex)
            {
                string message = $"Unexpected error retrieving location with ID {id}";
                _loggerService.LogError(message, ex);

                var errorArgs = new DataErrorEventArgs(
                Location.Core.Helpers.AlertService.ErrorSource.Unknown,
                    message,
                    ex);

                OnErrorOccurred(errorArgs);

                return DataOperationResult<LocationViewModel>.Failure(
                    ErrorSource.Unknown,
                    message,
                    ex);
            }
        }

        /// <summary>
        /// Gets a location by coordinates
        /// </summary>
        public async Task<DataOperationResult<LocationViewModel>> GetLocationAsync(double latitude, double longitude)
        {
            try
            {
                return await _locationQuery.GetLocationAsync(latitude, longitude);
            }
            catch (Exception ex)
            {
                string message = $"Unexpected error retrieving location at coordinates ({latitude}, {longitude})";
                _loggerService.LogError(message, ex);

                var errorArgs = new DataErrorEventArgs(
                    ErrorSource.Unknown,
                    message,
                    ex);

                OnErrorOccurred(errorArgs);

                return DataOperationResult<LocationViewModel>.Failure(
                    ErrorSource.Unknown,
                    message,
                    ex);
            }
        }

        /// <summary>
        /// Gets all locations
        /// </summary>
        public async Task<DataOperationResult<List<LocationViewModel>>> GetLocationsAsync()
        {
            try
            {
                var result = await _locationQuery.GetItemsAsync<LocationViewModel>();

                if (!result.IsSuccess)
                {
                    return DataOperationResult<List<LocationViewModel>>.Failure(
                        result.Source,
                        result.ErrorMessage,
                        result.Exception);
                }

                // Filter out deleted locations
                var filteredLocations = result.Data.Where(x => x.IsDeleted == false).ToList();

                return DataOperationResult<List<LocationViewModel>>.Success(filteredLocations);
            }
            catch (Exception ex)
            {
                string message = "Unexpected error retrieving locations";
                _loggerService.LogError(message, ex);

                var errorArgs = new DataErrorEventArgs(
                    ErrorSource.Unknown,
                    message,
                    ex);

                OnErrorOccurred(errorArgs);

                return DataOperationResult<List<LocationViewModel>>.Failure(
                    ErrorSource.Unknown,
                    message,
                    ex);
            }
        }

        /// <summary>
        /// Saves a location with ID return
        /// </summary>
        public async Task<DataOperationResult<LocationViewModel>> SaveWithIDReturnAsync(LocationViewModel location)
        {
            try
            {
                if (location == null)
                {
                    string message = "Cannot save null location";
                    _loggerService.LogWarning(message);

                    var errorArgs = new DataErrorEventArgs(
                        ErrorSource.ModelValidation,
                        message,
                        null,
                        location);

                    OnErrorOccurred(errorArgs);

                    return DataOperationResult<LocationViewModel>.Failure(
                        ErrorSource.ModelValidation,
                        message);
                }

                return await _locationQuery.SaveWithIDReturnAsync(location);
            }
            catch (Exception ex)
            {
                string message = "Unexpected error saving location with ID return";
                _loggerService.LogError(message, ex);

                var errorArgs = new DataErrorEventArgs(
                    ErrorSource.Unknown,
                    message,
                    ex,
                    location);

                OnErrorOccurred(errorArgs);

                return DataOperationResult<LocationViewModel>.Failure(
                    ErrorSource.Unknown,
                    message,
                    ex);
            }
        }

        /// <summary>
        /// Saves a location with optional weather and return options
        /// </summary>
        public async Task<DataOperationResult<LocationViewModel>> SaveLocationAsync(
            LocationViewModel location,
            bool getWeather = false,
            bool returnNew = false)
        {
            try
            {
                if (location == null)
                {
                    string message = "Cannot save null location";
                    _loggerService.LogWarning(message);

                    var errorArgs = new DataErrorEventArgs(
                        ErrorSource.ModelValidation,
                        message,
                        null,
                        location);

                    OnErrorOccurred(errorArgs);

                    return DataOperationResult<LocationViewModel>.Failure(
                        ErrorSource.ModelValidation,
                        message);
                }

                // Get city and state data
                try
                {
                    GeoLocationAPI geoLocationAPI = new GeoLocationAPI(ref location);
                    geoLocationAPI.GetCityAndState(location.Lattitude, location.Longitude);
                }
                catch (Exception ex)
                {
                    // Log but continue - geo data is not critical
                   // _loggerService.LogWarning($"Could not get geo location data: {ex.Message}", ex);
                }

                // Get weather data if requested
                if (getWeather)
                {
                    try
                    {
                        var settingsService = new SettingsService();
                        var apiKey = settingsService.GetSettingByName(MagicStrings.Weather_API_Key).Value;
                        var weatherUrl = settingsService.GetSettingByName(MagicStrings.WeatherURL).Value; ;

                        WeatherAPI weatherAPI = new WeatherAPI(
                            apiKey,
                            location.Lattitude,
                            location.Longitude,
                            weatherUrl);

                        var weatherData = await weatherAPI.GetWeatherAsync();

                        var weatherService = new WeatherService();
                      
                    }
                    catch (Exception ex)
                    {
                        // Log but continue - weather data is not critical
                        //_loggerService.LogWarning($"Could not get weather data: {ex.Message}", ex);
                    }
                }

                // Save the location
                var saveResult = await _locationQuery.SaveItemAsync(location);

                if (!saveResult.IsSuccess)
                {
                    return DataOperationResult<LocationViewModel>.Failure(
                        saveResult.Source,
                        saveResult.ErrorMessage,
                        saveResult.Exception);
                }

                // Return new or existing location based on parameter
                return DataOperationResult<LocationViewModel>.Success(
                    returnNew ? new LocationViewModel() : location);
            }
            catch (Exception ex)
            {
                string message = "Unexpected error saving location";
                _loggerService.LogError(message, ex);

                var errorArgs = new DataErrorEventArgs(
                    ErrorSource.Unknown,
                    message,
                    ex,
                    location);

                OnErrorOccurred(errorArgs);

                return DataOperationResult<LocationViewModel>.Failure(
                    ErrorSource.Unknown,
                    message,
                    ex);
            }
        }

        /// <summary>
        /// Updates a location
        /// </summary>
        public async Task<DataOperationResult<bool>> UpdateLocationAsync(LocationViewModel location)
        {
            try
            {
                if (location == null)
                {
                    string message = "Cannot update null location";
                    _loggerService.LogWarning(message);

                    var errorArgs = new DataErrorEventArgs(
                        ErrorSource.ModelValidation,
                        message,
                        null,
                        location);

                    OnErrorOccurred(errorArgs);

                    return DataOperationResult<bool>.Failure(
                        ErrorSource.ModelValidation,
                        message);
                }

                return await _locationQuery.UpdateAsync(location);
            }
            catch (Exception ex)
            {
                string message = "Unexpected error updating location";
                _loggerService.LogError(message, ex);

                var errorArgs = new DataErrorEventArgs(
                    ErrorSource.Unknown,
                    message,
                    ex,
                    location);

                OnErrorOccurred(errorArgs);

                return DataOperationResult<bool>.Failure(
                    ErrorSource.Unknown,
                    message,
                    ex);
            }
        }

        /// <summary>
        /// Deletes a location by ID
        /// </summary>
        public async Task<DataOperationResult<bool>> DeleteLocationAsync(int id)
        {
            try
            {
                return await _locationQuery.DeleteItemAsync<LocationViewModel>(id);
            }
            catch (Exception ex)
            {
                string message = $"Unexpected error deleting location with ID {id}";
                _loggerService.LogError(message, ex);

                var errorArgs = new DataErrorEventArgs(
                    ErrorSource.Unknown,
                    message,
                    ex);

                OnErrorOccurred(errorArgs);

                return DataOperationResult<bool>.Failure(
                    ErrorSource.Unknown,
                    message,
                    ex);
            }
        }

        /// <summary>
        /// Deletes a location by coordinates
        /// </summary>
        public async Task<DataOperationResult<bool>> DeleteLocationAsync(double latitude, double longitude)
        {
            try
            {
                // First get the location
                var getResult = await _locationQuery.GetLocationAsync(latitude, longitude);

                if (!getResult.IsSuccess)
                {
                    return DataOperationResult<bool>.Failure(
                        getResult.Source,
                        getResult.ErrorMessage,
                        getResult.Exception);
                }

                // Then delete it by ID
                return await DeleteLocationAsync(getResult.Data.Id);
            }
            catch (Exception ex)
            {
                string message = $"Unexpected error deleting location at coordinates ({latitude}, {longitude})";
                _loggerService.LogError(message, ex);

                var errorArgs = new DataErrorEventArgs(
                    ErrorSource.Unknown,
                    message,
                    ex);

                OnErrorOccurred(errorArgs);

                return DataOperationResult<bool>.Failure(
                    ErrorSource.Unknown,
                    message,
                    ex);
            }
        }

        /// <summary>
        /// Deletes a location by object
        /// </summary>
        public async Task<DataOperationResult<bool>> DeleteLocationAsync(LocationViewModel location)
        {
            try
            {
                if (location == null)
                {
                    string message = "Cannot delete null location";
                    _loggerService.LogWarning(message);

                    var errorArgs = new DataErrorEventArgs(
                        ErrorSource.ModelValidation,
                        message,
                        null,
                        location);

                    OnErrorOccurred(errorArgs);

                    return DataOperationResult<bool>.Failure(
                        ErrorSource.ModelValidation,
                        message);
                }

                return await DeleteLocationAsync(location.Id);
            }
            catch (Exception ex)
            {
                string message = "Unexpected error deleting location";
                _loggerService.LogError(message, ex);

                var errorArgs = new DataErrorEventArgs(
                    ErrorSource.Unknown,
                    message,
                    ex,
                    location);

                OnErrorOccurred(errorArgs);

                return DataOperationResult<bool>.Failure(
                    ErrorSource.Unknown,
                    message,
                    ex);
            }
        }
    }
}