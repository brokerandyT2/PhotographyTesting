using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Location.Photography.Shared;
using Microsoft.Maui.Storage;
namespace Location.Photography.Data.Queries
{
    public abstract class Database

    {


        public static string DatabasePath => Path.Combine(FileSystem.AppDataDirectory, Constants.DatabaseFilename);
        public Database()
        {
            dataB = new SQLiteAsyncConnection(Constants.DatabaseFilename, Constants.Flags);
        }
        public ISQLiteAsyncConnection dataB;

        public Database(ISQLiteAsyncConnection db)
        {
            this.dataB = db;
        }
    }
}
