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
    /// Repository implementation for tip operations
    /// </summary>
    public class TipRepository : RepositoryBase<TipViewModel>, ITipRepository
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
        /// Creates a new tip repository
        /// </summary>
     /*   public TipRepository(IAlertService alertService, ILoggerService loggerService)
            : base()
        {
            AlertService = alertService ?? throw new ArgumentNullException(nameof(alertService));
            LoggerService = loggerService ?? throw new ArgumentNullException(nameof(loggerService));

            // Initialize database connection
            dataB = DataEncrypted.GetAsyncConnection();
        }*/

        /// <summary>
        /// Gets a tip by its ID
        /// </summary>
        public override async Task<DataOperationResult<TipViewModel>> GetByIdAsync(int id)
        {
            try
            {
                var result = await dataB.Table<TipViewModel>()
                    .Where(x => x.Id == id)
                    .FirstOrDefaultAsync();

                if (result == null)
                {
                    return DataOperationResult<TipViewModel>.Failure(
                        ErrorSource.Database,
                        $"Tip with ID {id} not found");
                }

                return DataOperationResult<TipViewModel>.Success(result);
            }
            catch (SQLiteException ex)
            {
                string message = $"Database error retrieving tip with ID {id}: {ex.Message}";
            //    LoggerService.LogError(message, ex);
                OnErrorOccurred(new DataErrorEventArgs(
                    ErrorSource.Database,
                    message,
                    ex));
                return DataOperationResult<TipViewModel>.Failure(ErrorSource.Database, message, ex);
            }
            catch (Exception ex)
            {
                string message = $"Error retrieving tip with ID {id}: {ex.Message}";
            //    LoggerService.LogError(message, ex);
                OnErrorOccurred(new DataErrorEventArgs(
                    ErrorSource.Unknown,
                    message,
                    ex));
                return DataOperationResult<TipViewModel>.Failure(ErrorSource.Unknown, message, ex);
            }
        }

        /// <summary>
        /// Gets all tips
        /// </summary>
        public override async Task<DataOperationResult<IList<TipViewModel>>> GetAllAsync()
        {
            try
            {
                var results = await dataB.Table<TipViewModel>().ToListAsync();
                return DataOperationResult<IList<TipViewModel>>.Success(results);
            }
            catch (SQLiteException ex)
            {
                string message = $"Database error retrieving all tips: {ex.Message}";
            //    LoggerService.LogError(message, ex);
                OnErrorOccurred(new DataErrorEventArgs(
                    ErrorSource.Database,
                    message,
                    ex));
                return DataOperationResult<IList<TipViewModel>>.Failure(ErrorSource.Database, message, ex);
            }
            catch (Exception ex)
            {
                string message = $"Error retrieving all tips: {ex.Message}";
            //    LoggerService.LogError(message, ex);
                OnErrorOccurred(new DataErrorEventArgs(
                    ErrorSource.Unknown,
                    message,
                    ex));
                return DataOperationResult<IList<TipViewModel>>.Failure(ErrorSource.Unknown, message, ex);
            }
        }

        /// <summary>
        /// Saves a tip
        /// </summary>
        public override async Task<DataOperationResult<TipViewModel>> SaveAsync(TipViewModel entity)
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

                return DataOperationResult<TipViewModel>.Success(entity);
            }
            catch (SQLiteException ex)
            {
                string message = $"Database error saving tip: {ex.Message}";
            //    LoggerService.LogError(message, ex);
                OnErrorOccurred(new DataErrorEventArgs(
                    ErrorSource.Database,
                    message,
                    ex));
                return DataOperationResult<TipViewModel>.Failure(ErrorSource.Database, message, ex);
            }
            catch (Exception ex)
            {
                string message = $"Error saving tip: {ex.Message}";
            //    LoggerService.LogError(message, ex);
                OnErrorOccurred(new DataErrorEventArgs(
                    ErrorSource.Unknown,
                    message,
                    ex));
                return DataOperationResult<TipViewModel>.Failure(ErrorSource.Unknown, message, ex);
            }
        }

        /// <summary>
        /// Updates an existing tip
        /// </summary>
        public override async Task<DataOperationResult<bool>> UpdateAsync(TipViewModel entity)
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
                        "Cannot update tip with invalid ID");
                }

                int rowsAffected = await dataB.UpdateAsync(entity);
                bool success = rowsAffected > 0;

                return DataOperationResult<bool>.Success(success);
            }
            catch (SQLiteException ex)
            {
                string message = $"Database error updating tip: {ex.Message}";
             //   LoggerService.LogError(message, ex);
                OnErrorOccurred(new DataErrorEventArgs(
                    ErrorSource.Database,
                    message,
                    ex));
                return DataOperationResult<bool>.Failure(ErrorSource.Database, message, ex);
            }
            catch (Exception ex)
            {
                string message = $"Error updating tip: {ex.Message}";
            //    LoggerService.LogError(message, ex);
                OnErrorOccurred(new DataErrorEventArgs(
                    ErrorSource.Unknown,
                    message,
                    ex));
                return DataOperationResult<bool>.Failure(ErrorSource.Unknown, message, ex);
            }
        }

        /// <summary>
        /// Deletes a tip by ID
        /// </summary>
        public override async Task<DataOperationResult<bool>> DeleteAsync(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return DataOperationResult<bool>.Failure(
                        ErrorSource.ModelValidation,
                        "Cannot delete tip with invalid ID");
                }

                int rowsAffected = await dataB.DeleteAsync<TipViewModel>(id);
                bool success = rowsAffected > 0;

                return DataOperationResult<bool>.Success(success);
            }
            catch (SQLiteException ex)
            {
                string message = $"Database error deleting tip with ID {id}: {ex.Message}";
            //    LoggerService.LogError(message, ex);
                OnErrorOccurred(new DataErrorEventArgs(
                    ErrorSource.Database,
                    message,
                    ex));
                return DataOperationResult<bool>.Failure(ErrorSource.Database, message, ex);
            }
            catch (Exception ex)
            {
                string message = $"Error deleting tip with ID {id}: {ex.Message}";
            //    LoggerService.LogError(message, ex);
                OnErrorOccurred(new DataErrorEventArgs(
                    ErrorSource.Unknown,
                    message,
                    ex));
                return DataOperationResult<bool>.Failure(ErrorSource.Unknown, message, ex);
            }
        }

        /// <summary>
        /// Deletes a tip entity
        /// </summary>
        public override async Task<DataOperationResult<bool>> DeleteAsync(TipViewModel entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            return await DeleteAsync(entity.Id);
        }

        /// <summary>
        /// Gets a tip by title
        /// </summary>
        public async Task<DataOperationResult<TipViewModel>> GetByTitleAsync(string title)
        {
            try
            {
                if (string.IsNullOrEmpty(title))
                {
                    return DataOperationResult<TipViewModel>.Failure(
                        ErrorSource.ModelValidation,
                        "Tip title cannot be empty");
                }

                var result = await dataB.Table<TipViewModel>()
                    .Where(x => x.Description == title)
                    .FirstOrDefaultAsync();

                if (result == null)
                {
                    return DataOperationResult<TipViewModel>.Failure(
                        ErrorSource.Database,
                        $"Tip with title '{title}' not found");
                }

                return DataOperationResult<TipViewModel>.Success(result);
            }
            catch (SQLiteException ex)
            {
                string message = $"Database error retrieving tip with title '{title}': {ex.Message}";
             //   LoggerService.LogError(message, ex);
                OnErrorOccurred(new DataErrorEventArgs(
                    ErrorSource.Database,
                    message,
                    ex));
                return DataOperationResult<TipViewModel>.Failure(ErrorSource.Database, message, ex);
            }
            catch (Exception ex)
            {
                string message = $"Error retrieving tip with title '{title}': {ex.Message}";
            //    LoggerService.LogError(message, ex);
                OnErrorOccurred(new DataErrorEventArgs(
                    ErrorSource.Unknown,
                    message,
                    ex));
                return DataOperationResult<TipViewModel>.Failure(ErrorSource.Unknown, message, ex);
            }
        }
    }
}