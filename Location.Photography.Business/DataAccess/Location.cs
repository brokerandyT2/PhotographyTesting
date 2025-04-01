using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Locations.Core.Business;
namespace Location.Photography.Business.DataAccess
{
#if PHOTOGRAPHY
    public class Location : Locations.Core.Business.DataAccess.LocationsService
#else
    public class Location : Locations.Core.Business.DataAccess.LocationsService
#endif
    { 
    
    }
}
