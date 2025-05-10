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
    /// Repository implementation for tip type operations
    /// </summary>
    public class TipTypeRepository : RepositoryBase<TipTypeViewModel>, ITipTypeRepository
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
        /// Creates a new tip type repository
        /// </summary>
       public TipTypeRepository(IAlertService alertService, ILoggerService loggerService)
            : base()
        {
            AlertService = alertService ?? throw new ArgumentNullException(nameof(alertService));
            LoggerService = loggerService ?? throw new ArgumentNullException(nameof(loggerService));

            // Initialize database connection
            dataB = DataEncrypted.GetAsyncConnection();
        }

        /// <summary>
        /// Gets a tip type by its ID
        /// </summary>
        public override async Task<DataOperationResult<TipTypeViewModel>> GetByIdAsync(int id)
        {
            try
            {
                var result = await dataB.Table<TipTypeViewModel>()
                    .Where(x => x.Id == id)
                    .FirstOrDefaultAsync();

                if (result == null)
                {
                    return DataOperationResult<TipTypeViewModel>.Failure(
                        ErrorSource.Database,
                        $"Tip type with ID {id} not found");
                }

                return DataOperationResult<TipTypeViewModel>.Success(result);
            }
            catch (SQLiteException ex)
            {
                string message = $"Database error retrieving tip type with ID {id}: {ex.Message}";
               LoggerService.LogError(message, ex);
                OnErrorOccurred(new DataErrorEventArgs(
                    ErrorSource.Database,
                    message,
                    ex));
                return DataOperationResult<TipTypeViewModel>.Failure(ErrorSource.Database, message, ex);
            }
            catch (Exception ex)
            {
                string message = $"Error retrieving tip type with ID {id}: {ex.Message}";
               LoggerService.LogError(message, ex);
                OnErrorOccurred(new DataErrorEventArgs(
                    ErrorSource.Unknown,
                    message,
                    ex));
                return DataOperationResult<TipTypeViewModel>.Failure(ErrorSource.Unknown, message, ex);
            }
        }

        /// <summary>
        /// Gets all tip types
        /// </summary>
        public override async Task<DataOperationResult<IList<TipTypeViewModel>>> GetAllAsync()
        {
            try
            {
                var results = await dataB.Table<TipTypeViewModel>().ToListAsync();
                return DataOperationResult<IList<TipTypeViewModel>>.Success(results);
            }
            catch (SQLiteException ex)
            {
                string message = $"Database error retrieving all tip types: {ex.Message}";
               LoggerService.LogError(message, ex);
                OnErrorOccurred(new DataErrorEventArgs(
                    ErrorSource.Database,
                    message,
                    ex));
                return DataOperationResult<IList<TipTypeViewModel>>.Failure(ErrorSource.Database, message, ex);
            }
            catch (Exception ex)
            {
                string message = $"Error retrieving all tip types: {ex.Message}";
                LoggerService.LogError(message, ex);
                OnErrorOccurred(new DataErrorEventArgs(
                    ErrorSource.Unknown,
                    message,
                    ex));
                return DataOperationResult<IList<TipTypeViewModel>>.Failure(ErrorSource.Unknown, message, ex);
            }
        }

        /// <summary>
        /// Saves a tip type
        /// </summary>
        public override async Task<DataOperationResult<TipTypeViewModel>> SaveAsync(TipTypeViewModel entity)
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

                return DataOperationResult<TipTypeViewModel>.Success(entity);
            }
            catch (SQLiteException ex)
            {
                string message = $"Database error saving tip type: {ex.Message}";
                LoggerService.LogError(message, ex);
                OnErrorOccurred(new DataErrorEventArgs(
                    ErrorSource.Database,
                    message,
                    ex));
                return DataOperationResult<TipTypeViewModel>.Failure(ErrorSource.Database, message, ex);
            }
            catch (Exception ex)
            {
                string message = $"Error saving tip type: {ex.Message}";
                LoggerService.LogError(message, ex);
                OnErrorOccurred(new DataErrorEventArgs(
                    ErrorSource.Unknown,
                    message,
                    ex));
                return DataOperationResult<TipTypeViewModel>.Failure(ErrorSource.Unknown, message, ex);
            }
        }

        /// <summary>
        /// Updates an existing tip type
        /// </summary>
        
        public override async Task<DataOperationResult<bool>> UpdateAsync(TipTypeViewModel entity)
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
                        "Cannot update tip type with invalid ID");
                }

                int rowsAffected = await dataB.UpdateAsync(entity);
                bool success = rowsAffected > 0;

                return DataOperationResult<bool>.Success(success);
            }
            catch (SQLiteException ex)
            {
                string message = $"Database error updating tip type: {ex.Message}";
                LoggerService.LogError(message, ex);
                OnErrorOccurred(new DataErrorEventArgs(
                    ErrorSource.Database,
                    message,
                    ex));
                return DataOperationResult<bool>.Failure(ErrorSource.Database, message, ex);
            }
            catch (Exception ex)
            {
                string message = $"Error updating tip type: {ex.Message}";
                LoggerService.LogError(message, ex);
                OnErrorOccurred(new DataErrorEventArgs(
                    ErrorSource.Unknown,
                    message,
                    ex));
                return DataOperationResult<bool>.Failure(ErrorSource.Unknown, message, ex);
            }
        }

        /// <summary>
        /// Deletes a tip type by ID
        /// </summary>
        public override async Task<DataOperationResult<bool>> DeleteAsync(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return DataOperationResult<bool>.Failure(
                        ErrorSource.ModelValidation,
                        "Cannot delete tip type with invalid ID");
                }

                int rowsAffected = await dataB.DeleteAsync<TipTypeViewModel>(id);
                bool success = rowsAffected > 0;

                return DataOperationResult<bool>.Success(success);
            }
            catch (SQLiteException ex)
            {
               string message = $"Database error deleting tip type with ID {id}: {ex.Message}";
                LoggerService.LogError(message, ex);
                OnErrorOccurred(new DataErrorEventArgs(
                    ErrorSource.Database,
                    message,
                    ex));
                return DataOperationResult<bool>.Failure(ErrorSource.Database, message, ex);
            }
            catch (Exception ex)
            {
                string message = $"Error deleting tip type with ID {id}: {ex.Message}";
                LoggerService.LogError(message, ex);
                OnErrorOccurred(new DataErrorEventArgs(
                    ErrorSource.Unknown,
                    message,
                    ex));
                return DataOperationResult<bool>.Failure(ErrorSource.Unknown, message, ex);
            }
        }

        /// <summary>
        /// Deletes a tip type entity
        /// </summary>
        public override async Task<DataOperationResult<bool>> DeleteAsync(TipTypeViewModel entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            return await DeleteAsync(entity.Id);
        }
    }
}