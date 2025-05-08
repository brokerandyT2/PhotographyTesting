using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;
using Locations.Core.Data.Models;
using Locations.Core.Shared.ViewModels;
using Location.Core.Helpers.AlertService;
using Location.Core.Helpers.LoggingService;

namespace Locations.Core.Data.Queries
{
    /// <summary>
    /// Query operations for locations
    /// </summary>
    public class LocationQuery : QueryBase<LocationViewModel>
    {
        public LocationQuery(IAlertService alertService, ILoggerService loggerService)
            : base(alertService, loggerService)
        {
        }

        /// <summary>
        /// Gets a location by ID
        /// </summary>
        public override async Task<DataOperationResult<T>> GetItemAsync<T>(int id) 
        {
            try
            {
                // Handle type checking - we expect T to be LocationViewModel or a related type
                if (!typeof(LocationViewModel).IsAssignableFrom(typeof(T)))
                {
                    return DataOperationResult<T>.Failure(
                        ErrorSource.Unknown,
                        $"Invalid type requested: {typeof(T).Name}. Expected LocationViewModel or derived type.");
                }

                var result = await dataB.Table<LocationViewModel>()
                    .Where(x => x.Id == id)
                    .FirstOrDefaultAsync();

                if (result == null)
                {
                    return DataOperationResult<T>.Failure(
                        ErrorSource.Database,
                        $"Location with ID {id} not found");
                }

                // Convert the result to the requested type
                // This works if T is LocationViewModel or a derived type
                return DataOperationResult<T>.Success((T)Convert.ChangeType(result, typeof(T)));
            }
            catch (SQLiteException ex)
            {
                string message = $"Database error retrieving location ID {id}: {ex.Message}";
                LoggerService.LogError(message, ex);
                OnErrorOccurred(new DataErrorEventArgs(
                    ErrorSource.Database,
                    message,
                    ex));
                return DataOperationResult<T>.Failure(ErrorSource.Database, message, ex);
            }
            catch (InvalidCastException ex)
            {
                string message = $"Type conversion error retrieving location ID {id}: {ex.Message}";
                LoggerService.LogError(message, ex);
                OnErrorOccurred(new DataErrorEventArgs(
                    ErrorSource.Unknown,
                    message,
                    ex));
                return DataOperationResult<T>.Failure(ErrorSource.Unknown, message, ex);
            }
            catch (Exception ex)
            {
                string message = $"Error retrieving location ID {id}: {ex.Message}";
                LoggerService.LogError(message, ex);
                OnErrorOccurred(new DataErrorEventArgs(
                    ErrorSource.Unknown,
                    message,
                    ex));
                return DataOperationResult<T>.Failure(ErrorSource.Unknown, message, ex);
            }
        }

        /// <summary>
        /// Gets a location by coordinates
        /// </summary>
        public async Task<DataOperationResult<LocationViewModel>> GetLocationAsync(double latitude, double longitude)
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

        /// <summary>
        /// This operation is not supported for LocationViewModel
        /// </summary>
        public override async Task<DataOperationResult<string>> GetValueByStringAsync<T>(string name) 
        {
            return DataOperationResult<string>.Failure(
                ErrorSource.Unknown,
                "GetValueByString operation is not supported for LocationViewModel");
        }

        /// <summary>
        /// Gets all locations
        /// </summary>
        public override async Task<DataOperationResult<IList<T>>> GetItemsAsync<T>() 
        {
            try
            {
                // Handle type checking - we expect T to be LocationViewModel or a related type
                if (!typeof(LocationViewModel).IsAssignableFrom(typeof(T)))
                {
                    return DataOperationResult<IList<T>>.Failure(
                        ErrorSource.Unknown,
                        $"Invalid type requested: {typeof(T).Name}. Expected LocationViewModel or derived type.");
                }

                var results = await dataB.Table<LocationViewModel>().ToListAsync();

                // Convert the list to the requested type
                IList<T> convertedResults = new List<T>();
                foreach (var item in results)
                {
                    convertedResults.Add((T)Convert.ChangeType(item, typeof(T)));
                }

                return DataOperationResult<IList<T>>.Success(convertedResults);
            }
            catch (SQLiteException ex)
            {
                string message = $"Database error retrieving locations: {ex.Message}";
                LoggerService.LogError(message, ex);
                OnErrorOccurred(new DataErrorEventArgs(
                    ErrorSource.Database,
                    message,
                    ex));
                return DataOperationResult<IList<T>>.Failure(ErrorSource.Database, message, ex);
            }
            catch (InvalidCastException ex)
            {
                string message = $"Type conversion error retrieving locations: {ex.Message}";
                LoggerService.LogError(message, ex);
                OnErrorOccurred(new DataErrorEventArgs(
                    ErrorSource.Unknown,
                    message,
                    ex));
                return DataOperationResult<IList<T>>.Failure(ErrorSource.Unknown, message, ex);
            }
            catch (Exception ex)
            {
                string message = $"Error retrieving locations: {ex.Message}";
                LoggerService.LogError(message, ex);
                OnErrorOccurred(new DataErrorEventArgs(
                    ErrorSource.Unknown,
                    message,
                    ex));
                return DataOperationResult<IList<T>>.Failure(ErrorSource.Unknown, message, ex);
            }
        }

        /// <summary>
        /// This operation is not supported for LocationViewModel
        /// </summary>
        public override async Task<DataOperationResult<T>> GetItemByStringAsync<T>(string name) 
        {
            return DataOperationResult<T>.Failure(
                ErrorSource.Unknown,
                "GetItemByString operation is not supported for LocationViewModel");
        }
    }
}