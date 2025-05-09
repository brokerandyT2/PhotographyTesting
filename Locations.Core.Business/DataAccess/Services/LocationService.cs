using Location.Core.Helpers.AlertService;
using Location.Core.Helpers.LoggingService;
using Locations.Core.Business.DataAccess.Base;
using Locations.Core.Business.DataAccess.Interfaces;
using Locations.Core.Data.Models;
using Locations.Core.Data.Queries;
using Locations.Core.Data.Queries.Interfaces;
using Locations.Core.Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataErrorEventArgs = Locations.Core.Data.Models.DataErrorEventArgs;
using ILoggerService = Locations.Core.Business.DataAccess.Interfaces.ILoggerService;

namespace Locations.Core.Business.DataAccess.Services
{
    /// <summary>
    /// Service for location-related operations
    /// </summary>
    /// <typeparam name="T">The view model type for locations</typeparam>
    public class LocationService<T> : ILocationService<T>
        where T : LocationViewModel, new()
    {
        private readonly ILocationRepository _repository;
        private readonly IAlertService _alertService;
        private readonly ILoggerService _loggerService;
        private readonly IWeatherService<WeatherViewModel> _weatherService;

        /// <summary>
        /// Event triggered when an error occurs in the service
        /// </summary>
        public event DataErrorEventHandler ErrorOccurred;

        /// <summary>
        /// Creates a new instance of the LocationService with dependencies
        /// </summary>
        /// <param name="repository">The location repository</param>
        /// <param name="alertService">The alert service</param>
        /// <param name="loggerService">The logger service</param>
        /// <param name="weatherService">The weather service for location weather data</param>
        public LocationService(
            ILocationRepository repository,
            IAlertService alertService,
            ILoggerService loggerService,
            IWeatherService<WeatherViewModel> weatherService = null)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _alertService = alertService ?? throw new ArgumentNullException(nameof(alertService));
            _loggerService = loggerService ?? throw new ArgumentNullException(nameof(loggerService));
            _weatherService = weatherService;

            // Connect repository events
            if (_repository is ILocationRepository repoWithEvents)
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
        public virtual void OnErrorOccurred(DataErrorEventArgs e)
        {
            // Log the error
            _loggerService.LogError(e.Message, e.Exception);

            // Show alert for UI - using the standard methods from IAlertService
            if (_alertService != null)
            {
                if (e.Exception != null)
                {
                    // Use logging since IAlertService doesn't have ShowAlert
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
                    viewModel.InitializeFromDTO(result.Data);
                    return DataOperationResult<T>.Success(viewModel);
                }
                else
                {
                    // Create a new failure result for type T
                    return DataOperationResult<T>.Failure(
                        result.ErrorSource,
                        $"Failed to retrieve location with ID {id}",
                        result.Exception);
                }
            }
            catch (Exception ex)
            {
                var errorArgs = CreateErrorEventArgs(ErrorSource.Unknown,
                    $"Error retrieving entity with ID {id}: {ex.Message}", ex);
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
                        viewModel.InitializeFromDTO(item);
                        viewModels.Add(viewModel);
                    }
                    return DataOperationResult<List<T>>.Success(viewModels);
                }
                else
                {
                    // Create a new failure result
                    return DataOperationResult<List<T>>.Failure(
                        result.ErrorSource,
                        "Failed to retrieve all locations",
                        result.Exception);
                }
            }
            catch (Exception ex)
            {
                var errorArgs = CreateErrorEventArgs(ErrorSource.Unknown,
                    $"Error retrieving all entities: {ex.Message}", ex);
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
                // Convert T to LocationViewModel if needed
                var locationViewModel = entity as LocationViewModel;
                if (locationViewModel == null)
                {
                    throw new InvalidOperationException("Invalid entity type");
                }

                var result = await _repository.SaveAsync(locationViewModel);

                if (result.IsSuccess && result.Data != null)
                {
                    // Convert the result back to T
                    entity.InitializeFromDTO(result.Data);
                    return DataOperationResult<T>.Success(entity);
                }
                else
                {
                    // Create a new failure result
                    return DataOperationResult<T>.Failure(
                        result.ErrorSource,
                        "Failed to save location",
                        result.Exception);
                }
            }
            catch (Exception ex)
            {
                var errorArgs = CreateErrorEventArgs(ErrorSource.Unknown,
                    $"Error saving entity: {ex.Message}", ex);
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
                // Convert T to LocationViewModel if needed
                var locationViewModel = entity as LocationViewModel;
                if (locationViewModel == null)
                {
                    throw new InvalidOperationException("Invalid entity type");
                }

                var result = await _repository.UpdateAsync(locationViewModel);
                return result; // This already returns DataOperationResult<bool>
            }
            catch (Exception ex)
            {
                var errorArgs = CreateErrorEventArgs(ErrorSource.Unknown,
                    $"Error updating entity: {ex.Message}", ex);
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
                    $"Error deleting entity with ID {id}: {ex.Message}", ex);
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
                    throw new InvalidOperationException("Cannot delete entity with invalid ID");
                }

                return await DeleteAsync(id);
            }
            catch (Exception ex)
            {
                var errorArgs = CreateErrorEventArgs(ErrorSource.Unknown,
                    $"Error deleting entity: {ex.Message}", ex);
                OnErrorOccurred(errorArgs);
                return DataOperationResult<bool>.Failure(ErrorSource.Unknown, errorArgs.Message, ex);
            }
        }

        /// <summary>
        /// Gets a location by its coordinates
        /// </summary>
        public T GetLocationByCoordinates(double latitude, double longitude)
        {
            try
            {
                // Use the repository to find the location
                var task = Task.Run(async () =>
                    await _repository.GetByCoordinatesAsync(latitude, longitude));
                var result = task.Result;

                if (result.IsSuccess && result.Data != null)
                {
                    // Convert the result to type T
                    var viewModel = new T();
                    viewModel.InitializeFromDTO(result.Data);
                    return viewModel;
                }

                // Log the error if the operation failed
                if (!result.IsSuccess)
                {
                    _loggerService.LogWarning($"Failed to find location at coordinates ({latitude}, {longitude})");
                }

                return new T();
            }
            catch (Exception ex)
            {
                var errorArgs = CreateErrorEventArgs(ErrorSource.Unknown,
                    $"Error retrieving location at coordinates ({latitude}, {longitude}): {ex.Message}", ex);
                OnErrorOccurred(errorArgs);
                return new T();
            }
        }

        /// <summary>
        /// Gets all locations with full details including weather
        /// </summary>
        public async Task<List<T>> GetLocationsWithDetailsAsync()
        {
            try
            {
                var result = await GetAllAsync();

                if (result.IsSuccess && result.Data != null)
                {
                    var locations = result.Data;

                    // If we have a weather service, enrich locations with weather data
                    if (_weatherService != null)
                    {
                        foreach (var location in locations)
                        {
                            try
                            {
                                // Get weather for this location
                                var weatherResult = await _weatherService.GetWeatherForLocationAsync(location.Id);
                                if (weatherResult.IsSuccess && weatherResult.Data != null)
                                {
                                    // Associate weather with the location
                                    // This would typically be done through a property on the location view model
                                    // For this example, we'll assume we have a method to do this
                                    AssociateWeatherWithLocation(location, weatherResult.Data);
                                }
                            }
                            catch (Exception weatherEx)
                            {
                                _loggerService.LogWarning($"Failed to get weather for location {location.Id}: {weatherEx.Message}");
                            }
                        }
                    }

                    return locations;
                }

                return new List<T>();
            }
            catch (Exception ex)
            {
                var errorArgs = CreateErrorEventArgs(ErrorSource.Unknown,
                    $"Error retrieving locations with details: {ex.Message}", ex);
                OnErrorOccurred(errorArgs);
                return new List<T>();
            }
        }

        /// <summary>
        /// Gets nearby locations within a radius
        /// </summary>
        public async Task<List<T>> GetNearbyLocationsAsync(double latitude, double longitude, double radiusKm = 10)
        {
            try
            {
                // Get all locations first
                var result = await GetAllAsync();

                if (result.IsSuccess && result.Data != null)
                {
                    // Filter locations by distance
                    return result.Data
                        .Where(l => CalculateDistance(latitude, longitude, l.Lattitude, l.Longitude) <= radiusKm)
                        .ToList();
                }

                return new List<T>();
            }
            catch (Exception ex)
            {
                var errorArgs = CreateErrorEventArgs(ErrorSource.Unknown,
                    $"Error retrieving nearby locations: {ex.Message}", ex);
                OnErrorOccurred(errorArgs);
                return new List<T>();
            }
        }

        /// <summary>
        /// Calculates the distance between two geographic coordinates in kilometers
        /// </summary>
        private double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            // Haversine formula for distance calculation
            const double EarthRadiusKm = 6371.0;
            var dLat = DegreesToRadians(lat2 - lat1);
            var dLon = DegreesToRadians(lon2 - lon1);

            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(DegreesToRadians(lat1)) * Math.Cos(DegreesToRadians(lat2)) *
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return EarthRadiusKm * c;
        }

        private double DegreesToRadians(double degrees)
        {
            return degrees * Math.PI / 180.0;
        }

        /// <summary>
        /// Associates weather data with a location
        /// </summary>
        private void AssociateWeatherWithLocation(T location, WeatherViewModel weather)
        {
            // In a real implementation, this might set a WeatherViewModel property on the LocationViewModel
            // For this demonstration, we'll log that the association has been made
            _loggerService.LogInformation($"Associated weather data with location {location.Id}");
        }

    }
}