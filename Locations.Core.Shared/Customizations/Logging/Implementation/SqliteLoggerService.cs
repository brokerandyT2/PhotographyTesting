using Locations.Core.Shared.Customizations.Logging.Interfaces;
using Locations.Core.Shared.ViewModels;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locations.Core.Shared.Customizations.Logging.Implementation
{
    public class SqliteLoggerService : ILoggerService
    {
        private readonly SQLiteAsyncConnection _db;

        public SqliteLoggerService()
        {
            var dbPath = Path.Combine(MagicStrings.DataBasePath);
            _db = new SQLiteAsyncConnection(dbPath);
            _db.CreateTableAsync<Log>().Wait();
        }

        public void Log(string level, string message, Exception ex = null)
        {
            var entry = new Log
            {
                Timestamp = DateTime.UtcNow,
                Level = level,
                Message = message,
                Exception = ex?.ToString()
            };
            _db.InsertAsync(entry);
        }

        public void LogInformation(string message) => Log("Information", message);
        public void LogWarning(string message) => Log("Warning", message);
        public void LogError(string message, Exception ex = null) => Log("Error", message, ex);
        public void LogDebug(string message) => Log("Debug", message);
        public void LogTrace(string message) => Log("Trace", message);
    }
}