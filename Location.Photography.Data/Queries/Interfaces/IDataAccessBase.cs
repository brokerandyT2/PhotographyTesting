using Locations.Core.Shared.ViewModels;
using Locations.Core.Shared.ViewModels.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Location.Photography.Data.Queries.Interfaces
{
    public interface IDataAccessBase
    {
     
        public T GetItem<T>(int id);
        public IList<T> GetItems<T>();

        public abstract void DeleteItem<T>(int id);
        //public void DeletItem<T>(T item) where T : ViewModelBase;
        public T GetItemByString<T>(string name);
        public string GetValueByString<T>(string name);

    }
}
