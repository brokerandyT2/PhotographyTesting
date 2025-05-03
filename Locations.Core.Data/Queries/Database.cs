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
using Locations.Core.Shared.Customizations.Alerts.Interfraces;
using Locations.Core.Shared.Customizations.Logging.Interfaces;
namespace  Locations.Core.Data.Queries
{
    public abstract class Database

    {

        private IAlertService alertServ;
        private ILoggerService loggerService;
        public static string DatabasePath => MagicStrings.DataBasePath;
        public Database(IAlertService alert, ILoggerService log):this()
        {
            alertServ = alert;
            loggerService = log;
        }
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
