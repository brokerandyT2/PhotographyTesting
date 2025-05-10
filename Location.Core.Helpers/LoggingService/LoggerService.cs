using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Location.Core.Helpers.AlertService;
using SQLite;


namespace Location.Core.Helpers.LoggingService
{
    /// <summary>
    /// Service for logging application events to the database
    /// </summary>
    public class LoggerService : ILoggerService
    {
        private readonly ISQLiteAsyncConnection _database;
        private readonly IAlertService? _alertService;

        /// <summary>
        /// Creates a new logger service with optional alert service for critical errors
        /// </summary>
        /// <param name="alertService">Optional alert service for notifying users of critical errors</param>
        public LoggerService(ISQLiteAsyncConnection conn, IAlertService? alertService = null)
        {
            _alertService = alertService;
            _database = conn;


        }

        /// <summary>
        /// Initializes the database for logging
        /// </summary>
        private async Task InitializeDatabaseAsync()
        {
            try
            {
                await _database.CreateTableAsync<LogModel>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error initializing logging database: {ex.Message}");
                _alertService?.ShowError("Failed to initialize logging system");
            }
        }

        /// <summary>
        /// Logs an information message
        /// </summary>
        public void LogInfo(string message)
        {
            if (string.IsNullOrEmpty(message)) return;

            Task.Run(async () =>
            {
                try
                {
                    var log = new LogModel(LogLevel.Info, message);
                    await _database.InsertAsync(log);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Failed to log info: {ex.Message}");
                }
            });
        }

        /// <summary>
        /// Logs a warning message
        /// </summary>
        public void LogWarning(string message, Exception? exception = null)
        {
            if (string.IsNullOrEmpty(message)) return;

            Task.Run(async () =>
            {
                try
                {
                    var log = new LogModel(LogLevel.Warning, message, exception?.ToString() ?? string.Empty);
                    await _database.InsertAsync(log);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Failed to log warning: {ex.Message}");
                }
            });
        }

        /// <summary>
        /// Logs an error message
        /// </summary>
        public void LogError(string message, Exception? exception = null)
        {
            if (string.IsNullOrEmpty(message)) return;

            Task.Run(async () =>
            {
                try
                {
                    var log = new LogModel(LogLevel.Error, message, exception?.ToString() ?? string.Empty);
                    await _database.InsertAsync(log);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Failed to log error: {ex.Message}");
                }
            });
        }

        /// <summary>
        /// Logs a debug message
        /// </summary>
        public void LogDebug(string message, Exception? exception = null)
        {
            if (string.IsNullOrEmpty(message)) return;

            Task.Run(async () =>
            {
                try
                {
                    var log = new LogModel(LogLevel.Debug, message, exception?.ToString() ?? string.Empty);
                    await _database.InsertAsync(log);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Failed to log debug info: {ex.Message}");
                }
            });
        }

        /// <summary>
        /// Gets logs matching the specified criteria
        /// </summary>
        public async Task<IEnumerable<LogModel>> GetLogsAsync(
            string? level = null,
            DateTime? startDate = null,
            DateTime? endDate = null,
            int? limit = null)
        {
            try
            {
                var query = _database.Table<LogModel>();

                // Apply filters
                if (!string.IsNullOrEmpty(level))
                {
                    query = query.Where(l => l.Level == level);
                }

                if (startDate.HasValue)
                {
                    query = query.Where(l => l.Timestamp >= startDate.Value);
                }

                if (endDate.HasValue)
                {
                    query = query.Where(l => l.Timestamp <= endDate.Value);
                }

                // Order by timestamp (most recent first)
                query = query.OrderByDescending(l => l.Timestamp);

                // Apply limit if specified
                if (limit.HasValue && limit.Value > 0)
                {
                    query = query.Take(limit.Value);
                }

                return await query.ToListAsync();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error retrieving logs: {ex.Message}");
                return new List<LogModel>();
            }
        }

        /// <summary>
        /// Clears all logs from the database
        /// </summary>
        public async Task ClearLogsAsync()
        {
            try
            {
                await _database.DeleteAllAsync<LogModel>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error clearing logs: {ex.Message}");
                _alertService?.ShowError("Failed to clear logs");
            }
        }
    }
}