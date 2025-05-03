using Location.Photography.Business.DataAccess.Interfaces;
using Location.Photography.Data.Queries;
using Locations.Core.Shared.Customizations.Alerts.Interfraces;
using Locations.Core.Shared.Customizations.Logging.Interfaces;
using Locations.Core.Shared.ViewModels;

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
