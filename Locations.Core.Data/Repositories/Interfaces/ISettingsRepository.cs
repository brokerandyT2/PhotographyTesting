using System.Threading.Tasks;
using Locations.Core.Data.Models;
using Locations.Core.Data.Queries.Interfaces;
using Locations.Core.Shared.ViewModels;

namespace Locations.Core.Data.Queries.Interfaces
{
    /// <summary>
    /// Repository interface for settings operations
    /// </summary>
    public interface ISettingsRepository : IRepository<SettingViewModel>
    {
        /// <summary>
        /// Gets a setting by name
        /// </summary>
        Task<DataOperationResult<SettingViewModel>> GetByNameAsync(string name);

        /// <summary>
        /// Gets a setting value by name
        /// </summary>
        Task<DataOperationResult<string>> GetValueByNameAsync(string name);

        /// <summary>
        /// Saves a setting with name and value
        /// </summary>
        Task<DataOperationResult<bool>> SaveSettingAsync(string name, string value);
    }
}