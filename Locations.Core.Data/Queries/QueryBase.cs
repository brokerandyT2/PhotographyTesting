using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;
using Locations.Core.Data.Queries.Interfaces;
using Locations.Core.Data.Models;
using Locations.Core.Data.Interfaces;
using Location.Core.Helpers.AlertService;
using Location.Core.Helpers.LoggingService;
using Locations.Core.Shared;
using Locations.Core.Shared.StorageSvc;

namespace Locations.Core.Data.Queries
{
    /// <summary>
    /// Base class for queries with common functionality
    /// </summary>
    public abstract class QueryBase<T> : Database, IDataAccessBase
    {
        protected QueryBase(IAlertService alertService, ILoggerService loggerService)
            : base(alertService, loggerService)
        {
        }

        public abstract Task<DataOperationResult<T>> GetItemAsync<T>(int id) where T : new();
        public abstract Task<DataOperationResult<string>> GetValueByStringAsync<T>(string name) where T : new();
        public abstract Task<DataOperationResult<IList<T>>> GetItemsAsync<T>() where T : new();
        public abstract Task<DataOperationResult<T>> GetItemByStringAsync<T>(string name) where T : new();

        /// <summary>
        /// Deletes an item by ID
        /// </summary>
        public virtual async Task<DataOperationResult<bool>> DeleteItemAsync<T>(int id) where T : new()
        {
            try
            {
                var itemResult = await GetItemAsync<T>(id);
                if (!itemResult.IsSuccess)
                {
                    return DataOperationResult<bool>.Failure(
                        ErrorSource.Database,
                        $"Failed to find item with ID {id} for deletion");
                }

                var result = await dataB.DeleteAsync(itemResult.Data);
                return result > 0
                    ? DataOperationResult<bool>.Success(true)
                    : DataOperationResult<bool>.Failure(ErrorSource.Database, "Item was not deleted");
            }
            catch (SQLiteException ex)
            {
                string message = $"Database error while deleting item ID {id}: {ex.Message}";
                LoggerService.LogError(message, ex);
                OnErrorOccurred(new DataErrorEventArgs(
                    ErrorSource.Database,
                    message,
                    ex));
                return DataOperationResult<bool>.Failure(ErrorSource.Database, message, ex);
            }
            catch (Exception ex)
            {
                string message = $"Error deleting item ID {id}: {ex.Message}";
                LoggerService.LogError(message, ex);
                OnErrorOccurred(new DataErrorEventArgs(
                    ErrorSource.Unknown,
                    message,
                    ex));
                return DataOperationResult<bool>.Failure(ErrorSource.Unknown, message, ex);
            }
        }

        /// <summary>
        /// Saves an item to the database
        /// </summary>
        public virtual async Task<DataOperationResult<bool>> SaveItemAsync<T>(T item) where T : new()
        {
            try
            {
                // Check if item implements IValidatable
                if (item is IValidatable validatable)
                {
                    if (!validatable.Validate(out List<string> errors))
                    {
                        string validationErrors = string.Join(", ", errors);
                        string message = $"Validation failed: {validationErrors}";

                        LoggerService.LogWarning(message);
                        OnErrorOccurred(new DataErrorEventArgs(
                            ErrorSource.ModelValidation,
                            message,
                            null,
                            item));

                        return DataOperationResult<bool>.Failure(
                            ErrorSource.ModelValidation,
                            message);
                    }
                }

                var result = await dataB.InsertOrReplaceAsync(item);
                return result > 0
                    ? DataOperationResult<bool>.Success(true)
                    : DataOperationResult<bool>.Failure(ErrorSource.Database, "Item was not saved");
            }
            catch (SQLiteException ex)
            {
                string message = $"Database error while saving item: {ex.Message}";
                LoggerService.LogError(message, ex);
                OnErrorOccurred(new DataErrorEventArgs(
                    ErrorSource.Database,
                    message,
                    ex,
                    item));
                return DataOperationResult<bool>.Failure(ErrorSource.Database, message, ex);
            }
            catch (Exception ex)
            {
                string message = $"Error saving item: {ex.Message}";
                LoggerService.LogError(message, ex);
                OnErrorOccurred(new DataErrorEventArgs(
                    ErrorSource.Unknown,
                    message,
                    ex,
                    item));
                return DataOperationResult<bool>.Failure(ErrorSource.Unknown, message, ex);
            }
        }

        /// <summary>
        /// Updates an item in the database
        /// </summary>
        public virtual Task<DataOperationResult<bool>> UpdateAsync<T>(T item) where T : new()
        {
            // Update is the same as save in this implementation
            return SaveItemAsync(item);
        }

        /// <summary>
        /// Saves an item and returns it with ID populated
        /// </summary>
        public virtual async Task<DataOperationResult<T>> SaveWithIDReturnAsync<T>(T item) where T : new()
        {
            try
            {
                // Check if item implements IValidatable
                if (item is IValidatable validatable)
                {
                    if (!validatable.Validate(out List<string> errors))
                    {
                        string validationErrors = string.Join(", ", errors);
                        string message = $"Validation failed: {validationErrors}";

                        LoggerService.LogWarning(message);
                        OnErrorOccurred(new DataErrorEventArgs(
                            ErrorSource.ModelValidation,
                            message,
                            null,
                            item));

                        return DataOperationResult<T>.Failure(
                            ErrorSource.ModelValidation,
                            message);
                    }
                }

                // Get the encryption key components
                string email = NativeStorageService.GetSetting(MagicStrings.Email);
                string guid = NativeStorageService.GetSetting(MagicStrings.UniqueID);
                string key = guid + email;

                // Get a synchronous connection for this operation
                SQLiteConnection conn = GetSyncEncryptedConnection();
                conn.Insert(item);

                return DataOperationResult<T>.Success(item);
            }
            catch (SQLiteException ex)
            {
                string message = $"Database error while saving item with ID return: {ex.Message}";
                LoggerService.LogError(message, ex);
                OnErrorOccurred(new DataErrorEventArgs(
                    ErrorSource.Database,
                    message,
                    ex,
                    item));
                return DataOperationResult<T>.Failure(ErrorSource.Database, message, ex);
            }
            catch (Exception ex)
            {
                string message = $"Error saving item with ID return: {ex.Message}";
                LoggerService.LogError(message, ex);
                OnErrorOccurred(new DataErrorEventArgs(
                    ErrorSource.Unknown,
                    message,
                    ex,
                    item));
                return DataOperationResult<T>.Failure(ErrorSource.Unknown, message, ex);
            }
        }
    }
}