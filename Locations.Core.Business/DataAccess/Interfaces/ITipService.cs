using Locations.Core.Business.DataAccess.Base;
using Locations.Core.Shared.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Locations.Core.Business.DataAccess.Interfaces
{
    /// <summary>
    /// Interface for tip service operations
    /// </summary>
    /// <typeparam name="T">The view model type for tips</typeparam>
    public interface ITipService<T> : IServiceBase<T> where T : class, new()
    {
        /// <summary>
        /// Gets all tip types
        /// </summary>
        /// <returns>A list of tip type view models</returns>
        Task<Locations.Core.Shared.ViewModels.OperationResult<List<TipTypeViewModel>>> GetTipTypesAsync();

        /// <summary>
        /// Gets a random tip for a specific type
        /// </summary>
        /// <param name="tipTypeId">The tip type ID</param>
        /// <returns>A random tip view model</returns>
        Task<Locations.Core.Shared.ViewModels.OperationResult<TipViewModel>> GetRandomTipForTypeAsync(int tipTypeId);

        /// <summary>
        /// Gets all tips for a specific type
        /// </summary>
        /// <param name="tipTypeId">The tip type ID</param>
        /// <returns>A list of tip view models</returns>
        Task<Locations.Core.Shared.ViewModels.OperationResult<List<TipViewModel>>> GetTipsForTypeAsync(int tipTypeId);
    }
}