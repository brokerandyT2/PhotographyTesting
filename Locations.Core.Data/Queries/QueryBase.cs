using EncryptedSQLite;
using Locations.Core.Data.Queries.Interfaces;

namespace Locations.Core.Data.Queries
{
    public abstract class QueryBase<T> : Database, IDataAccessBase where T : new()
    {


        public bool IsError;
        public QueryBase() : base()
        {


        }


        public abstract T GetItem<T>(int id);

        public abstract string GetValueByString<T>(string name);

        public abstract IList<T> GetItems<T>();
        public abstract T GetItemByString<T>(string name);
        public int DeleteItem<T>(T item) where T : new()
        {
            try
            {
                var x = dataB.DeleteAsync(item).Result;
                if (x != 0)
                {
                    IsError = true;
                    return 69;
                }
                else
                {
                    IsError = false;
                    return x;
                }
            }
            catch
            {
                return 420;
            }
        }


        public void DeleteItem<T>(int id)
        {
            try
            {
                var x = GetItem<T>(id);
                var y = dataB.DeleteAsync(x).Result;
            }
            catch (Exception ex)
            {
                IsError = true;


            }
        }
        public void SaveItem(T item)
        {
            try
            {
                var x = dataB.InsertOrReplaceAsync(item).Result;
            }
            catch (Exception ex)
            {
                IsError = true;
             

            }
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
            var conn = DataEncrypted.GetSyncConnection(KEY);
            

            try
            {
                conn.Insert(item);
            }
            catch (Exception ex)
            {
                IsError = true;
               

            }
            return (T)Convert.ChangeType(item, typeof(T));
        }

    }
}
