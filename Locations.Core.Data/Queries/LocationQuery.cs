using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using Locations.Core.Shared.Customizations.Alerts.Implementation;
using Locations.Core.Shared.Customizations.Alerts.Interfraces;
using Locations.Core.Shared.Customizations.Logging.Implementation;
using Locations.Core.Shared.Customizations.Logging.Interfaces;
using  Locations.Core.Shared.DTO;
using  Locations.Core.Shared.ViewModels;
using  Locations.Core.Shared.ViewModels.Interface;
using Microsoft.Maui.Platform;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace  Locations.Core.Data.Queries
{
    public class LocationQuery<T> : QueryBase<T> where T : LocationViewModel, new()
    {
        
        public LocationQuery(IAlertService alertServ, ILoggerService loggerService) : base(alertServ, loggerService)
        {

        }
       
        public T GetItem<T>(double latitude, double longitude)
            {
            return (T)Convert.ChangeType(dataB.Table<LocationViewModel>().Where(x => x.Lattitude == latitude && x.Longitude == longitude).FirstOrDefaultAsync().Result, typeof(T));
        }
        public override T GetItem<T>(int id)
        {
            return (T)Convert.ChangeType(dataB.Table<LocationViewModel>().Where( x => x.Id == id).FirstOrDefaultAsync().Result, typeof(T));
        }

        public override string GetValueByString<T>(string name)
        {
            return string.Empty;
        }

        public override IList<T> GetItems<T>()
        {
            IList<T> results = (IList<T>)dataB.Table<LocationViewModel>().Where(x => x.Id == x.Id).ToListAsync().Result;
            return results;
        }

        public override T GetItemByString<T>(string name)
        {
            throw new NotImplementedException();
        }
    }
}
