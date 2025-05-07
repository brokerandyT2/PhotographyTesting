using Location.Photography.Business.DataAccess.Interfaces;
using Location.Photography.Data.Queries;
using Locations.Core.Business.Logging.Interfaces;
using Locations.Core.Shared;

using Locations.Core.Shared.ViewModels;
using SQLite;
namespace Location.Photography.Business.DataAccess
{
    public class LocationService : Locations.Core.Business.DataAccess.LocationsService, ILocationService<LocationViewModel>
    {
        LocationsQuery<LocationViewModel> LocationsQuery = new LocationsQuery<LocationViewModel>();
        SQLiteAsyncConnection conn;
        public LocationService(ILoggerService logger, string email) : this( )
        {
            var ss = new SettingsService();
            var x = ss.GetSettingByName(MagicStrings.Email).Value;
            if (string.IsNullOrEmpty(x))
            {
                //loggerService.LogWarning($"Email is not set.  Cannot use encrypted database. Email Address {x}");
                RaiseError(new ArgumentException("Email is not set.  Cannot use encrypted database."));
            }

        }
        public LocationService()
        {
            
           
            // DataEncrypted enc = new DataEncrypted(email);
            SettingsService ss = new SettingsService();
            var x = ss.GetSettingByName(MagicStrings.Email).Value;
            if (string.IsNullOrEmpty(x))
            {
                //loggerService.LogWarning($"Email is not set.  Cannot use encrypted database. Email Address {x}");
                RaiseError(new ArgumentException("Email is not set.  Cannot use encrypted database."));
            }

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
