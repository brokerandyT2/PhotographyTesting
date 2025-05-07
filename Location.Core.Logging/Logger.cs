using Locations.Core.Shared;
using Locations.Core.Shared.StorageSvc;
using EncryptedSQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using Locations.Core.Shared.ViewModels;

namespace Location.Core.Logging
{
    public class Logger : ILogger
    {
        static SQLiteAsyncConnection conn;
        public void LogWarning(string message, Exception ex) => LogGenericError("Warning", message, ex);
        public void LogInfo(string message, Exception ex) => LogGenericError("Info", message, ex);
        public void LogError(string message, Exception ex) => LogGenericError("Error", message, ex);
        public void LogWarning(string message) => LogGenericError("Warning",  message, null);
        public void LogInfo(string message) => LogGenericError("Info", message, null);
        public void LogError(string message)=> LogGenericError("Error", message, null);
        private void LogGenericError(string type, string message, Exception? ex)
        {
            var KEY = NativeStorageService.GetSetting(MagicStrings.UniqueID) + NativeStorageService.GetSetting(MagicStrings.UniqueID);
            conn = EncryptedSQLite.DataEncrypted.GetAsyncConnection(KEY);
            Locations.Core.Shared.ViewModels.Log log = new Locations.Core.Shared.ViewModels.Log();
            log.Level = type;
            log.Message = message;
            log.Exception= ex != null? ex.Message : string.Empty;
            conn.InsertAsync(log);
        }

        void ILogger.LogGenericError(string type, string message, Exception? ex)
        {
            LogGenericError(type, message, ex);
        }
    }
}
