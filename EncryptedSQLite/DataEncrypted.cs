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
        /// NO RETURN>  THROWS ERROR
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>

        public DataEncrypted(string email)
        {
            throw new NotImplementedException();
            SQLitePCL.Batteries_V2.Init();
            var x = new sql.SQLiteConnectionString(MagicStrings.DataBasePathEncrypted, true, key: email);
            _conn = new sql.SQLiteAsyncConnection(x);
        }
        public static sql.SQLiteAsyncConnection GetAsyncConnection(string compositeKey)
        {
            var x = new sql.SQLiteConnectionString(MagicStrings.DataBasePathEncrypted, true, compositeKey);
            return new sql.SQLiteAsyncConnection(x);
        }
        public static sql.SQLiteConnection GetConnection(string compositeKey)
        {
            var x = new sql.SQLiteConnectionString(MagicStrings.DataBasePathEncrypted, true, compositeKey);
            return new sql.SQLiteConnection(x);
        }
        public static sql.SQLiteAsyncConnection GetConnection(string email, string GUID)
        {
            Email = email;
            GUID = GUID;
            SQLitePCL.Batteries_V2.Init();
            var x = new sql.SQLiteConnectionString(MagicStrings.DataBasePathEncrypted, true, key: compKey);
            var conn = new sql.SQLiteAsyncConnection(x);
            return conn;
        }

        public static sql.SQLiteConnection GetSyncConnection(string email, string GUID)
        {
            Email = email;
            GUID = GUID;
            SQLitePCL.Batteries_V2.Init();
            var x = new sql.SQLiteConnectionString(MagicStrings.DataBasePathEncrypted, true, key: compKey);
            var conn = new sql.SQLiteConnection(x);
            return conn;
        }
        public static sql.SQLiteConnection GetSyncConnection(string key)
        {

            SQLitePCL.Batteries_V2.Init();
            var x = new sql.SQLiteConnectionString(MagicStrings.DataBasePathEncrypted, true, key);
            var conn = new sql.SQLiteConnection(x);
            return conn;
        }
    }
}
