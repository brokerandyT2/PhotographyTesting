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
        public T GetItemByString<T>(string name)
        {
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
            try
            {
                var x = GetItem<T>(id);
                var y = dataB.DeleteAsync(x).Result;
            }
            catch (Exception ex)
            {
                Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");

            }
        }
        public void SaveItem(T item)
        {
            try
            {
                dataB.InsertOrReplaceAsync(item);
            }
            catch (Exception ex)
            {
                Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");

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
                Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");

            }
        }
    }
}
