using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locations.Core.Shared
{
    public static class Constants
    {
#if PHOTOGRAPHY
        public const string DatabaseFilename = "photography.db3";
#endif

        public const SQLite.SQLiteOpenFlags Flags =
            // open the database in read/write mode
            SQLite.SQLiteOpenFlags.ReadWrite |
            // create the database if it doesn't exist
            SQLite.SQLiteOpenFlags.Create |
            // enable multi-threaded database access
            SQLite.SQLiteOpenFlags.SharedCache;
#if PHOTOGRAPHY
        public static string FullDatabasePath = Path.Combine(FileSystem.AppDataDirectory, DatabaseFilename);
#endif

    }
}
