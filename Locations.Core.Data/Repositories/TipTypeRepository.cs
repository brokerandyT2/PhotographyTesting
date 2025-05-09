using Location.Core.Helpers.AlertService;
using Location.Core.Helpers.LoggingService;
using Locations.Core.Data.Queries.Interfaces;
using Locations.Core.Shared.ViewModels;

namespace Locations.Core.Data.Queries
{
    /// <summary>
    /// Repository implementation for tip type operations
    /// </summary>
    public class TipTypeRepository : RepositoryBase<TipTypeViewModel>, ITipTypeRepository
    {
        public TipTypeRepository(IAlertService alertService, ILoggerService loggerService)
            : base(alertService, loggerService)
        {
        }

        // All needed functionality is inherited from RepositoryBase
    }
}