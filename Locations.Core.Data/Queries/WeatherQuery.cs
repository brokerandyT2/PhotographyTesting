using EncryptedSQLite;
using Locations.Core.Shared;
using Locations.Core.Shared.Customizations.Alerts.Interfraces;
using Locations.Core.Shared.Customizations.Logging.Implementation;
using Locations.Core.Shared.Customizations.Logging.Interfaces;
using Locations.Core.Shared.ViewModels;
using Microsoft.Maui.ApplicationModel.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NormalSQLite;


namespace Locations.Core.Data.Queries
{
    public class WeatherQuery<T> : QueryBase<T> where T : WeatherViewModel, new()
    {
        public WeatherQuery(IAlertService alertServ, ILoggerService loggerService, string email) : this(alertServ, loggerService)
        {
            SettingsQuery<SettingViewModel> settings = new SettingsQuery<SettingViewModel>(alertServ, loggerService);
            var addy = settings.GetItemByString<SettingViewModel>(MagicStrings.Email).Value;

            if (string.IsNullOrEmpty(addy))
            {
                loggerService.LogWarning($"Email is not set.  Cannot use encrypted database. Email Address {addy}");
                throw new ArgumentException("Email is not set.  Cannot use encrypted database.");
            }
            dataB = DataEncrypted.GetConnection(email);
        }
        public WeatherQuery(IAlertService alertServ, ILoggerService loggerService) : base(alertServ, loggerService)
        {
            dataB = DataUnEncrypted.GetConnection();
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
