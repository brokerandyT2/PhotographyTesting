using Location.Photography.Business.DataAccess.Interfaces;
using Location.Photography.Data.Queries;
using Locations.Core.Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Location.Photography.Business.DataAccess
{
    public class LocationService : Locations.Core.Business.DataAccess.LocationsService, ILocationService<LocationViewModel>
    {
        LocationsQuery<LocationViewModel> LocationsQuery = new LocationsQuery<LocationViewModel>();
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
