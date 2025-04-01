using Locations.Core.Business.DataAccess.Interfaces;
using Locations.Core.Business.Weather;
using Locations.Core.Data.Queries;
using Locations.Core.Shared;
using Locations.Core.Shared.ViewModels;
using Locations.Core.Shared.ViewModels.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locations.Core.Business.DataAccess
{
    public class WeatherService : IWeatherService
    {
        WeatherQuery<WeatherViewModel> _weatherQuery = new WeatherQuery<WeatherViewModel>();
        SettingsService _settingsService = new SettingsService();
        private readonly IConnectivity _connectivity;
        public WeatherService() { }
        public WeatherViewModel GetWeather(double latitude, double longitude)
        {

            var x = _weatherQuery.GetWeather(latitude, longitude);
            WeatherViewModel wvm = new WeatherViewModel();
            if (x == null)
            {
                NetworkAccess accessType = Connectivity.Current.NetworkAccess;
                if (accessType == NetworkAccess.Internet)
                {
                    WeatherAPI w = new WeatherAPI(
                        _settingsService.GetSetting(MagicStrings.Weather_API_Key).Value,
                        latitude,
                        longitude,
                        _settingsService.GetSetting(MagicStrings.WeatherURL).Value);

                    wvm = w.GetWeatherAsync().Result;
                    Save(wvm);
                }
                return wvm;
            }
            else
            {
                return x;
            }

        }
        public WeatherViewModel Save(WeatherViewModel model)
        {
            _weatherQuery.SaveItem(model);
            return model;
        }
        public WeatherViewModel Save(WeatherViewModel model, bool returnNew)
        {
            var x = Save(model);
            return returnNew ? new WeatherViewModel() : model;
        }
        public WeatherViewModel Get(int id)
        {
            return _weatherQuery.GetItem<WeatherViewModel>(id);

        }

        public bool Delete(WeatherViewModel model)
        {
            var y = _weatherQuery.DeleteItem<WeatherViewModel>(model);
            return y == 420 ? false : true;
        }
        public bool Delete(int id)
        {

            return _weatherQuery.DeleteItem<WeatherViewModel>(_weatherQuery.GetItem<WeatherViewModel>(id)) == 420 ? false : true;

        }
        public bool Delete(double latitude, double longitude)
        {
            return _weatherQuery.DeleteItem<WeatherViewModel>(_weatherQuery.GetWeather(latitude, longitude)) == 420 ? false : true;

        }


    }
}
