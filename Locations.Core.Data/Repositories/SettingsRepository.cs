using System;
using System.Threading.Tasks;
using SQLite;
using Location.Core.Helpers.AlertService;
using Location.Core.Helpers.LoggingService;
using Locations.Core.Data.Models;
using Locations.Core.Data.Queries.Interfaces;
using Locations.Core.Shared.ViewModels;
using DataErrorEventArgs = Locations.Core.Data.Models.DataErrorEventArgs;
using ErrorSource = Locations.Core.Data.Models.ErrorSource;
namespace Locations.Core.Data.Queries
{
    /// <summary>
    /// Repository implementation for settings operations
    /// </summary>
    public class SettingsRepository : RepositoryBase<SettingViewModel>, ISettingsRepository
    {
        public SettingsRepository(IAlertService alertService, ILoggerService loggerService)
            : base(alertService, loggerService)
        {
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

                SettingViewModel setting;
                if (existingSetting != null)
                {
                    // Update existing setting
                    existingSetting.Value = value;
                    setting = existingSetting;
                }
                else
                {
                    // Create new setting
                    setting = new SettingViewModel
                    {
                        Key = name,
                        Value = value
                    };
                }

                // Save the setting
                return await SaveAsync(setting);
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