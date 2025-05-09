using Locations.Core.Business.DataAccess.Base;
using Locations.Core.Shared.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Locations.Core.Business.DataAccess.Interfaces
{
    /// <summary>
    /// Interface for tip type service operations
    /// </summary>
    /// <typeparam name="T">The view model type for tip types</typeparam>
    public interface ITipTypeService<T> : IServiceBase<T> where T : class, new()
    {
        /// <summary>
        /// Gets a tip type by its name
        /// </summary>
        /// <param name="name">The tip type name</param>
        /// <returns>The tip type if found</returns>
        Task<Locations.Core.Shared.ViewModels.OperationResult<T>> GetByNameAsync(string name);

        /// <summary>
        /// Gets all tip types sorted by display order
        /// </summary>
        /// <returns>A list of tip type view models sorted by display order</returns>
        Task<Locations.Core.Shared.ViewModels.OperationResult<List<T>>> GetAllSortedAsync();
    }
}