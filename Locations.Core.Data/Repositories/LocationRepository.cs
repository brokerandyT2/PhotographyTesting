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
using Location.Core.Helpers.LoggingService;

namespace Locations.Core.Data.Queries
{
    /// <summary>
    /// Repository implementation for location operations
    /// </summary>
    public class LocationRepository : RepositoryBase<LocationViewModel>, ILocationRepository
    {
        /// <summary>
        /// Database connection
        /// </summary>
        private readonly SQLiteAsyncConnection dataB;

        /// <summary>
        /// Logger service
        /// </summary>
        protected readonly ILoggerService LoggerService;

        /// <summary>
        /// Alert service
        /// </summary>
        protected readonly IAlertService AlertService;

        /// <summary>
        /// Creates a new location repository
        /// </summary>
        public LocationRepository(IAlertService alertService, ILoggerService loggerService)
            : base()
        {
            AlertService = alertService ?? throw new ArgumentNullException(nameof(alertService));
            LoggerService = loggerService ?? throw new ArgumentNullException(nameof(loggerService));

            // Initialize database connection - assuming this is set up somewhere
            dataB = DataEncrypted.GetAsyncConnection();
        }

        /// <summary>
        /// Gets a location by its ID
        /// </summary>
        public override async Task<DataOperationResult<LocationViewModel>> GetByIdAsync(int id)
        {
            try
            {
                var result = await dataB.Table<LocationViewModel>()
                    .Where(x => x.Id == id)
                    .FirstOrDefaultAsync();

                if (result == null)
                {
                    return DataOperationResult<LocationViewModel>.Failure(
                        ErrorSource.Database,
                        $"Location with ID {id} not found");
                }

                return DataOperationResult<LocationViewModel>.Success(result);
            }
            catch (SQLiteException ex)
            {
                string message = $"Database error retrieving location with ID {id}: {ex.Message}";
                LoggerService.LogError(message, ex);
                OnErrorOccurred(new DataErrorEventArgs(
                    ErrorSource.Database,
                    message,
                    ex));
                return DataOperationResult<LocationViewModel>.Failure(ErrorSource.Database, message, ex);
            }
            catch (Exception ex)
            {
                string message = $"Error retrieving location with ID {id}: {ex.Message}";
               LoggerService.LogError(message, ex);
                OnErrorOccurred(new DataErrorEventArgs(
                    ErrorSource.Unknown,
                    message,
                    ex));
                return DataOperationResult<LocationViewModel>.Failure(ErrorSource.Unknown, message, ex);
            }
        }

        /// <summary>
        /// Gets all locations
        /// </summary>
        public override async Task<DataOperationResult<IList<LocationViewModel>>> GetAllAsync()
        {
            try
            {
                var results = await dataB.Table<LocationViewModel>().ToListAsync();
                return DataOperationResult<IList<LocationViewModel>>.Success(results);
            }
            catch (SQLiteException ex)
            {
                string message = $"Database error retrieving all locations: {ex.Message}";
                LoggerService.LogError(message, ex);
                OnErrorOccurred(new DataErrorEventArgs(
                    ErrorSource.Database,
                    message,
                    ex));
                return DataOperationResult<IList<LocationViewModel>>.Failure(ErrorSource.Database, message, ex);
            }
            catch (Exception ex)
            {
                string message = $"Error retrieving all locations: {ex.Message}";
                LoggerService.LogError(message, ex);
                OnErrorOccurred(new DataErrorEventArgs(
                    ErrorSource.Unknown,
                    message,
                    ex));
                return DataOperationResult<IList<LocationViewModel>>.Failure(ErrorSource.Unknown, message, ex);
            }
        }

        /// <summary>
        /// Saves a location
        /// </summary>
        public override async Task<DataOperationResult<LocationViewModel>> SaveAsync(LocationViewModel entity)
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
                    // For existing entities (this should be in UpdateAsync, but including for safety)
                    await dataB.UpdateAsync(entity);
                }

                return DataOperationResult<LocationViewModel>.Success(entity);
            }
            catch (SQLiteException ex)
            {
                string message = $"Database error saving location: {ex.Message}";
                LoggerService.LogError(message, ex);
                OnErrorOccurred(new DataErrorEventArgs(
                    ErrorSource.Database,
                    message,
                    ex));
                return DataOperationResult<LocationViewModel>.Failure(ErrorSource.Database, message, ex);
            }
            catch (Exception ex)
            {
                string message = $"Error saving location: {ex.Message}";
                LoggerService.LogError(message, ex);
                OnErrorOccurred(new DataErrorEventArgs(
                    ErrorSource.Unknown,
                    message,
                    ex));
                return DataOperationResult<LocationViewModel>.Failure(ErrorSource.Unknown, message, ex);
            }
        }

        /// <summary>
        /// Updates an existing location
        /// </summary>
        public override async Task<DataOperationResult<bool>> UpdateAsync(LocationViewModel entity)
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
                        "Cannot update location with invalid ID");
                }

                int rowsAffected = await dataB.UpdateAsync(entity);
                bool success = rowsAffected > 0;

                return DataOperationResult<bool>.Success(success);
            }
            catch (SQLiteException ex)
            {
                string message = $"Database error updating location: {ex.Message}";
                LoggerService.LogError(message, ex);
                OnErrorOccurred(new DataErrorEventArgs(
                    ErrorSource.Database,
                    message,
                    ex));
                return DataOperationResult<bool>.Failure(ErrorSource.Database, message, ex);
            }
            catch (Exception ex)
            {
                string message = $"Error updating location: {ex.Message}";
                LoggerService.LogError(message, ex);
                OnErrorOccurred(new DataErrorEventArgs(
                    ErrorSource.Unknown,
                    message,
                    ex));
                return DataOperationResult<bool>.Failure(ErrorSource.Unknown, message, ex);
            }
        }

        /// <summary>
        /// Deletes a location by ID
        /// </summary>
        public override async Task<DataOperationResult<bool>> DeleteAsync(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return DataOperationResult<bool>.Failure(
                        ErrorSource.ModelValidation,
                        "Cannot delete location with invalid ID");
                }

                int rowsAffected = await dataB.DeleteAsync<LocationViewModel>(id);
                bool success = rowsAffected > 0;

                return DataOperationResult<bool>.Success(success);
            }
            catch (SQLiteException ex)
            {
                string message = $"Database error deleting location with ID {id}: {ex.Message}";
                LoggerService.LogError(message, ex);
                OnErrorOccurred(new DataErrorEventArgs(
                    ErrorSource.Database,
                    message,
                    ex));
                return DataOperationResult<bool>.Failure(ErrorSource.Database, message, ex);
            }
            catch (Exception ex)
            {
                string message = $"Error deleting location with ID {id}: {ex.Message}";
                LoggerService.LogError(message, ex);
                OnErrorOccurred(new DataErrorEventArgs(
                    ErrorSource.Unknown,
                    message,
                    ex));
                return DataOperationResult<bool>.Failure(ErrorSource.Unknown, message, ex);
            }
        }

        /// <summary>
        /// Deletes a location entity
        /// </summary>
        public override async Task<DataOperationResult<bool>> DeleteAsync(LocationViewModel entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            return await DeleteAsync(entity.Id);
        }

        /// <summary>
        /// Gets a location by its geographic coordinates
        /// </summary>
        public async Task<DataOperationResult<LocationViewModel>> GetByCoordinatesAsync(double latitude, double longitude)
        {
            try
            {
                var result = await dataB.Table<LocationViewModel>()
                    .Where(x => x.Lattitude == latitude && x.Longitude == longitude)
                    .FirstOrDefaultAsync();

                if (result == null)
                {
                    return DataOperationResult<LocationViewModel>.Failure(
                        ErrorSource.Database,
                        $"Location at coordinates ({latitude}, {longitude}) not found");
                }

                return DataOperationResult<LocationViewModel>.Success(result);
            }
            catch (SQLiteException ex)
            {
                string message = $"Database error retrieving location at coordinates ({latitude}, {longitude}): {ex.Message}";
                LoggerService.LogError(message, ex);
                OnErrorOccurred(new DataErrorEventArgs(
                    ErrorSource.Database,
                    message,
                    ex));
                return DataOperationResult<LocationViewModel>.Failure(ErrorSource.Database, message, ex);
            }
            catch (Exception ex)
            {
                string message = $"Error retrieving location at coordinates ({latitude}, {longitude}): {ex.Message}";
                LoggerService.LogError(message, ex);
                OnErrorOccurred(new DataErrorEventArgs(
                    ErrorSource.Unknown,
                    message,
                    ex));
                return DataOperationResult<LocationViewModel>.Failure(ErrorSource.Unknown, message, ex);
            }
        }
    }
}