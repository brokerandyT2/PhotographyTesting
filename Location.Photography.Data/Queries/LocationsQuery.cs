using Locations.Core.Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Location.Photography.Data.Queries
{
    public class LocationsQuery<T> : QueryBase<T> where T : LocationViewModel, new()
    {
        public override T GetItem<T>(int id)
        {
            throw new NotImplementedException();
        }

        public override IList<T> GetItems<T>()
        {
           return (IList<T>)dataB.Table<LocationViewModel>().Where(x=> x.IsDeleted == false).ToListAsync().Result;
        }

        public override string GetValueByString<T>(string name)
        {
            throw new NotImplementedException();
        }
    }
}
