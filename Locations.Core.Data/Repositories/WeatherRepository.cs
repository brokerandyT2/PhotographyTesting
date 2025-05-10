using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;
using Location.Core.Helpers.AlertService;
//using Location.Core.Helpers.LoggingService;
using Locations.Core.Data.Models;
using Locations.Core.Data.Queries.Interfaces;
using Locations.Core.Data.Queries.Base;
using Locations.Core.Shared.ViewModels;
using DataErrorEventArgs = Locations.Core.Data.Models.DataErrorEventArgs;
using ErrorSource = Locations.Core.Data.Models.ErrorSource;
using EncryptedSQLite;

namespace Locations.Core.Data.Queries
{
    /// <summary>
    /// Repository implementation for weather operations
    /// </summary>
    public class WeatherRepository : RepositoryBase<WeatherViewModel>, IWeatherRepository
    {
        /// <summary>
        /// Database connection
        /// </summary>
        private readonly SQLiteAsyncConnection dataB;

        /// <summary>
        /// Logger service
        /// </summary>
       // protected readonly ILoggerService LoggerService;

        /// <summary>
        /// Alert service
        /// </summary>
        protected readonly IAlertService AlertService;

        /// <summary>
        /// Creates a new weather repository
        /// </summary>
     /*   public WeatherRepository(IAlertService alertService, ILoggerService loggerService)
            : base()
        {
            AlertService = alertService ?? throw new ArgumentNullException(nameof(alertService));
            LoggerService = loggerService ?? throw new ArgumentNullException(nameof(loggerService));

            // Initialize database connection
            dataB = DataEncrypted.GetAsyncConnection();
        }*/

        /// <summary>
        /// Gets a weather entry by its ID
        /// </summary>
        public override async Task<DataOperationResult<WeatherViewModel>> GetByIdAsync(int id)
        {
            try
            {
                var result = await dataB.Table<WeatherViewModel>()
                    .Where(x => x.Id == id)
                    .FirstOrDefaultAsync();

                if (result == null)
                {
                    return DataOperationResult<WeatherViewModel>.Failure(
                        ErrorSource.Database,
                        $"Weather entry with ID {id} not found");
                }

                return DataOperationResult<WeatherViewModel>.Success(result);
            }
            catch (SQLiteException ex)
            {
                string message = $"Database error retrieving weather entry with ID {id}: {ex.Message}";
             //   LoggerService.LogError(message, ex);
                OnErrorOccurred(new DataErrorEventArgs(
                    ErrorSource.Database,
                    message,
                    ex));
                return DataOperationResult<WeatherViewModel>.Failure(ErrorSource.Database, message, ex);
            }
            catch (Exception ex)
            {
                string message = $"Error retrieving weather entry with ID {id}: {ex.Message}";
            //    LoggerService.LogError(message, ex);
                OnErrorOccurred(new DataErrorEventArgs(
                    ErrorSource.Unknown,
                    message,
                    ex));
                return DataOperationResult<WeatherViewModel>.Failure(ErrorSource.Unknown, message, ex);
            }
        }

        /// <summary>
        /// Gets all weather entries
        /// </summary>
        public override async Task<DataOperationResult<IList<WeatherViewModel>>> GetAllAsync()
        {
            try
            {
                var results = await dataB.Table<WeatherViewModel>().ToListAsync();
                return DataOperationResult<IList<WeatherViewModel>>.Success(results);
            }
            catch (SQLiteException ex)
            {
                string message = $"Database error retrieving all weather entries: {ex.Message}";
            //    LoggerService.LogError(message, ex);
                OnErrorOccurred(new DataErrorEventArgs(
                    ErrorSource.Database,
                    message,
                    ex));
                return DataOperationResult<IList<WeatherViewModel>>.Failure(ErrorSource.Database, message, ex);
            }
            catch (Exception ex)
            {
                string message = $"Error retrieving all weather entries: {ex.Message}";
            //    LoggerService.LogError(message, ex);
                OnErrorOccurred(new DataErrorEventArgs(
                    ErrorSource.Unknown,
                    message,
                    ex));
                return DataOperationResult<IList<WeatherViewModel>>.Failure(ErrorSource.Unknown, message, ex);
            }
        }

        /// <summary>
        /// Saves a weather entry
        /// </summary>
        public override async Task<DataOperationResult<WeatherViewModel>> SaveAsync(WeatherViewModel entity)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentNullException(nameof(entity));
                }

                // For new entities
                if (entity.Id <= 0)
                {
                    await dataB.InsertAsync(entity);
                }
                else
                {
                    // For existing entities
                    await dataB.UpdateAsync(entity);
                }

                return DataOperationResult<WeatherViewModel>.Success(entity);
            }
            catch (SQLiteException ex)
            {
                string message = $"Database error saving weather entry: {ex.Message}";
            //    LoggerService.LogError(message, ex);
                OnErrorOccurred(new DataErrorEventArgs(
                    ErrorSource.Database,
                    message,
                    ex));
                return DataOperationResult<WeatherViewModel>.Failure(ErrorSource.Database, message, ex);
            }
            catch (Exception ex)
            {
                string message = $"Error saving weather entry: {ex.Message}";
            //    LoggerService.LogError(message, ex);
                OnErrorOccurred(new DataErrorEventArgs(
                    ErrorSource.Unknown,
                    message,
                    ex));
                return DataOperationResult<WeatherViewModel>.Failure(ErrorSource.Unknown, message, ex);
            }
        }

        /// <summary>
        /// Updates an existing weather entry
        /// </summary>
        public override async Task<DataOperationResult<bool>> UpdateAsync(WeatherViewModel entity)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentNullException(nameof(entity));
                }

                if (entity.Id <= 0)
                {
                    return DataOperationResult<bool>.Failure(
                        ErrorSource.ModelValidation,
                        "Cannot update weather entry with invalid ID");
                }

                int rowsAffected = await dataB.UpdateAsync(entity);
                bool success = rowsAffected > 0;

                return DataOperationResult<bool>.Success(success);
            }
            catch (SQLiteException ex)
            {
                string message = $"Database error updating weather entry: {ex.Message}";
             //   LoggerService.LogError(message, ex);
                OnErrorOccurred(new DataErrorEventArgs(
                    ErrorSource.Database,
                    message,
                    ex));
                return DataOperationResult<bool>.Failure(ErrorSource.Database, message, ex);
            }
            catch (Exception ex)
            {
                string message = $"Error updating weather entry: {ex.Message}";
            //    LoggerService.LogError(message, ex);
                OnErrorOccurred(new DataErrorEventArgs(
                    ErrorSource.Unknown,
                    message,
                    ex));
                return DataOperationResult<bool>.Failure(ErrorSource.Unknown, message, ex);
            }
        }

        /// <summary>
        /// Deletes a weather entry by ID
        /// </summary>
        public override async Task<DataOperationResult<bool>> DeleteAsync(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return DataOperationResult<bool>.Failure(
                        ErrorSource.ModelValidation,
                        "Cannot delete weather entry with invalid ID");
                }

                int rowsAffected = await dataB.DeleteAsync<WeatherViewModel>(id);
                bool success = rowsAffected > 0;

                return DataOperationResult<bool>.Success(success);
            }
            catch (SQLiteException ex)
            {
                string message = $"Database error deleting weather entry with ID {id}: {ex.Message}";
            //    LoggerService.LogError(message, ex);
                OnErrorOccurred(new DataErrorEventArgs(
                    ErrorSource.Database,
                    message,
                    ex));
                return DataOperationResult<bool>.Failure(ErrorSource.Database, message, ex);
            }
            catch (Exception ex)
            {
                string message = $"Error deleting weather entry with ID {id}: {ex.Message}";
            //    LoggerService.LogError(message, ex);
                OnErrorOccurred(new DataErrorEventArgs(
                    ErrorSource.Unknown,
                    message,
                    ex));
                return DataOperationResult<bool>.Failure(ErrorSource.Unknown, message, ex);
            }
        }

        /// <summary>
        /// Deletes a weather entry entity
        /// </summary>
        public override async Task<DataOperationResult<bool>> DeleteAsync(WeatherViewModel entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            return await DeleteAsync(entity.Id);
        }

        /// <summary>
        /// Gets weather data by coordinates
        /// </summary>
        public async Task<DataOperationResult<WeatherViewModel>> GetByCoordinatesAsync(double latitude, double longitude)
        {
            try
            {
                var result = await dataB.Table<WeatherViewModel>()
                    .Where(x => x.Latitude == latitude && x.Longitude == longitude)
                    .FirstOrDefaultAsync();

                if (result == null)
                {
                    return DataOperationResult<WeatherViewModel>.Failure(
                        ErrorSource.Database,
                        $"Weather data for coordinates ({latitude}, {longitude}) not found");
                }

                return DataOperationResult<WeatherViewModel>.Success(result);
            }
            catch (SQLiteException ex)
            {
                string message = $"Database error retrieving weather for coordinates ({latitude}, {longitude}): {ex.Message}";
            //    LoggerService.LogError(message, ex);
                OnErrorOccurred(new DataErrorEventArgs(
                    ErrorSource.Database,
                    message,
                    ex));
                return DataOperationResult<WeatherViewModel>.Failure(ErrorSource.Database, message, ex);
            }
            catch (Exception ex)
            {
                string message = $"Error retrieving weather for coordinates ({latitude}, {longitude}): {ex.Message}";
            //    LoggerService.LogError(message, ex);
                OnErrorOccurred(new DataErrorEventArgs(
                    ErrorSource.Unknown,
                    message,
                    ex));
                return DataOperationResult<WeatherViewModel>.Failure(ErrorSource.Unknown, message, ex);
            }
        }

        /// <summary>
        /// Gets weather data by coordinates or returns a new instance if not found
        /// </summary>
        public async Task<DataOperationResult<WeatherViewModel>> GetByCoordinatesOrDefaultAsync(double latitude, double longitude)
        {
            try
            {
                var result = await GetByCoordinatesAsync(latitude, longitude);

                // If the weather data was found, return it
                if (result.IsSuccess)
                {
                    return result;
                }

                // Otherwise, return a new instance
                return DataOperationResult<WeatherViewModel>.Success(new WeatherViewModel
                {
                    Latitude = latitude,
                    Longitude = longitude
                });
            }
            catch (Exception ex)
            {
                string message = $"Error retrieving weather for coordinates ({latitude}, {longitude}): {ex.Message}";
           //     LoggerService.LogError(message, ex);
                OnErrorOccurred(new DataErrorEventArgs(
                    ErrorSource.Unknown,
                    message,
                    ex));
                return DataOperationResult<WeatherViewModel>.Failure(ErrorSource.Unknown, message, ex);
            }
        }

        public Task<DataOperationResult<WeatherViewModel>> GetByLocationIdAsync(int locationId)
        {
            return GetByIdAsync(locationId);
        }
    }
}