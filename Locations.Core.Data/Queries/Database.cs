using Locations.Core.Shared;
using Locations.Core.Shared.Alerts.Implementation;
using Locations.Core.Shared.StorageSvc;
using SQLite;
namespace Locations.Core.Data.Queries
{
    public abstract class Database

    {
        public event EventHandler<AlertEventArgs> RaiseAlert;
        public AlertEventArgs alertEventArgs;
        public bool IsError { get; set; } = false;
        public static string DatabasePath => MagicStrings.DataBasePath;

        public static string Email = NativeStorageService.GetSetting(MagicStrings.Email);
        public static string Guid = NativeStorageService.GetSetting(MagicStrings.UniqueID);
        public static string KEY { get { return Guid + Email; } }
        public Database()
        {
            this.RaiseAlert += OnAlertRaised;
            dataB = EncryptedSQLite.DataEncrypted.GetAsyncConnection(KEY);
        }

        private void OnAlertRaised(object? sender, AlertEventArgs e)
        {
            IsError = true;
            RaiseAlert?.Invoke(this, e);
        }

        public SQLiteAsyncConnection dataB;

        public Database(ISQLiteAsyncConnection db)
        {
            this.dataB = db as SQLiteAsyncConnection;
        }
    }
}
