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
    /// Repository implementation for location operations
    /// </summary>
    public class LocationRepository : RepositoryBase<LocationViewModel>, ILocationRepository
    {
        public LocationRepository(IAlertService alertService, ILoggerService loggerService)
            : base(alertService, loggerService)
        {
        }

        /// <summary>
        /// Gets a location by its geographic coordinates
        /// </summary>
        public async Task<DataOperationResult<LocationViewModel>> GetByCoordinatesAsync(double latitude, double longitude)
        {
            try
            {
                var result = await dataB.Table<LocationViewModel>()
                    .Where(x => x.Lattitude == latitude && x.Longitude == longitude)
                    .FirstOrDefaultAsync();

                if (result == null)
                {
                    return DataOperationResult<LocationViewModel>.Failure(
                        ErrorSource.Database,
                        $"Location at coordinates ({latitude}, {longitude}) not found");
                }

                return DataOperationResult<LocationViewModel>.Success(result);
            }
            catch (SQLiteException ex)
            {
                string message = $"Database error retrieving location at coordinates ({latitude}, {longitude}): {ex.Message}";
                LoggerService.LogError(message, ex);
                OnErrorOccurred(new DataErrorEventArgs(
                    ErrorSource.Database,
                    message,
                    ex));
                return DataOperationResult<LocationViewModel>.Failure(ErrorSource.Database, message, ex);
            }
            catch (Exception ex)
            {
                string message = $"Error retrieving location at coordinates ({latitude}, {longitude}): {ex.Message}";
                LoggerService.LogError(message, ex);
                OnErrorOccurred(new DataErrorEventArgs(
                    ErrorSource.Unknown,
                    message,
                    ex));
                return DataOperationResult<LocationViewModel>.Failure(ErrorSource.Unknown, message, ex);
            }
        }
    }
}