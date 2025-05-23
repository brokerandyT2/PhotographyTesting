﻿using Microsoft.Maui.Storage;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Location.Photography.Shared
{
    public class Constants
    {
        public const string DatabaseFilename = "photography.db3";

        public const SQLite.SQLiteOpenFlags Flags =
        // open the database in read/write mode
            SQLite.SQLiteOpenFlags.ReadWrite |
        // create the database if it doesn't exist
            SQLite.SQLiteOpenFlags.Create |
        // enable multi-threaded database access
            SQLite.SQLiteOpenFlags.SharedCache;
        public static string FullDatabasePath = Path.Combine(FileSystem.AppDataDirectory, DatabaseFilename);
    }
}
