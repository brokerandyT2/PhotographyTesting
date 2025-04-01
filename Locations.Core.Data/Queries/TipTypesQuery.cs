using Locations.Core.Shared.ViewModels;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using Microsoft.Data.Sqlite;
using Locations.Core.Shared;

namespace Locations.Core.Data.Queries
{
    public class TipTypesQuery<T> : QueryBase<T> where T : TipTypeViewModel, new()
    {
        public override T GetItem<T>(int id)
        {
            return (T)Convert.ChangeType(dataB.Table<TipTypeViewModel>().Where(x => x.Id == id), typeof(T));
        }
       
        public override T GetItemByString<T>(string name)
        {
            throw new NotImplementedException();
        }

        public override IList<T> GetItems<T>()
        {
            return (IList<T>)dataB.Table<TipTypeViewModel>().Where(x => x == x).ToListAsync().Result;
        }

        public override string GetValueByString<T>(string name)
        {
            throw new NotImplementedException();
        }
    }
}
