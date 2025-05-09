using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;
using Location.Core.Helpers.AlertService;
using Location.Core.Helpers.LoggingService;
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
    /// Repository implementation for settings operations
    /// </summary>
    public class SettingsRepository : RepositoryBase<SettingViewModel>, ISettingsRepository
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
        /// Creates a new settings repository
        /// </summary>
        public SettingsRepository(IAlertService alertService, ILoggerService loggerService)
            : base()
        {
            AlertService = alertService ?? throw new ArgumentNullException(nameof(alertService));
            LoggerService = loggerService ?? throw new ArgumentNullException(nameof(loggerService));

            // Initialize database connection
            dataB = DataEncrypted.GetAsyncConnection();
        }

        /// <summary>
        /// Gets a setting by its ID
        /// </summary>
        public override async Task<DataOperationResult<SettingViewModel>> GetByIdAsync(int id)
        {
            try
            {
                var result = await dataB.Table<SettingViewModel>()
                    .Where(x => x.Id == id)
                    .FirstOrDefaultAsync();

                if (result == null)
                {
                    return DataOperationResult<SettingViewModel>.Failure(
                        ErrorSource.Database,
                        $"Setting with ID {id} not found");
                }

                return DataOperationResult<SettingViewModel>.Success(result);
            }
            catch (SQLiteException ex)
            {
                string message = $"Database error retrieving setting with ID {id}: {ex.Message}";
                LoggerService.LogError(message, ex);
                OnErrorOccurred(new DataErrorEventArgs(
                    ErrorSource.Database,
                    message,
                    ex));
                return DataOperationResult<SettingViewModel>.Failure(ErrorSource.Database, message, ex);
            }
            catch (Exception ex)
            {
                string message = $"Error retrieving setting with ID {id}: {ex.Message}";
                LoggerService.LogError(message, ex);
                OnErrorOccurred(new DataErrorEventArgs(
                    ErrorSource.Unknown,
                    message,
                    ex));
                return DataOperationResult<SettingViewModel>.Failure(ErrorSource.Unknown, message, ex);
            }
        }

        /// <summary>
        /// Gets all settings
        /// </summary>
        public override async Task<DataOperationResult<IList<SettingViewModel>>> GetAllAsync()
        {
            try
            {
                var results = await dataB.Table<SettingViewModel>().ToListAsync();
                return DataOperationResult<IList<SettingViewModel>>.Success(results);
            }
            catch (SQLiteException ex)
            {
                string message = $"Database error retrieving all settings: {ex.Message}";
                LoggerService.LogError(message, ex);
                OnErrorOccurred(new DataErrorEventArgs(
                    ErrorSource.Database,
                    message,
                    ex));
                return DataOperationResult<IList<SettingViewModel>>.Failure(ErrorSource.Database, message, ex);
            }
            catch (Exception ex)
            {
                string message = $"Error retrieving all settings: {ex.Message}";
                LoggerService.LogError(message, ex);
                OnErrorOccurred(new DataErrorEventArgs(
                    ErrorSource.Unknown,
                    message,
                    ex));
                return DataOperationResult<IList<SettingViewModel>>.Failure(ErrorSource.Unknown, message, ex);
            }
        }

        /// <summary>
        /// Saves a setting
        /// </summary>
        public override async Task<DataOperationResult<SettingViewModel>> SaveAsync(SettingViewModel entity)
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

                return DataOperationResult<SettingViewModel>.Success(entity);
            }
            catch (SQLiteException ex)
            {
                string message = $"Database error saving setting: {ex.Message}";
                LoggerService.LogError(message, ex);
                OnErrorOccurred(new DataErrorEventArgs(
                    ErrorSource.Database,
                    message,
                    ex));
                return DataOperationResult<SettingViewModel>.Failure(ErrorSource.Database, message, ex);
            }
            catch (Exception ex)
            {
                string message = $"Error saving setting: {ex.Message}";
                LoggerService.LogError(message, ex);
                OnErrorOccurred(new DataErrorEventArgs(
                    ErrorSource.Unknown,
                    message,
                    ex));
                return DataOperationResult<SettingViewModel>.Failure(ErrorSource.Unknown, message, ex);
            }
        }

        /// <summary>
        /// Updates an existing setting
        /// </summary>
        public override async Task<DataOperationResult<bool>> UpdateAsync(SettingViewModel entity)
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
                        "Cannot update setting with invalid ID");
                }

                int rowsAffected = await dataB.UpdateAsync(entity);
                bool success = rowsAffected > 0;

                return DataOperationResult<bool>.Success(success);
            }
            catch (SQLiteException ex)
            {
                string message = $"Database error updating setting: {ex.Message}";
                LoggerService.LogError(message, ex);
                OnErrorOccurred(new DataErrorEventArgs(
                    ErrorSource.Database,
                    message,
                    ex));
                return DataOperationResult<bool>.Failure(ErrorSource.Database, message, ex);
            }
            catch (Exception ex)
            {
                string message = $"Error updating setting: {ex.Message}";
                LoggerService.LogError(message, ex);
                OnErrorOccurred(new DataErrorEventArgs(
                    ErrorSource.Unknown,
                    message,
                    ex));
                return DataOperationResult<bool>.Failure(ErrorSource.Unknown, message, ex);
            }
        }

        /// <summary>
        /// Deletes a setting by ID
        /// </summary>
        public override async Task<DataOperationResult<bool>> DeleteAsync(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return DataOperationResult<bool>.Failure(
                        ErrorSource.ModelValidation,
                        "Cannot delete setting with invalid ID");
                }

                int rowsAffected = await dataB.DeleteAsync<SettingViewModel>(id);
                bool success = rowsAffected > 0;

                return DataOperationResult<bool>.Success(success);
            }
            catch (SQLiteException ex)
            {
                string message = $"Database error deleting setting with ID {id}: {ex.Message}";
                LoggerService.LogError(message, ex);
                OnErrorOccurred(new DataErrorEventArgs(
                    ErrorSource.Database,
                    message,
                    ex));
                return DataOperationResult<bool>.Failure(ErrorSource.Database, message, ex);
            }
            catch (Exception ex)
            {
                string message = $"Error deleting setting with ID {id}: {ex.Message}";
                LoggerService.LogError(message, ex);
                OnErrorOccurred(new DataErrorEventArgs(
                    ErrorSource.Unknown,
                    message,
                    ex));
                return DataOperationResult<bool>.Failure(ErrorSource.Unknown, message, ex);
            }
        }

        /// <summary>
        /// Deletes a setting entity
        /// </summary>
        public override async Task<DataOperationResult<bool>> DeleteAsync(SettingViewModel entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            return await DeleteAsync(entity.Id);
        }

        /// <summary>
        /// Gets a setting by name
        /// </summary>
        public async Task<DataOperationResult<SettingViewModel>> GetByNameAsync(string name)
        {
            try
            {
                if (string.IsNullOrEmpty(name))
                {
                    return DataOperationResult<SettingViewModel>.Failure(
                        ErrorSource.ModelValidation,
                        "Setting name cannot be empty");
                }

                var result = await dataB.Table<SettingViewModel>()
                    .Where(x => x.Key == name)
                    .FirstOrDefaultAsync();

                if (result == null)
                {
                    return DataOperationResult<SettingViewModel>.Failure(
                        ErrorSource.Database,
                        $"Setting with name '{name}' not found");
                }

                return DataOperationResult<SettingViewModel>.Success(result);
            }
            catch (SQLiteException ex)
            {
                string message = $"Database error retrieving setting '{name}': {ex.Message}";
                LoggerService.LogError(message, ex);
                OnErrorOccurred(new DataErrorEventArgs(
                    ErrorSource.Database,
                    message,
                    ex));
                return DataOperationResult<SettingViewModel>.Failure(ErrorSource.Database, message, ex);
            }
            catch (Exception ex)
            {
                string message = $"Error retrieving setting '{name}': {ex.Message}";
                LoggerService.LogError(message, ex);
                OnErrorOccurred(new DataErrorEventArgs(
                    ErrorSource.Unknown,
                    message,
                    ex));
                return DataOperationResult<SettingViewModel>.Failure(ErrorSource.Unknown, message, ex);
            }
        }

        /// <summary>
        /// Gets a setting value by name
        /// </summary>
        public async Task<DataOperationResult<string>> GetValueByNameAsync(string name)
        {
            try
            {
                if (string.IsNullOrEmpty(name))
                {
                    return DataOperationResult<string>.Failure(
                        ErrorSource.ModelValidation,
                        "Setting name cannot be empty");
                }

                var setting = await dataB.Table<SettingViewModel>()
                    .Where(x => x.Key == name)
                    .FirstOrDefaultAsync();

                if (setting == null)
                {
                    return DataOperationResult<string>.Failure(
                        ErrorSource.Database,
                        $"Setting with name '{name}' not found");
                }

                return DataOperationResult<string>.Success(setting.Value);
            }
            catch (SQLiteException ex)
            {
                string message = $"Database error retrieving setting value '{name}': {ex.Message}";
                LoggerService.LogError(message, ex);
                OnErrorOccurred(new DataErrorEventArgs(
                    ErrorSource.Database,
                    message,
                    ex));
                return DataOperationResult<string>.Failure(ErrorSource.Database, message, ex);
            }
            catch (Exception ex)
            {
                string message = $"Error retrieving setting value '{name}': {ex.Message}";
                LoggerService.LogError(message, ex);
                OnErrorOccurred(new DataErrorEventArgs(
                    ErrorSource.Unknown,
                    message,
                    ex));
                return DataOperationResult<string>.Failure(ErrorSource.Unknown, message, ex);
            }
        }

        /// <summary>
        /// Saves a setting with name and value
        /// </summary>
        public async Task<DataOperationResult<bool>> SaveSettingAsync(string name, string value)
        {
            try
            {
                if (string.IsNullOrEmpty(name))
                {
                    return DataOperationResult<bool>.Failure(
                        ErrorSource.ModelValidation,
                        "Setting name cannot be empty");
                }

                // Check if setting already exists
                var existingSetting = await dataB.Table<SettingViewModel>()
                    .Where(x => x.Key == name)
                    .FirstOrDefaultAsync();

                if (existingSetting != null)
                {
                    // Update existing setting
                    existingSetting.Value = value;
                    await dataB.UpdateAsync(existingSetting);
                    return DataOperationResult<bool>.Success(true);
                }
                else
                {
                    // Create new setting
                    var setting = new SettingViewModel
                    {
                        Key = name,
                        Value = value
                    };
                    await dataB.InsertAsync(setting);
                    return DataOperationResult<bool>.Success(true);
                }
            }
            catch (SQLiteException ex)
            {
                string message = $"Database error saving setting '{name}': {ex.Message}";
                LoggerService.LogError(message, ex);
                OnErrorOccurred(new DataErrorEventArgs(
                    ErrorSource.Database,
                    message,
                    ex));
                return DataOperationResult<bool>.Failure(ErrorSource.Database, message, ex);
            }
            catch (Exception ex)
            {
                string message = $"Error saving setting '{name}': {ex.Message}";
                LoggerService.LogError(message, ex);
                OnErrorOccurred(new DataErrorEventArgs(
                    ErrorSource.Unknown,
                    message,
                    ex));
                return DataOperationResult<bool>.Failure(ErrorSource.Unknown, message, ex);
            }
        }
    }
}