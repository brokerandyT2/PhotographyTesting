extern alias SQLEncrypt;
using sql = SQLEncrypt.SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Locations.Core.Shared;
using System.Runtime.CompilerServices;
using Locations.Core.Shared.StorageSvc;

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

        public DataEncrypted() {
           
        }

      
        public static sql.SQLiteAsyncConnection GetAsyncConnection()
        {
            var x = new sql.SQLiteConnectionString(MagicStrings.DataBasePathEncrypted, true, compKey);
            return new sql.SQLiteAsyncConnection(x);
        }
              
        public static sql.SQLiteConnection GetSyncConnection()
        {

            SQLitePCL.Batteries_V2.Init();
            var x = new sql.SQLiteConnectionString(MagicStrings.DataBasePathEncrypted, true, key: compKey);
            var conn = new sql.SQLiteConnection(x);
            return conn;
        }
       
    }
}
