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
    /// Repository implementation for tip operations
    /// </summary>
    public class TipRepository : RepositoryBase<TipViewModel>, ITipRepository
    {
        public TipRepository(IAlertService alertService, ILoggerService loggerService)
            : base(alertService, loggerService)
        {
        }

        /// <summary>
        /// Gets a tip by title
        /// </summary>
        public async Task<DataOperationResult<TipViewModel>> GetByTitleAsync(string title)
        {
            try
            {
                if (string.IsNullOrEmpty(title))
                {
                    return DataOperationResult<TipViewModel>.Failure(
                        ErrorSource.ModelValidation,
                        "Tip title cannot be empty");
                }

                var result = await dataB.Table<TipViewModel>()
                    .Where(x => x.Description == title)
                    .FirstOrDefaultAsync();

                if (result == null)
                {
                    return DataOperationResult<TipViewModel>.Failure(
                        ErrorSource.Database,
                        $"Tip with title '{title}' not found");
                }

                return DataOperationResult<TipViewModel>.Success(result);
            }
            catch (SQLiteException ex)
            {
                string message = $"Database error retrieving tip with title '{title}': {ex.Message}";
                LoggerService.LogError(message, ex);
                OnErrorOccurred(new DataErrorEventArgs(
                    ErrorSource.Database,
                    message,
                    ex));
                return DataOperationResult<TipViewModel>.Failure(ErrorSource.Database, message, ex);
            }
            catch (Exception ex)
            {
                string message = $"Error retrieving tip with title '{title}': {ex.Message}";
                LoggerService.LogError(message, ex);
                OnErrorOccurred(new DataErrorEventArgs(
                    ErrorSource.Unknown,
                    message,
                    ex));
                return DataOperationResult<TipViewModel>.Failure(ErrorSource.Unknown, message, ex);
            }
        }
    }
}