using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using  Locations.Core.Shared;
using  Locations.Core.Shared.ViewModels;
namespace  Locations.Core.Data.Queries
{
    public class TipQuery<T> : QueryBase<T> where T : TipViewModel, new()
    {
        public override T GetItem<T>(int id)
        {
            return (T)Convert.ChangeType(dataB.Table<TipViewModel>().Where(x => x.ID == id).FirstOrDefaultAsync().Result,typeof(T));
        }

        public override T GetItemByString<T>(string title)
        {
            return (T)Convert.ChangeType(dataB.Table<TipViewModel>().Where(x => x.Title == title).FirstOrDefaultAsync().Result, typeof(T));
        }

        public string GetSettingByString<T1>(string name)
        {
            throw new NotImplementedException();
        }

        public override IList<T> GetItems<T>()
        {
            return (IList<T>)dataB.Table<TipViewModel>().ToListAsync().Result;
        }

        public override string GetValueByString<T>(string name)
        {
            throw new NotImplementedException();
        }
    }
}
