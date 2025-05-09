using Locations.Core.Shared.ViewModels;
using System;
using System.Threading.Tasks;

namespace Locations.Core.Business.DataAccess.Interfaces
{
    /// <summary>
    /// Interface for a service that provides secure storage capabilities for app settings
    /// </summary>
    public interface INativeStorageService
    {
        /// <summary>
        /// Retrieves a setting from secure storage
        /// </summary>
        /// <param name="key">The key of the setting to retrieve</param>
        /// <returns>The value of the setting, or empty string if not found or an error occurs</returns>
        Task<string> GetSettingAsync(string key);

        /// <summary>
        /// Saves a setting to secure storage
        /// </summary>
        /// <param name="key">The key to store the value under</param>
        /// <param name="value">The value to store</param>
        /// <returns>A task representing the asynchronous operation</returns>
        Task SaveSettingAsync(string key, string value);

        /// <summary>
        /// Saves an object to secure storage by converting it to a string
        /// </summary>
        /// <param name="key">The key to store the value under</param>
        /// <param name="value">The object to store</param>
        /// <returns>A task representing the asynchronous operation</returns>
        Task SaveSettingAsync(string key, object value);

        /// <summary>
        /// Updates an existing setting if the old value matches
        /// </summary>
        /// <param name="key">The key of the setting to update</param>
        /// <param name="oldValue">The expected current value</param>
        /// <param name="newValue">The new value to set</param>
        /// <returns>Task with bool result indicating success</returns>
        Task<bool> UpdateSettingAsync(string key, string oldValue, string newValue);

        /// <summary>
        /// Updates an existing setting if the old value matches, converting objects to strings
        /// </summary>
        /// <param name="key">The key of the setting to update</param>
        /// <param name="oldValue">The expected current value as an object</param>
        /// <param name="newValue">The new value to set as an object</param>
        /// <returns>Task with bool result indicating success</returns>
        Task<bool> UpdateSettingAsync(string key, object oldValue, object newValue);

        /// <summary>
        /// Deletes a setting from secure storage
        /// </summary>
        /// <param name="key">The key of the setting to delete</param>
        /// <returns>True if the setting was successfully deleted, false otherwise</returns>
        Task<bool> DeleteSettingAsync(string key);

        /// <summary>
        /// Event that is raised when an error occurs in the storage service
        /// </summary>
        event EventHandler<OperationErrorEventArgs> ErrorOccurred;
    }
}