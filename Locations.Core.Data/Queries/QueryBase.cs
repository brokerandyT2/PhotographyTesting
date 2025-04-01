using  Locations.Core.Data.Queries.Interfaces;
using Locations.Core.Shared;
using  Locations.Core.Shared.ViewModels;
using  Locations.Core.Shared.ViewModels.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace  Locations.Core.Data.Queries
{
    public abstract class QueryBase<T> : Database, IDataAccessBase where T : new()
    {
      
        public abstract T GetItem<T>(int id);

        public abstract string GetValueByString<T>(string name);

        public abstract IList<T> GetItems<T>();
        public abstract T GetItemByString<T>(string name);
        public int DeleteItem<T>(T item) where T : new()
        {
            try
            {
                
                return dataB.DeleteAsync(item).Result; 
            }
            catch {
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
           var x = dataB.InsertOrReplaceAsync(item).Result;
        }
        public void Update(T item)
        {
            SaveItem(item);
        }
        /// <summary>
        /// Inserts the object and returns the entire object with the primary key populated.  This does not use the SQLite ASYNCRONOUS, but rather instantiates a SYNCRONOUS SQLlite connetion.  Only use when return ID is needed for things like FK relationships for insertion.
        /// </summary>
        /// <param name="item">Must be of type ViewModel</param>
        /// <returns></returns>
        public T SaveWithIDReturn(T item)
        {
            SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(Constants.FullDatabasePath, Constants.Flags);
            
            conn.Insert(item);
            //var x = dataB.InsertOrReplaceAsync(item).Result;

            return (T)Convert.ChangeType(item, typeof(T));
        }

    }
}
