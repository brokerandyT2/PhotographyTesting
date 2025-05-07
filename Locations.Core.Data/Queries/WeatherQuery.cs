using EncryptedSQLite;
using Locations.Core.Shared.ViewModels;


namespace Locations.Core.Data.Queries
{
    public class WeatherQuery<T> : QueryBase<T> where T : WeatherViewModel, new()
    {
        SettingsQuery<SettingViewModel> settings;
        public WeatherQuery() : base()
        {
            settings = new SettingsQuery<SettingViewModel>();
            
            dataB = DataEncrypted.GetAsyncConnection(KEY);
        }
       

        public override T GetItem<T>(int id)
        {
            return (T)Convert.ChangeType(dataB.Table<WeatherViewModel>().Where(x => x.ID == id).FirstOrDefaultAsync().Result, typeof(T));
        }

        public T GetWeather(double lat, double lon)
        {

            return (T)dataB.Table<WeatherViewModel>().Where(x => x.Latitude == lat && x.Longitude == lon).FirstOrDefaultAsync().Result;
        }
        /// <summary>
        /// Returns a NEW ViewModel if the weather does not exist.
        /// </summary>
        /// <param name="lat"></param>
        /// <param name="lon"></param>
        /// <returns></returns>
        public T GetWeatherDefaultViewModel(double lat, double lon)
        {
            var x = GetWeather(lat, lon);
            if (x == null)
            {
                return new T();
            }
            else
            {
                return x;
            }
        }

        public string GetItemValueByString<T>(string name)
        {
            throw new NotImplementedException();
        }

        public override IList<T> GetItems<T>()
        {
            return (IList<T>)dataB.Table<WeatherViewModel>().ToListAsync().Result;
        }

        public override string GetValueByString<T>(string name)
        {
            throw new NotImplementedException();
        }

        public override T1 GetItemByString<T1>(string name)
        {
            throw new NotImplementedException();
        }
    }
}
