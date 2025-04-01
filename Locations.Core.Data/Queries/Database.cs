using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using Microsoft.Maui;
using Microsoft.Maui.Storage;
using  Locations.Core.Shared;
namespace  Locations.Core.Data.Queries
{
    public abstract class Database

    {
       

        public static string DatabasePath => MagicStrings.DataBasePath;
        public Database()
        {
            dataB = new SQLiteAsyncConnection(MagicStrings.DataBasePath, Constants.Flags);
        }
        public SQLiteAsyncConnection dataB;

        public Database(ISQLiteAsyncConnection db)
        {
            this.dataB = db as SQLiteAsyncConnection;
        }
    }
}
