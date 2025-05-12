extern alias SQLEncrypt;
using Locations.Core.Shared;
using sql = SQLEncrypt.SQLite;

namespace EncryptedSQLite
{
    public class DataEncrypted
    {
        public sql.SQLiteAsyncConnection _conn;
        internal static string Email;
        internal static string GUID;
        public static string compKey
        {
            get { return GUID + Email; }
        }
        /// <summary>
        /// Use this to use the default storage of email and guid
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private const sql.SQLiteOpenFlags Flags =
            // open the database in read/write mode
            sql.SQLiteOpenFlags.ReadWrite |
            // create the database if it doesn't exist
            sql.SQLiteOpenFlags.Create |
            // enable multi-threaded database access
            sql.SQLiteOpenFlags.SharedCache;
        public DataEncrypted() {
           
        }
        private static void CreateIfDoesntExist()
        {
            if (!(File.Exists(MagicStrings.DataBasePathEncrypted)))
            {
                var x = new sql.SQLiteConnectionString(MagicStrings.DataBasePathEncrypted, true, compKey);
            }
        }
      
        public static sql.SQLiteAsyncConnection GetAsyncConnection()
        {
            CreateIfDoesntExist();
            //var x = new sql.SQLiteConnectionString(MagicStrings.DataBasePathEncrypted, true, compKey);
            return new sql.SQLiteAsyncConnection(MagicStrings.DataBasePathEncrypted, Flags);
        }
              
        public static sql.SQLiteConnection GetSyncConnection()
        {

            
           // var x = new sql.SQLiteConnectionString(MagicStrings.DataBasePathEncrypted, true, key: compKey);
            var conn = new sql.SQLiteConnection(MagicStrings.DataBasePathEncrypted, Flags);
            return conn;
        }
       
    }
}
