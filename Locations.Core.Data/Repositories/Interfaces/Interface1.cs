using System.Threading.Tasks;
using Locations.Core.Data.Models;
using Locations.Core.Data.Queries.Interfaces;
using Locations.Core.Shared.ViewModels;

namespace Locations.Core.Data.Queries.Interfaces
{
    /// <summary>
    /// Repository interface for tip operations
    /// </summary>
    public interface ITipRepository : IRepository<TipViewModel>
    {
        /// <summary>
        /// Gets a tip by title
        /// </summary>
        Task<DataOperationResult<TipViewModel>> GetByTitleAsync(string title);
    }
}