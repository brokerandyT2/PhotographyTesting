using Locations.Core.Shared.ViewModels.Interface;
using Locations.Core.Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locations.Core.Shared.ViewModels
{
    public class LocationsListViewModel : ILocationList
    {
        private List<LocationViewModel> _locationsListViewModels= new List<LocationViewModel>();
        public List<LocationViewModel> locations
        { get=>_locationsListViewModels; set { _locationsListViewModels = value; } }
        public LocationsListViewModel() { }

    }
}
