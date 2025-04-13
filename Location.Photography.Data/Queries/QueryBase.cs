using Location.Photography.Data.Queries.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Location.Photography.Data.Queries
{
    public abstract class QueryBase<T> : Database, IDataAccessBase where T : new()
    {

        public abstract T GetItem<T>(int id);

        public abstract string GetValueByString<T>(string name);

        public abstract IList<T> GetItems<T>();
        public T GetItemByString<T>(string name) {
            throw new NotImplementedException();
        }
        public int DeleteItem<T>(T item) where T : new()
        {
            try
            {

                return dataB.DeleteAsync(item).Result;
            }
            catch
            {
                return 420;
            }
        }


        public void DeleteItem<T>(int id)
        {
            var x = GetItem<T>(id);
            var y = dataB.DeleteAsync(x).Result;
        }
        public void SaveItem(T item)
        {
            dataB.InsertOrReplaceAsync(item);
        }
        public void Update(T item)
        {
            SaveItem(item);
        }
    }
}
