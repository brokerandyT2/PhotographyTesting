using Location.Core.Helpers.AlertService;
//using Location.Core.Helpers.LoggingService;
using Locations.Core.Business.DataAccess.Interfaces;
using Locations.Core.Data.Models;
using Locations.Core.Data.Queries.Interfaces;
using Locations.Core.Shared;
using Locations.Core.Shared.DTO;
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
    /// Service for settings-related operations
    /// </summary>
    /// <typeparam name="T">The view model type for settings</typeparam>
    public class SettingsService<T> : ISettingService<T>
        where T : SettingViewModel, new()
    {
        private readonly ISettingsRepository _repository;
        private readonly IAlertService _alertService;
        private readonly ILoggerService _loggerService;

        /// <summary>
        /// Event triggered when an error occurs in the service
        /// </summary>
        public event DataErrorEventHandler ErrorOccurred;

        /// <summary>
        /// Creates a new instance of the SettingsService with dependencies
        /// </summary>
        /// <param name="repository">The settings repository</param>
        /// <param name="alertService">The alert service</param>
        /// <param name="loggerService">The logger service</param>
        public SettingsService(
            ISettingsRepository repository,
            IAlertService alertService,
            ILoggerService loggerService)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _alertService = alertService ?? throw new ArgumentNullException(nameof(alertService));
            _loggerService = loggerService ?? throw new ArgumentNullException(nameof(loggerService));

            // Connect repository events if available
            if (_repository is ISettingsRepository repoWithEvents)
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
                    // Create a DTO from the viewModel to use with InitializeFromDTO
                    var dto = new SettingDTO
                    {
                        Id = result.Data.Id,
                        Key = result.Data.Key,
                        Value = result.Data.Value,
                        Description = result.Data.Description
                    };
                    viewModel.InitializeFromDTO(dto);
                    return DataOperationResult<T>.Success(viewModel);
                }
                else
                {
                    // Create a new failure result for type T
                    return DataOperationResult<T>.Failure(
                        result.ErrorSource,
                        $"Failed to retrieve setting with ID {id}",
                        result.Exception);
                }
            }
            catch (Exception ex)
            {
                var errorArgs = CreateErrorEventArgs(ErrorSource.Unknown,
                    $"Error retrieving setting with ID {id}: {ex.Message}", ex);
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
                        // Create a DTO from the viewModel to use with InitializeFromDTO
                        var dto = new SettingDTO
                        {
                            Id = item.Id,
                            Key = item.Key,
                            Value = item.Value,
                            Description = item.Description
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
                        "Failed to retrieve all settings",
                        result.Exception);
                }
            }
            catch (Exception ex)
            {
                var errorArgs = CreateErrorEventArgs(ErrorSource.Unknown,
                    $"Error retrieving all settings: {ex.Message}", ex);
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

                // Create a SettingViewModel from the entity
                var settingViewModel = new SettingViewModel
                {
                    Id = entity.Id,
                    Key = entity.Key,
                    Value = entity.Value,
                    Description = entity.Description
                };

                var result = await _repository.SaveAsync(settingViewModel);

                if (result.IsSuccess && result.Data != null)
                {
                    // Convert the result back to T
                    // Create a DTO first
                    var dto = new SettingDTO
                    {
                        Id = result.Data.Id,
                        Key = result.Data.Key,
                        Value = result.Data.Value,
                        Description = result.Data.Description
                    };
                    entity.InitializeFromDTO(dto);
                    return DataOperationResult<T>.Success(entity);
                }
                else
                {
                    // Create a new failure result
                    return DataOperationResult<T>.Failure(
                        result.ErrorSource,
                        "Failed to save setting",
                        result.Exception);
                }
            }
            catch (Exception ex)
            {
                var errorArgs = CreateErrorEventArgs(ErrorSource.Unknown,
                    $"Error saving setting: {ex.Message}", ex);
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

                // Create a SettingViewModel from the entity
                var settingViewModel = new SettingViewModel
                {
                    Id = entity.Id,
                    Key = entity.Key,
                    Value = entity.Value,
                    Description = entity.Description
                };

                var result = await _repository.UpdateAsync(settingViewModel);
                return result; // This already returns DataOperationResult<bool>
            }
            catch (Exception ex)
            {
                var errorArgs = CreateErrorEventArgs(ErrorSource.Unknown,
                    $"Error updating setting: {ex.Message}", ex);
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
                    $"Error deleting setting with ID {id}: {ex.Message}", ex);
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
                    throw new InvalidOperationException("Cannot delete setting with invalid ID");
                }

                return await DeleteAsync(id);
            }
            catch (Exception ex)
            {
                var errorArgs = CreateErrorEventArgs(ErrorSource.Unknown,
                    $"Error deleting setting: {ex.Message}", ex);
                OnErrorOccurred(errorArgs);
                return DataOperationResult<bool>.Failure(ErrorSource.Unknown, errorArgs.Message, ex);
            }
        }

        /// <summary>
        /// Gets a setting by its name
        /// </summary>
        public T GetSettingByName(string name)
        {
            try
            {
                // Use the repository to find the setting
                var task = Task.Run(async () =>
                    await _repository.GetByNameAsync(name));
                var result = task.Result;

                if (result.IsSuccess && result.Data != null)
                {
                    // Convert to view model
                    var viewModel = new T();
                    // Create a DTO first
                    var dto = new SettingDTO
                    {
                        Id = result.Data.Id,
                        Key = result.Data.Key,
                        Value = result.Data.Value,
                        Description = result.Data.Description
                    };
                    viewModel.InitializeFromDTO(dto);
                    return viewModel;
                }

                // If operation failed or no data found
                _loggerService.LogWarning($"Failed to find setting with name '{name}'");
                return new T { Key = name, Value = string.Empty };
            }
            catch (Exception ex)
            {
                var errorArgs = CreateErrorEventArgs(ErrorSource.Unknown,
                    $"Error retrieving setting with name '{name}': {ex.Message}", ex);
                OnErrorOccurred(errorArgs);
                return new T { Key = name, Value = string.Empty };
            }
        }

        /// <summary>
        /// Gets all application settings
        /// </summary>
        public SettingsViewModel GetAllSettings()
        {
            try
            {
                // Get all settings
                var task = Task.Run(async () => await GetAllAsync());
                var result = task.Result;

                if (result.IsSuccess && result.Data != null)
                {
                    // Create and populate a SettingsViewModel
                    var settingsVM = new SettingsViewModel();

                    // Map the individual settings
                    foreach (var setting in result.Data)
                    {
                        MapSettingToViewModel(settingsVM, setting);
                    }

                    return settingsVM;
                }

                return new SettingsViewModel();
            }
            catch (Exception ex)
            {
                var errorArgs = CreateErrorEventArgs(ErrorSource.Unknown,
                    $"Error retrieving all settings: {ex.Message}", ex);
                OnErrorOccurred(errorArgs);
                return new SettingsViewModel();
            }
        }

        /// <summary>
        /// Gets a setting value by its key using a magic string
        /// </summary>
        public string GetSettingWithMagicString(string key)
        {
            try
            {
                var setting = GetSettingByName(key);
                return setting?.Value ?? string.Empty;
            }
            catch (Exception ex)
            {
                var errorArgs = CreateErrorEventArgs(ErrorSource.Unknown,
                    $"Error retrieving setting value for key '{key}': {ex.Message}", ex);
                OnErrorOccurred(errorArgs);
                return string.Empty;
            }
        }

        /// <summary>
        /// Saves a setting synchronously by name and value
        /// </summary>
        public bool SaveSetting(string name, string value)
        {
            try
            {
                // First, try to get the existing setting
                var setting = GetSettingByName(name);

                // If setting exists, update its value
                if (setting != null && setting.Id > 0)
                {
                    setting.Value = value;
                    var task = Task.Run(async () => await UpdateAsync(setting));
                    var result = task.Result;
                    return result.IsSuccess;
                }
                else
                {
                    // Create a new setting
                    var newSetting = new T
                    {
                        Key = name,
                        Value = value
                    };

                    var task = Task.Run(async () => await SaveAsync(newSetting));
                    var result = task.Result;
                    return result.IsSuccess;
                }
            }
            catch (Exception ex)
            {
                var errorArgs = CreateErrorEventArgs(ErrorSource.Unknown,
                    $"Error saving setting '{name}': {ex.Message}", ex);
                OnErrorOccurred(errorArgs);
                return false;
            }
        }

        /// <summary>
        /// Saves all settings from a SettingsViewModel
        /// </summary>
        public async Task<bool> SaveAllSettingsAsync(SettingsViewModel settingsVM)
        {
            try
            {
                if (settingsVM == null)
                {
                    return false;
                }

                // Get properties of the SettingsViewModel that are of type SettingViewModel
                var settingProperties = typeof(SettingsViewModel).GetProperties()
                    .Where(p => p.PropertyType == typeof(SettingViewModel));

                bool allSucceeded = true;

                // Save each setting
                foreach (var property in settingProperties)
                {
                    var setting = property.GetValue(settingsVM) as SettingViewModel;
                    if (setting != null)
                    {
                        var result = await SaveOrUpdateSettingAsync(setting);
                        if (!result)
                        {
                            allSucceeded = false;
                        }
                    }
                }

                return allSucceeded;
            }
            catch (Exception ex)
            {
                var errorArgs = CreateErrorEventArgs(ErrorSource.Unknown,
                    $"Error saving all settings: {ex.Message}", ex);
                OnErrorOccurred(errorArgs);
                return false;
            }
        }

        /// <summary>
        /// Saves or updates a setting
        /// </summary>
        private async Task<bool> SaveOrUpdateSettingAsync(SettingViewModel setting)
        {
            try
            {
                // Try to get the existing setting
                var existingSetting = GetSettingByName(setting.Key);

                if (existingSetting != null && existingSetting.Id > 0)
                {
                    // Update existing setting
                    existingSetting.Value = setting.Value;
                    existingSetting.Description = setting.Description;
                    var updateResult = await UpdateAsync(existingSetting as T);
                    return updateResult.IsSuccess;
                }
                else
                {
                    // Create new setting
                    var newSetting = new T
                    {
                        Key = setting.Key,
                        Value = setting.Value,
                        Description = setting.Description
                    };

                    var saveResult = await SaveAsync(newSetting);
                    return saveResult.IsSuccess;
                }
            }
            catch (Exception ex)
            {
                _loggerService.LogError($"Error saving/updating setting '{setting.Key}': {ex.Message}", ex);
                return false;
            }
        }

        /// <summary>
        /// Maps a setting to a property in SettingsViewModel
        /// </summary>
        private void MapSettingToViewModel(SettingsViewModel settingsVM, T setting)
        {
            try
            {
                // Try to find a property in SettingsViewModel with the same name
                var property = typeof(SettingsViewModel).GetProperty(setting.Key);
                if (property != null && property.PropertyType == typeof(SettingViewModel))
                {
                    property.SetValue(settingsVM, setting);
                }
                else
                {
                    // Handle special cases based on known setting keys
                    switch (setting.Key)
                    {
                        case "Email":
                            settingsVM.Email = setting;
                            break;
                        case "DateFormat":
                            settingsVM.DateFormat = setting;
                            break;
                        case "TimeFormat":
                            settingsVM.TimeFormat = setting;
                            break;
                        // Add more cases for other known settings
                        default:
                            // Log that we couldn't map this setting
                            _loggerService.LogWarning($"Could not map setting '{setting.Key}' to SettingsViewModel");
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                _loggerService.LogError($"Error mapping setting '{setting.Key}': {ex.Message}", ex);
            }
        }
        public void OnErrorOccurred(Locations.Core.Data.Models.DataErrorEventArgs e)
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

    }
}