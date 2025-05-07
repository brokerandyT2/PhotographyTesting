using Locations.Core.Shared;
using Locations.Core.Shared.StorageSvc;
using SQLite;
namespace Locations.Core.Data.Queries
{
    public abstract class Database

    {

        public bool IsError { get; set; } = false;
        public static string DatabasePath => MagicStrings.DataBasePath;

        public static string Email = NativeStorageService.GetSetting(MagicStrings.Email);
        public static string Guid = NativeStorageService.GetSetting(MagicStrings.UniqueID);
        public static string KEY { get { return Guid + Email; } }
        public Database()
        {
            dataB = EncryptedSQLite.DataEncrypted.GetAsyncConnection(KEY);
        }
        public SQLiteAsyncConnection dataB;

        public Database(ISQLiteAsyncConnection db)
        {
            this.dataB = db as SQLiteAsyncConnection;
        }
    }
}
