using Location.Core.Helpers.AlertService;
using Location.Core.Helpers.LoggingService;
using Locations.Core.Business.DataAccess.Interfaces;
using Locations.Core.Data.Models;
using Locations.Core.Data.Queries.Interfaces;
using Locations.Core.Shared.DTO;
using Locations.Core.Shared.ViewModels;
using DataErrorEventArgs = Locations.Core.Data.Models.DataErrorEventArgs;
using ErrorSource = Locations.Core.Data.Models.ErrorSource;


namespace Locations.Core.Business.DataAccess.Services
{
    /// <summary>
    /// Service for weather-related operations
    /// </summary>
    /// <typeparam name="T">The view model type for weather</typeparam>
    public class WeatherService<T> : IWeatherService<T>
        where T : WeatherViewModel, new()
    {
        private readonly IWeatherRepository _repository;
        private readonly IAlertService _alertService;
        private readonly ILoggerService _loggerService;
        private readonly ILocationService<LocationViewModel> _locationService;

        /// <summary>
        /// Event triggered when an error occurs in the service
        /// </summary>
        public event DataErrorEventHandler ErrorOccurred;

        /// <summary>
        /// Creates a new instance of the WeatherService with dependencies
        /// </summary>
        /// <param name="repository">The weather repository</param>
        /// <param name="alertService">The alert service</param>
        /// <param name="loggerService">The logger service</param>
        /// <param name="locationService">The location service for retrieving locations</param>
        public WeatherService(
            IWeatherRepository repository,
            IAlertService alertService,
            ILoggerService loggerService,
            ILocationService<LocationViewModel> locationService = null)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _alertService = alertService ?? throw new ArgumentNullException(nameof(alertService));
            _loggerService = loggerService ?? throw new ArgumentNullException(nameof(loggerService));
            _locationService = locationService;

            // Connect repository events
            if (_repository is IWeatherRepository repoWithEvents)
            {
                repoWithEvents.ErrorOccurred += RepositoryOnErrorOccurred;
            }
        }

        /// <summary>
        /// Handles errors from the repository
        /// </summary>
        private void RepositoryOnErrorOccurred(object sender, DataErrorEventArgs e)
        {
            // Log the error
            _loggerService.LogError(e.Message, e.Exception);

            // Forward the error event
            OnErrorOccurred(e);
        }

        /// <summary>
        /// Raises the error event
        /// </summary>
        public void OnErrorOccurred(DataErrorEventArgs e)
        {
            // Log the error
            _loggerService.LogError(e.Message, e.Exception);

            // Show alert for UI
            if (_alertService != null)
            {
                if (e.Exception != null)
                {
                    _loggerService.LogError($"{e.Source}: {e.Message}", e.Exception);
                }
                else
                {
                    _loggerService.LogWarning($"{e.Source}: {e.Message}");
                }
            }

            // Raise the event
            ErrorOccurred?.Invoke(this, e);
        }

        /// <summary>
        /// Creates a new error event
        /// </summary>
        protected virtual DataErrorEventArgs CreateErrorEventArgs(ErrorSource source, string message, Exception ex = null)
        {
            return new DataErrorEventArgs(source, message, ex);
        }

        /// <summary>
        /// Gets an entity by ID
        /// </summary>
        public virtual async Task<DataOperationResult<T>> GetByIdAsync(int id)
        {
            try
            {
                var result = await _repository.GetByIdAsync(id);

                if (result.IsSuccess && result.Data != null)
                {
                    // Convert the result to the expected type T
                    var viewModel = new T();
                    // Create a WeatherDTO from the repository data
                    var dto = new WeatherDTO
                    {
                        Id = result.Data.Id,
                        LocationId = result.Data.LocationId,
                        Timestamp = result.Data.Timestamp,
                        Temperature = result.Data.Temperature,
                        Description = result.Data.Description,
                        WindSpeed = result.Data.WindSpeed,
                        WindDirection = result.Data.WindDirection,
                        Humidity = result.Data.Humidity,
                        Pressure = result.Data.Pressure,
                        Visibility = result.Data.Visibility,
                        Precipitation = result.Data.Precipitation,
                        SunriseTime = result.Data.SunriseTime,
                        SunsetTime = result.Data.SunsetTime,
                        CloudCover = result.Data.CloudCover,
                        FeelsLike = result.Data.FeelsLike,
                        UVIndex = result.Data.UVIndex,
                        AirQuality = result.Data.AirQuality,
                        LastUpdate = result.Data.LastUpdate,
                        Forecast = result.Data.Forecast,
                        IsDeleted = result.Data.IsDeleted
                    };
                    viewModel.InitializeFromDTO(dto);
                    return DataOperationResult<T>.Success(viewModel);
                }
                else
                {
                    // Create a new failure result for type T
                    return DataOperationResult<T>.Failure(
                        result.ErrorSource,
                        $"Failed to retrieve weather with ID {id}",
                        result.Exception);
                }
            }
            catch (Exception ex)
            {
                var errorArgs = CreateErrorEventArgs(ErrorSource.Unknown,
                    $"Error retrieving weather with ID {id}: {ex.Message}", ex);
                OnErrorOccurred(errorArgs);
                return DataOperationResult<T>.Failure(ErrorSource.Unknown, errorArgs.Message, ex);
            }
        }

        /// <summary>
        /// Gets all entities
        /// </summary>
        public virtual async Task<DataOperationResult<List<T>>> GetAllAsync()
        {
            try
            {
                var result = await _repository.GetAllAsync();

                if (result.IsSuccess && result.Data != null)
                {
                    // Convert the results to a List<T>
                    var viewModels = new List<T>();
                    foreach (var item in result.Data)
                    {
                        var viewModel = new T();
                        // Create a WeatherDTO from the repository data
                        var dto = new WeatherDTO
                        {
                            Id = item.Id,
                            LocationId = item.LocationId,
                            Timestamp = item.Timestamp,
                            Temperature = item.Temperature,
                            Description = item.Description,
                            WindSpeed = item.WindSpeed,
                            WindDirection = item.WindDirection,
                            Humidity = item.Humidity,
                            Pressure = item.Pressure,
                            Visibility = item.Visibility,
                            Precipitation = item.Precipitation,
                            SunriseTime = item.SunriseTime,
                            SunsetTime = item.SunsetTime,
                            CloudCover = item.CloudCover,
                            FeelsLike = item.FeelsLike,
                            UVIndex = item.UVIndex,
                            AirQuality = item.AirQuality,
                            LastUpdate = item.LastUpdate,
                            Forecast = item.Forecast,
                            IsDeleted = item.IsDeleted
                        };
                        viewModel.InitializeFromDTO(dto);
                        viewModels.Add(viewModel);
                    }
                    return DataOperationResult<List<T>>.Success(viewModels);
                }
                else
                {
                    // Create a new failure result
                    return DataOperationResult<List<T>>.Failure(
                        result.ErrorSource,
                        "Failed to retrieve all weather data",
                        result.Exception);
                }
            }
            catch (Exception ex)
            {
                var errorArgs = CreateErrorEventArgs(ErrorSource.Unknown,
                    $"Error retrieving all weather data: {ex.Message}", ex);
                OnErrorOccurred(errorArgs);
                return DataOperationResult<List<T>>.Failure(ErrorSource.Unknown, errorArgs.Message, ex);
            }
        }

        /// <summary>
        /// Saves an entity
        /// </summary>
        public virtual async Task<DataOperationResult<T>> SaveAsync(T entity)
        {
            try
            {
                if (entity == null)
                {
                    throw new InvalidOperationException("Entity cannot be null");
                }

                // Create a WeatherViewModel from the entity
                var weatherViewModel = new WeatherViewModel
                {
                    Id = entity.Id,
                    LocationId = entity.LocationId,
                    Timestamp = entity.Timestamp,
                    Temperature = entity.Temperature,
                    Description = entity.Description,
                    WindSpeed = entity.WindSpeed,
                    WindDirection = entity.WindDirection,
                    Humidity = entity.Humidity,
                    Pressure = entity.Pressure,
                    Visibility = entity.Visibility,
                    Precipitation = entity.Precipitation,
                    SunriseTime = entity.SunriseTime,
                    SunsetTime = entity.SunsetTime,
                    CloudCover = entity.CloudCover,
                    FeelsLike = entity.FeelsLike,
                    UVIndex = entity.UVIndex,
                    AirQuality = entity.AirQuality,
                    LastUpdate = entity.LastUpdate,
                    Forecast = entity.Forecast,
                    IsDeleted = entity.IsDeleted
                };

                var result = await _repository.SaveAsync(weatherViewModel);

                if (result.IsSuccess && result.Data != null)
                {
                    // Convert the result back to T using a DTO
                    var dto = new WeatherDTO
                    {
                        Id = result.Data.Id,
                        LocationId = result.Data.LocationId,
                        Timestamp = result.Data.Timestamp,
                        Temperature = result.Data.Temperature,
                        Description = result.Data.Description,
                        WindSpeed = result.Data.WindSpeed,
                        WindDirection = result.Data.WindDirection,
                        Humidity = result.Data.Humidity,
                        Pressure = result.Data.Pressure,
                        Visibility = result.Data.Visibility,
                        Precipitation = result.Data.Precipitation,
                        SunriseTime = result.Data.SunriseTime,
                        SunsetTime = result.Data.SunsetTime,
                        CloudCover = result.Data.CloudCover,
                        FeelsLike = result.Data.FeelsLike,
                        UVIndex = result.Data.UVIndex,
                        AirQuality = result.Data.AirQuality,
                        LastUpdate = result.Data.LastUpdate,
                        Forecast = result.Data.Forecast,
                        IsDeleted = result.Data.IsDeleted
                    };
                    entity.InitializeFromDTO(dto);
                    return DataOperationResult<T>.Success(entity);
                }
                else
                {
                    // Create a new failure result
                    return DataOperationResult<T>.Failure(
                        result.ErrorSource,
                        "Failed to save weather data",
                        result.Exception);
                }
            }
            catch (Exception ex)
            {
                var errorArgs = CreateErrorEventArgs(ErrorSource.Unknown,
                    $"Error saving weather data: {ex.Message}", ex);
                OnErrorOccurred(errorArgs);
                return DataOperationResult<T>.Failure(ErrorSource.Unknown, errorArgs.Message, ex);
            }
        }

        /// <summary>
        /// Updates an existing entity
        /// </summary>
        public virtual async Task<DataOperationResult<bool>> UpdateAsync(T entity)
        {
            try
            {
                if (entity == null)
                {
                    throw new InvalidOperationException("Entity cannot be null");
                }

                // Create a WeatherViewModel from the entity
                var weatherViewModel = new WeatherViewModel
                {
                    Id = entity.Id,
                    LocationId = entity.LocationId,
                    Timestamp = entity.Timestamp,
                    Temperature = entity.Temperature,
                    Description = entity.Description,
                    WindSpeed = entity.WindSpeed,
                    WindDirection = entity.WindDirection,
                    Humidity = entity.Humidity,
                    Pressure = entity.Pressure,
                    Visibility = entity.Visibility,
                    Precipitation = entity.Precipitation,
                    SunriseTime = entity.SunriseTime,
                    SunsetTime = entity.SunsetTime,
                    CloudCover = entity.CloudCover,
                    FeelsLike = entity.FeelsLike,
                    UVIndex = entity.UVIndex,
                    AirQuality = entity.AirQuality,
                    LastUpdate = entity.LastUpdate,
                    Forecast = entity.Forecast,
                    IsDeleted = entity.IsDeleted
                };

                var result = await _repository.UpdateAsync(weatherViewModel);
                return result; // This already returns DataOperationResult<bool>
            }
            catch (Exception ex)
            {
                var errorArgs = CreateErrorEventArgs(ErrorSource.Unknown,
                    $"Error updating weather data: {ex.Message}", ex);
                OnErrorOccurred(errorArgs);
                return DataOperationResult<bool>.Failure(ErrorSource.Unknown, errorArgs.Message, ex);
            }
        }

        /// <summary>
        /// Deletes an entity by ID
        /// </summary>
        public virtual async Task<DataOperationResult<bool>> DeleteAsync(int id)
        {
            try
            {
                var result = await _repository.DeleteAsync(id);
                return result; // This already returns DataOperationResult<bool>
            }
            catch (Exception ex)
            {
                var errorArgs = CreateErrorEventArgs(ErrorSource.Unknown,
                    $"Error deleting weather data with ID {id}: {ex.Message}", ex);
                OnErrorOccurred(errorArgs);
                return DataOperationResult<bool>.Failure(ErrorSource.Unknown, errorArgs.Message, ex);
            }
        }

        /// <summary>
        /// Deletes an entity
        /// </summary>
        public virtual async Task<DataOperationResult<bool>> DeleteAsync(T entity)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentNullException(nameof(entity));
                }

                // Get the ID of the entity
                var id = entity.Id;
                if (id <= 0)
                {
                    throw new InvalidOperationException("Cannot delete weather data with invalid ID");
                }

                return await DeleteAsync(id);
            }
            catch (Exception ex)
            {
                var errorArgs = CreateErrorEventArgs(ErrorSource.Unknown,
                    $"Error deleting weather data: {ex.Message}", ex);
                OnErrorOccurred(errorArgs);
                return DataOperationResult<bool>.Failure(ErrorSource.Unknown, errorArgs.Message, ex);
            }
        }

        /// <summary>
        /// Gets weather for a specific location
        /// </summary>
        public async Task<OperationResult<T>> GetWeatherForLocationAsync(int locationId)
        {
            try
            {
                // Use the repository's GetByLocationIdAsync method
                var result = await _repository.GetByLocationIdAsync(locationId);

                if (result.IsSuccess && result.Data != null)
                {
                    // Convert to view model
                    var viewModel = new T();
                    // Create a WeatherDTO from the repository data
                    var dto = new WeatherDTO
                    {
                        Id = result.Data.Id,
                        LocationId = result.Data.LocationId,
                        Timestamp = result.Data.Timestamp,
                        Temperature = result.Data.Temperature,
                        Description = result.Data.Description,
                        WindSpeed = result.Data.WindSpeed,
                        WindDirection = result.Data.WindDirection,
                        Humidity = result.Data.Humidity,
                        Pressure = result.Data.Pressure,
                        Visibility = result.Data.Visibility,
                        Precipitation = result.Data.Precipitation,
                        SunriseTime = result.Data.SunriseTime,
                        SunsetTime = result.Data.SunsetTime,
                        CloudCover = result.Data.CloudCover,
                        FeelsLike = result.Data.FeelsLike,
                        UVIndex = result.Data.UVIndex,
                        AirQuality = result.Data.AirQuality,
                        LastUpdate = result.Data.LastUpdate,
                        Forecast = result.Data.Forecast,
                        IsDeleted = result.Data.IsDeleted
                    };
                    viewModel.InitializeFromDTO(dto);
                    return OperationResult<T>.Success(viewModel);
                }
                else
                {
                    // If we couldn't find it in the database, we might need to fetch it from an external service
                    // For this example, we'll just return a failure
                    return OperationResult<T>.Failure(
                        $"No weather data found for location ID {locationId}",
                        null,
                        OperationErrorSource.Unknown);
                }
            }
            catch (Exception ex)
            {
                var errorArgs = CreateErrorEventArgs(ErrorSource.Unknown,
                    $"Error retrieving weather for location ID {locationId}: {ex.Message}", ex);
                OnErrorOccurred(errorArgs);
                return OperationResult<T>.Failure(
                    $"Error retrieving weather: {ex.Message}",
                    ex,
                    OperationErrorSource.Unknown);
            }
        }

        /// <summary>
        /// Gets weather by geographic coordinates
        /// </summary>
        public async Task<OperationResult<T>> GetWeatherByCoordinatesAsync(double latitude, double longitude)
        {
            try
            {
                // First, try to find a location with these coordinates
                LocationViewModel location = null;
                if (_locationService != null)
                {
                    location = _locationService.GetLocationByCoordinates(latitude, longitude);
                }

                if (location != null && location.Id > 0)
                {
                    // If we found a location, get its weather
                    return await GetWeatherForLocationAsync(location.Id);
                }
                else
                {
                    // If no location found, try to get weather directly by coordinates
                    var result = await _repository.GetByCoordinatesAsync(latitude, longitude);

                    if (result.IsSuccess && result.Data != null)
                    {
                        // Convert to view model
                        var viewModel = new T();
                        // Create a WeatherDTO from the repository data
                        var dto = new WeatherDTO
                        {
                            Id = result.Data.Id,
                            LocationId = result.Data.LocationId,
                            Timestamp = result.Data.Timestamp,
                            Temperature = result.Data.Temperature,
                            Description = result.Data.Description,
                            WindSpeed = result.Data.WindSpeed,
                            WindDirection = result.Data.WindDirection,
                            Humidity = result.Data.Humidity,
                            Pressure = result.Data.Pressure,
                            Visibility = result.Data.Visibility,
                            Precipitation = result.Data.Precipitation,
                            SunriseTime = result.Data.SunriseTime,
                            SunsetTime = result.Data.SunsetTime,
                            CloudCover = result.Data.CloudCover,
                            FeelsLike = result.Data.FeelsLike,
                            UVIndex = result.Data.UVIndex,
                            AirQuality = result.Data.AirQuality,
                            LastUpdate = result.Data.LastUpdate,
                            Forecast = result.Data.Forecast,
                            IsDeleted = result.Data.IsDeleted
                        };
                        viewModel.InitializeFromDTO(dto);
                        return OperationResult<T>.Success(viewModel);
                    }
                    else
                    {
                        // If we couldn't find it in the database, we might need to fetch it from an external service
                        // For this example, we'll just return a failure
                        return OperationResult<T>.Failure(
                            $"No weather data found for coordinates ({latitude}, {longitude})",
                            null,
                            OperationErrorSource.Unknown);
                    }
                }
            }
            catch (Exception ex)
            {
                var errorArgs = CreateErrorEventArgs(ErrorSource.Unknown,
                    $"Error retrieving weather for coordinates ({latitude}, {longitude}): {ex.Message}", ex);
                OnErrorOccurred(errorArgs);
                return OperationResult<T>.Failure(
                    $"Error retrieving weather: {ex.Message}",
                    ex,
                    OperationErrorSource.Unknown);
            }
        }

        /// <summary>
        /// Gets forecast for a location for a specified number of days
        /// </summary>
        public async Task<OperationResult<string>> GetForecastForLocationAsync(int locationId, int days = 5)
        {
            try
            {
                // In a real implementation, this would fetch forecast data from a weather API
                // For this example, we'll simulate it

                // First, get the current weather to ensure we have the location
                var weatherResult = await GetWeatherForLocationAsync(locationId);

                if (weatherResult.IsSuccess && weatherResult.Data != null)
                {
                    // Create a simple forecast string
                    var weatherData = weatherResult.Data;
                    var forecast = $"Forecast for next {days} days starting from {DateTime.Now:yyyy-MM-dd}";

                    // In a real app, this would be structured data, not just a string
                    return OperationResult<string>.Success(forecast);
                }
                else
                {
                    return OperationResult<string>.Failure(
                        $"Could not retrieve current weather for location ID {locationId}",
                        null,
                        OperationErrorSource.Unknown);
                }
            }
            catch (Exception ex)
            {
                var errorArgs = CreateErrorEventArgs(ErrorSource.Unknown,
                    $"Error retrieving forecast for location ID {locationId}: {ex.Message}", ex);
                OnErrorOccurred(errorArgs);
                return OperationResult<string>.Failure(
                    $"Error retrieving forecast: {ex.Message}",
                    ex,
                    OperationErrorSource.Unknown);
            }
        }
    }
}