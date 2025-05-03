using Location.Photography.Business.DataAccess.Interfaces;
using Locations.Core.Shared.Customizations.Alerts.Interfraces;
using Locations.Core.Shared.Customizations.Logging.Interfaces;
using SQLite;

namespace Location.Photography.Business.DataAccess
{
    public class DataAccess : IDataAccess
    {
        public ISQLiteConnection database { get { return new SQLiteConnection(Constants.FullDatabasePath, Constants.Flags); }}

        ISQLiteConnection IBaseData.database { get => database; set => throw new NotImplementedException(); }

        public SQLiteConnection DatabaseConnection => throw new NotImplementedException();
        private IAlertService alertServ;
        private ILoggerService loggerService;
        public DataAccess() { }
        public DataAccess(IAlertService alertSer, ILoggerService log)
        {
            alertServ = alertSer;
            loggerService = log;
        }
    }
}
