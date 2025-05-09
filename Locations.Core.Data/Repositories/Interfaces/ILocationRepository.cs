using System.Threading.Tasks;
using Locations.Core.Shared.ViewModels;
using Locations.Core.Data.Queries.Interfaces;
using Locations.Core.Data.Models;


namespace Locations.Core.Data.Queries.Interfaces
{
    /// <summary>
    /// Repository interface for location operations
    /// </summary>
    public interface ILocationRepository : IRepository<LocationViewModel>
    {
        /// <summary>
        /// Gets a location by its geographic coordinates
        /// </summary>
        Task<DataOperationResult<LocationViewModel>> GetByCoordinatesAsync(double latitude, double longitude);
    }
}