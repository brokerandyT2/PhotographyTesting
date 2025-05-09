using EncryptedSQLite;
using Location.Core.Helpers.AlertService;
using Location.Core.Helpers.LoggingService;
using Locations.Core.Data.Helpers;
using Locations.Core.Data.Models;
using Locations.Core.Shared;
using SQLite;
using System;
using DataErrorEventArgs = Locations.Core.Data.Models.DataErrorEventArgs;
using ErrorSource = Locations.Core.Data.Models.ErrorSource;

namespace Locations.Core.Data.Queries
{
    /// <summary>
    /// Base class for database operations with error handling
    /// </summary>
    public abstract class Database
    {
        protected readonly IAlertService AlertService;
        protected readonly ILoggerService LoggerService;

        public event DataErrorEventHandler? ErrorOccurred;

        public static string DatabasePath => MagicStrings.DataBasePath;

        // The SQLite connection
        protected SQLiteAsyncConnection dataB;

        protected Database(IAlertService alertService, ILoggerService loggerService)
        {
            AlertService = alertService ?? throw new ArgumentNullException(nameof(alertService));
            LoggerService = loggerService ?? throw new ArgumentNullException(nameof(loggerService));

            try
            {
                // Create an encrypted connection using centralized key management
                dataB = GetAsyncEncryptedConnection();
            }
            catch (Exception ex)
            {
                string message = $"Failed to initialize database: {ex.Message}";
                LoggerService.LogError(message, ex);
                OnErrorOccurred(new DataErrorEventArgs(
                    ErrorSource.Database,
                    message,
                    ex));
                throw;
            }
        }

        /// <summary>
        /// Gets an encrypted SQLite connection
        /// </summary>
        protected SQLiteAsyncConnection GetAsyncEncryptedConnection()
        {
            try
            {
                return DataEncrypted.GetAsyncConnection();
            }
            catch (Exception ex)
            {
                string message = $"Error creating encrypted database connection: {ex.Message}";
                LoggerService.LogError(message, ex);
                OnErrorOccurred(new DataErrorEventArgs(
                    ErrorSource.Database,
                    message,
                    ex));
                throw;
            }
        }

        /// <summary>
        /// Gets a synchronous encrypted SQLite connection for operations that need to return the ID
        /// </summary>
        protected SQLiteConnection GetSyncEncryptedConnection()
        {
            try
            {
                return DataEncrypted.GetSyncConnection();
            }
            catch (Exception ex)
            {
                string message = $"Error creating sync encrypted database connection: {ex.Message}";
                LoggerService.LogError(message, ex);
                OnErrorOccurred(new DataErrorEventArgs(
                    ErrorSource.Database,
                    message,
                    ex));
                throw;
            }
        }

        /// <summary>
        /// Raises the ErrorOccurred event
        /// </summary>
        protected virtual void OnErrorOccurred(DataErrorEventArgs e)
        {
            LoggerService.LogError($"Database error: {e.Source} - {e.Message}");
            ErrorOccurred?.Invoke(this, e);
        }
    }
}