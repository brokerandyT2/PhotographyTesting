extern alias NormalSQLite;
using sql = NormalSQLite::SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Locations.Core.Shared;

namespace NormalSQLite
{
    public class DataUnEncrypted
    {
        public sql.SQLiteAsyncConnection _conn;

        /// <summary>
        /// NO RETURN>  THROWS ERROR
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>

        public DataUnEncrypted()
        {
            SQLitePCL.Batteries_V2.Init();
           
            _conn = new sql.SQLiteAsyncConnection(MagicStrings.DataBasePath, Constants.Flags);
        }
        public static sql.SQLiteAsyncConnection GetConnection()
        {
            SQLitePCL.Batteries_V2.Init();

            var conn = new sql.SQLiteAsyncConnection(MagicStrings.DataBasePath, Constants.Flags);
            return conn;
        }
    }
}
