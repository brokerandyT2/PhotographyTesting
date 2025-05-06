using Location.Photography.Business.DataAccess.Interfaces;
using Location.Photography.Data.Queries;
using Locations.Core.Shared.Customizations.Alerts.Interfraces;
using Locations.Core.Shared.Customizations.Logging.Interfaces;
using Locations.Core.Shared.ViewModels;
using EncryptedSQLite;
using Serilog.Core;
using Locations.Core.Shared.Customizations.Logging.Implementation;
using Locations.Core.Shared.Customizations.Alerts.Implementation;
using Locations.Core.Shared;
namespace Location.Photography.Business.DataAccess
{
    public class LocationService : Locations.Core.Business.DataAccess.LocationsService, ILocationService<LocationViewModel>
    {
        LocationsQuery<LocationViewModel> LocationsQuery = new LocationsQuery<LocationViewModel>();
        private IAlertService alertServ;
        private ILoggerService loggerService;
        public LocationService(ILoggerService logger, IAlertService alert)
        {
            alertServ = alert;
            loggerService = logger;
        }
        public LocationService(ILoggerService logger, IAlertService alert, string email) : this( logger, alert)
        {
            var ss = new SettingsService(alert, logger);
            var x = ss.GetSettingByName(MagicStrings.Email).Value;
            if (string.IsNullOrEmpty(x))
            {
                loggerService.LogWarning($"Email is not set.  Cannot use encrypted database. Email Address {x}");
                RaiseError(new ArgumentException("Email is not set.  Cannot use encrypted database."));
            }
            LocationsQuery.dataB = DataEncrypted.GetConnection(email);

        }
        public LocationService(string email)
        {
            alertServ = new AlertService();
            loggerService = new LoggerService();
            // DataEncrypted enc = new DataEncrypted(email);
            SettingsService ss = new SettingsService();
            var x = ss.GetSettingByName(MagicStrings.Email).Value;
            if (string.IsNullOrEmpty(x))
            {
                loggerService.LogWarning($"Email is not set.  Cannot use encrypted database. Email Address {x}");
                RaiseError(new ArgumentException("Email is not set.  Cannot use encrypted database."));
            }

            LocationsQuery.dataB = DataEncrypted.GetConnection(email);
        }
        public bool Delete(LocationViewModel model)
        {
            throw new NotImplementedException();

        }

        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public bool Delete(double latitude, double longitude)
        {
            throw new NotImplementedException();
        }

        public LocationViewModel Get(int id)
        {
            throw new NotImplementedException();
        }

        public LocationViewModel Save(LocationViewModel model)
        {
            throw new NotImplementedException();
        }

        public LocationViewModel Save(LocationViewModel model, bool returnNew)
        {
            throw new NotImplementedException();
        }
    }
}
