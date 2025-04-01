using Locations.Core.Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locations.Core.Shared.ViewModels.Interface
{
    public interface ILocationList
    {
        public List<LocationViewModel> locations { get; }
    }
}
