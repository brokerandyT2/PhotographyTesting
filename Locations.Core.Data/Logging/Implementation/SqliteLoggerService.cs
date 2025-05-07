using Locations.Core.Business.Logging.Interfaces;
using Locations.Core.Shared;
using Locations.Core.Shared.StorageSvc;
using Locations.Core.Shared.ViewModels;
using SQLite;

namespace Locations.Core.Business.Logging.Implementation
{
    public class SqliteLoggerService : ILoggerService
    {
        string email;
        string guid;
        public string compKey {
            get { return email + guid; }
        }
        private readonly SQLiteAsyncConnection _db;

        public SqliteLoggerService()
        {
            //var dbPath = Path.Combine(MagicStrings.DataBasePath);
             email = NativeStorageService.GetSetting(MagicStrings.Email);
             guid = NativeStorageService.GetSetting(MagicStrings.UniqueID);

            _db = EncryptedSQLite.DataEncrypted.GetAsyncConnection(compKey);
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