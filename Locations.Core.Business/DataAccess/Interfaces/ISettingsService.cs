using Locations.Core.Business.DataAccess.Base;
using Locations.Core.Shared.ViewModels;
using System.Threading.Tasks;

namespace Locations.Core.Business.DataAccess.Interfaces
{
    /// <summary>
    /// Interface for settings service operations
    /// </summary>
    /// <typeparam name="T">The view model type for settings</typeparam>
    public interface ISettingService<T> : IServiceBase<T> where T : class, new()
    {
        /// <summary>
        /// Gets a setting by its name
        /// </summary>
        /// <param name="name">The setting name</param>
        /// <returns>The setting if found</returns>
        T GetSettingByName(string name);

        /// <summary>
        /// Gets all application settings
        /// </summary>
        /// <returns>A settings view model containing all settings</returns>
        SettingsViewModel GetAllSettings();

        /// <summary>
        /// Gets a setting value by its key using a magic string
        /// </summary>
        /// <param name="key">The setting key</param>
        /// <returns>The setting value</returns>
        string GetSettingWithMagicString(string key);

        bool SaveSetting(string name, string value);
    }
}