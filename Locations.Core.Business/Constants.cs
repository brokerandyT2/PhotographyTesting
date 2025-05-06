using Locations.Core.Business.DataAccess;
using Locations.Core.Shared.Enums;
using Locations.Core.Shared.Helpers;
using Locations.Core.Shared.ViewModels;
using Locations.Core.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Locations.Core.Business
{
    public static class Constants
    {
#if PHOTOGRAPHY
        private static string DatabaseFilename = "photography.db3";
#endif
        public static string DatabasePath => Path.Combine(FileSystem.AppDataDirectory, DatabaseFilename);

        public const SQLite.SQLiteOpenFlags Flags =
            // open the database in read/write mode
            SQLite.SQLiteOpenFlags.ReadWrite |
            // create the database if it doesn't exist
            SQLite.SQLiteOpenFlags.Create |
            // enable multi-threaded database access
            SQLite.SQLiteOpenFlags.SharedCache;

    }
}
