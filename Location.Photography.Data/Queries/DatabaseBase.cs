using Locations.Core.Shared;
using Locations.Core.Shared.Alerts.Implementation;
using Locations.Core.Shared.StorageSvc;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Location.Photography.Data.Queries
{
    public class DatabaseBase
    {
        public SQLiteAsyncConnection dataAsync { get; set; }
        public SQLiteConnection dataSync { get; set; }
        public static event EventHandler<AlertEventArgs> AlertRaised;
        AlertEventArgs alertEventArgs;
        //private IAlertService alertService;
        public static bool IsError { get; set; } = false;

        public object Connection { get; set; }
        public string KEY
        {
            get { return guid + email; }
        }
        private string email;
        private string guid;

        public DatabaseBase():base()
        {
            email = NativeStorageService.GetSetting(MagicStrings.Email);
             guid = NativeStorageService.GetSetting(MagicStrings.UniqueID);
            AlertRaised += OnAlertRaised;
            getKey();
            dataSync = EncryptedSQLite.DataEncrypted.GetSyncConnection(KEY);
            dataAsync = EncryptedSQLite.DataEncrypted.GetAsyncConnection(KEY);
        }

        private void getKey()
        {

            var email = NativeStorageService.GetSetting(MagicStrings.Email);
            var guid = NativeStorageService.GetSetting(MagicStrings.UniqueID);
            dataSync = EncryptedSQLite.DataEncrypted.GetSyncConnection(KEY);
            dataAsync = EncryptedSQLite.DataEncrypted.GetAsyncConnection(KEY);
        }


        public static void LogInfo(string message)
        {
        }

        private void OnAlertRaised(object? sender, AlertEventArgs e)
        {
            alertEventArgs = e;
            AlertRaised?.Invoke(this, e);
            IsError = true;

        }

        protected virtual void OnAlertRaised(string title, string message)
        {
            alertEventArgs = new AlertEventArgs
            {
                Title = title,
                Message = message,
                IsError = true
            };
            AlertRaised?.Invoke(this, alertEventArgs);
            IsError = true;
        }
        public static void RaiseAlert(object? sender, AlertEventArgs e)
        {
            AlertRaised?.Invoke(null, e);
            IsError = true;

        }
        public static void RaiseError(Exception ex)
        {
            AlertRaised?.Invoke(null, new AlertEventArgs("Error", ex.Message, true));
            IsError = true;
        }
    }
}
