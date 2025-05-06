extern alias SQLEncrypt;
using sql = SQLEncrypt.SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Locations.Core.Shared;



namespace EncryptedSQLite
{
    public class DataEncrypted
    {
        public sql.SQLiteAsyncConnection _conn;

        /// <summary>
        /// NO RETURN>  THROWS ERROR
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        
        public DataEncrypted(string email)
        {
            SQLitePCL.Batteries_V2.Init();
            var x = new sql.SQLiteConnectionString(MagicStrings.DataBasePathEncrypted, true, key: email);
            _conn = new sql.SQLiteAsyncConnection(x);
        }
        public static sql.SQLiteAsyncConnection GetConnection(string email)
        {
            SQLitePCL.Batteries_V2.Init();
            var x = new sql.SQLiteConnectionString(MagicStrings.DataBasePathEncrypted, true, key: email);
            var conn = new sql.SQLiteAsyncConnection(x);
            return conn;
        }

        public static sql.SQLiteConnection GetSyncConnection(string email)
        {
            SQLitePCL.Batteries_V2.Init();
            var x = new sql.SQLiteConnectionString(MagicStrings.DataBasePathEncrypted, true, key: email);
            var conn = new sql.SQLiteConnection(x);
            return conn;
        }
    }
}
