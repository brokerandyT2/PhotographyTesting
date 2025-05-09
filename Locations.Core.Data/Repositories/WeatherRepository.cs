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
    /// Repository implementation for weather operations
    /// </summary>
    public class WeatherRepository : RepositoryBase<WeatherViewModel>, IWeatherRepository
    {
        public WeatherRepository(IAlertService alertService, ILoggerService loggerService)
            : base(alertService, loggerService)
        {
        }

        /// <summary>
        /// Gets weather data by coordinates
        /// </summary>
        public async Task<DataOperationResult<WeatherViewModel>> GetByCoordinatesAsync(double latitude, double longitude)
        {
            try
            {
                var result = await dataB.Table<WeatherViewModel>()
                    .Where(x => x.Latitude == latitude && x.Longitude == longitude)
                    .FirstOrDefaultAsync();

                if (result == null)
                {
                    return DataOperationResult<WeatherViewModel>.Failure(
                        ErrorSource.Database,
                        $"Weather data for coordinates ({latitude}, {longitude}) not found");
                }

                return DataOperationResult<WeatherViewModel>.Success(result);
            }
            catch (SQLiteException ex)
            {
                string message = $"Database error retrieving weather for coordinates ({latitude}, {longitude}): {ex.Message}";
                LoggerService.LogError(message, ex);
                OnErrorOccurred(new DataErrorEventArgs(
                    ErrorSource.Database,
                    message,
                    ex));
                return DataOperationResult<WeatherViewModel>.Failure(ErrorSource.Database, message, ex);
            }
            catch (Exception ex)
            {
                string message = $"Error retrieving weather for coordinates ({latitude}, {longitude}): {ex.Message}";
                LoggerService.LogError(message, ex);
                OnErrorOccurred(new DataErrorEventArgs(
                    ErrorSource.Unknown,
                    message,
                    ex));
                return DataOperationResult<WeatherViewModel>.Failure(ErrorSource.Unknown, message, ex);
            }
        }

        /// <summary>
        /// Gets weather data by coordinates or returns a new instance if not found
        /// </summary>
        public async Task<DataOperationResult<WeatherViewModel>> GetByCoordinatesOrDefaultAsync(double latitude, double longitude)
        {
            try
            {
                var result = await GetByCoordinatesAsync(latitude, longitude);

                // If the weather data was found, return it
                if (result.IsSuccess)
                {
                    return result;
                }

                // Otherwise, return a new instance
                return DataOperationResult<WeatherViewModel>.Success(new WeatherViewModel
                {
                    Latitude = latitude,
                    Longitude = longitude
                });
            }
            catch (Exception ex)
            {
                string message = $"Error retrieving weather for coordinates ({latitude}, {longitude}): {ex.Message}";
                LoggerService.LogError(message, ex);
                OnErrorOccurred(new DataErrorEventArgs(
                    ErrorSource.Unknown,
                    message,
                    ex));
                return DataOperationResult<WeatherViewModel>.Failure(ErrorSource.Unknown, message, ex);
            }
        }
    }
}