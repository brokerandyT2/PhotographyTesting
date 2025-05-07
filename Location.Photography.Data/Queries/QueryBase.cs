using Location.Photography.Data.Queries.Interfaces;

namespace Location.Photography.Data.Queries
{
    public abstract class QueryBase<T> : Database, IDataAccessBase where T : new()
    {

        public abstract T GetItem<T>(int id);

        public abstract string GetValueByString<T>(string name);

        public abstract IList<T> GetItems<T>();
        public T GetItemByString<T>(string name)
        {
            throw new NotImplementedException();
        }
        public int DeleteItem<T>(T item) where T : new()
        {
            try
            {

                return dataAsync.DeleteAsync(item).Result;
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
                var y = dataAsync.DeleteAsync(x).Result;
            }
            catch (Exception ex)
            {
               

            }
        }
        public void SaveItem(T item)
        {
            try
            {
                dataAsync.InsertOrReplaceAsync(item);
            }
            catch (Exception ex)
            {

            }
        }
        public void Update(T item)
        {
            try
            {
                SaveItem(item);
            }
            catch (Exception ex)
            {

            }
        }
    }
}
