using Location.Core.Helpers.LoggingService;
using Locations.Core.Shared.ViewModels;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Locations.Core.Business.Services
{
    /// <summary>
    /// Implementation of the ILoggerService interface that logs to a SQLite database
    /// </summary>
    public class LoggerService : ILoggerService
    {
        private readonly SQLiteConnection _database;
        private readonly object _lockObject = new object();
        private readonly string _loggerDbPath;

        /// <summary>
        /// Initializes a new instance of the LoggerService class with the default database path
        /// </summary>
        public LoggerService()
            : this(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "logger.db"))
        {
        }

        /// <summary>
        /// Initializes a new instance of the LoggerService class with a custom database path
        /// </summary>
        /// <param name="dbPath">The path to the SQLite database file</param>
        public LoggerService(string dbPath)
        {
            _loggerDbPath = dbPath;

            try
            {
                _database = new SQLiteConnection(_loggerDbPath);
                _database.CreateTable<Log>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error initializing logger database: {ex.Message}");
                // In a real app, we might try to log to a fallback location or use a different approach
            }
        }

        /// <summary>
        /// Logs a debug message
        /// </summary>
        /// <param name="message">The message to log</param>
        public void LogDebug(string message)
        {
            WriteLog("Debug", message, null);
        }

        /// <summary>
        /// Logs an informational message
        /// </summary>
        /// <param name="message">The message to log</param>
        public void LogInformation(string message)
        {
            WriteLog("Info", message, null);
        }

        /// <summary>
        /// Logs a warning message
        /// </summary>
        /// <param name="message">The message to log</param>
        public void LogWarning(string message)
        {
            WriteLog("Warning", message, null);
        }

        /// <summary>
        /// Logs an error message
        /// </summary>
        /// <param name="message">The error message to log</param>
        public void LogError(string message)
        {
            WriteLog("Error", message, null);
        }

        /// <summary>
        /// Logs an error message with exception details
        /// </summary>
        /// <param name="message">The error message</param>
        /// <param name="exception">The exception to log</param>
        public void LogError(string message, Exception exception)
        {
            WriteLog("Error", message, exception);
        }

        /// <summary>
        /// Logs a critical error message
        /// </summary>
        /// <param name="message">The critical error message to log</param>
        public void LogCritical(string message)
        {
            WriteLog("Critical", message, null);
        }

        /// <summary>
        /// Logs a critical error message with exception details
        /// </summary>
        /// <param name="message">The critical error message</param>
        /// <param name="exception">The exception to log</param>
        public void LogCritical(string message, Exception exception)
        {
            WriteLog("Critical", message, exception);
        }

        /// <summary>
        /// Filters logs by severity level
        /// </summary>
        /// <param name="level">The log level to filter by</param>
        /// <param name="pageSize">Maximum number of logs to return</param>
        /// <param name="pageNumber">Page number for pagination</param>
        /// <returns>Collection of log entries matching the criteria</returns>
        public IEnumerable<object> GetLogsByLevel(string level, int pageSize = 100, int pageNumber = 1)
        {
            try
            {
                lock (_lockObject)
                {
                    if (_database == null) return Enumerable.Empty<Log>();

                    int skip = (pageNumber - 1) * pageSize;
                    return _database.Table<Log>()
                        .Where(l => l.Level == level)
                        .OrderByDescending(l => l.Timestamp)
                        .Skip(skip)
                        .Take(pageSize)
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving logs by level: {ex.Message}");
                return Enumerable.Empty<Log>();
            }
        }

        /// <summary>
        /// Filters logs by date range
        /// </summary>
        /// <param name="startDate">The start date (inclusive)</param>
        /// <param name="endDate">The end date (inclusive)</param>
        /// <param name="pageSize">Maximum number of logs to return</param>
        /// <param name="pageNumber">Page number for pagination</param>
        /// <returns>Collection of log entries matching the criteria</returns>
        public IEnumerable<object> GetLogsByDateRange(DateTime startDate, DateTime endDate, int pageSize = 100, int pageNumber = 1)
        {
            try
            {
                lock (_lockObject)
                {
                    if (_database == null) return Enumerable.Empty<Log>();

                    int skip = (pageNumber - 1) * pageSize;
                    return _database.Table<Log>()
                        .Where(l => l.Timestamp >= startDate && l.Timestamp <= endDate)
                        .OrderByDescending(l => l.Timestamp)
                        .Skip(skip)
                        .Take(pageSize)
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving logs by date range: {ex.Message}");
                return Enumerable.Empty<Log>();
            }
        }

        /// <summary>
        /// Searches logs by message content
        /// </summary>
        /// <param name="searchText">Text to search for in log messages</param>
        /// <param name="pageSize">Maximum number of logs to return</param>
        /// <param name="pageNumber">Page number for pagination</param>
        /// <returns>Collection of log entries matching the criteria</returns>
        public IEnumerable<object> SearchLogs(string searchText, int pageSize = 100, int pageNumber = 1)
        {
            try
            {
                lock (_lockObject)
                {
                    if (_database == null) return Enumerable.Empty<Log>();
                    if (string.IsNullOrWhiteSpace(searchText)) return GetLogs(pageSize, pageNumber);

                    int skip = (pageNumber - 1) * pageSize;
                    return _database.Table<Log>()
                        .Where(l => l.Message.Contains(searchText) || l.Exception.Contains(searchText))
                        .OrderByDescending(l => l.Timestamp)
                        .Skip(skip)
                        .Take(pageSize)
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error searching logs: {ex.Message}");
                return Enumerable.Empty<Log>();
            }
        }

        /// <summary>
        /// Retrieves logs with pagination
        /// </summary>
        /// <param name="pageSize">Maximum number of logs to return</param>
        /// <param name="pageNumber">Page number for pagination</param>
        /// <returns>Collection of log entries for the specified page</returns>
        public IEnumerable<object> GetLogs(int pageSize = 100, int pageNumber = 1)
        {
            try
            {
                lock (_lockObject)
                {
                    if (_database == null) return Enumerable.Empty<Log>();

                    int skip = (pageNumber - 1) * pageSize;
                    return _database.Table<Log>()
                        .OrderByDescending(l => l.Timestamp)
                        .Skip(skip)
                        .Take(pageSize)
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving logs: {ex.Message}");
                return Enumerable.Empty<Log>();
            }
        }

        /// <summary>
        /// Clears logs older than the specified date
        /// </summary>
        /// <param name="olderThan">Date threshold for deletion</param>
        /// <returns>Number of logs deleted</returns>
        public int ClearLogsOlderThan(DateTime olderThan)
        {
            try
            {
                lock (_lockObject)
                {
                    if (_database == null) return 0;

                    return _database.Execute("DELETE FROM Log WHERE Timestamp < ?", olderThan);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error clearing old logs: {ex.Message}");
                return 0;
            }
        }

        /// <summary>
        /// Gets the total count of log entries
        /// </summary>
        /// <returns>Total number of log entries</returns>
        public int GetLogCount()
        {
            try
            {
                lock (_lockObject)
                {
                    if (_database == null) return 0;

                    return _database.Table<Log>().Count();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting log count: {ex.Message}");
                return 0;
            }
        }

        /// <summary>
        /// Gets the count of log entries with the specified level
        /// </summary>
        /// <param name="level">Log level to count</param>
        /// <returns>Number of log entries with the specified level</returns>
        public int GetLogCountByLevel(string level)
        {
            try
            {
                lock (_lockObject)
                {
                    if (_database == null) return 0;

                    return _database.Table<Log>().Count(l => l.Level == level);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting log count by level: {ex.Message}");
                return 0;
            }
        }

        /// <summary>
        /// Writes a log entry to the database
        /// </summary>
        /// <param name="level">Log level</param>
        /// <param name="message">Log message</param>
        /// <param name="exception">Exception (optional)</param>
        private void WriteLog(string level, string message, Exception exception)
        {
            try
            {
                var log = new Log
                {
                    Timestamp = DateTime.Now,
                    Level = level,
                    Message = message ?? string.Empty,
                    Exception = exception?.ToString() ?? string.Empty
                };

                lock (_lockObject)
                {
                    if (_database == null) return;

                    _database.Insert(log);
                }
            }
            catch (Exception ex)
            {
                // In a production environment, we might want a fallback mechanism
                Console.WriteLine($"Error writing to log: {ex.Message}");
            }
        }
    }
}