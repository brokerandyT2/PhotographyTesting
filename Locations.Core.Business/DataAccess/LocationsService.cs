using Locations.Core.Business.DataAccess.Interfaces;
using Locations.Core.Business.GeoLocation;
using Locations.Core.Business.Weather;
using Locations.Core.Data.Queries;
using Locations.Core.Shared.Helpers;
using Locations.Core.Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locations.Core.Business.DataAccess
{
    public class LocationsService : ILocationService<LocationViewModel>
    {
        private LocationQuery<LocationViewModel> lq = new Data.Queries.LocationQuery<LocationViewModel>();
        public LocationsService() { }
        public LocationViewModel SaveSettingWithObjectReturn(LocationViewModel s)
        {
            return lq.SaveWithIDReturn(s);
        }
        public LocationViewModel Save(LocationViewModel locationViewModel, bool getWeather, bool returnNew)
        {
            if (getWeather)
            {
                SettingsService set = new Business.DataAccess.SettingsService();

                WeatherAPI weatherAPI = new WeatherAPI(set.GetSettingByName(Constants.Weather_API_Key_string).GetValue(),locationViewModel.Lattitude, locationViewModel.Longitude, set.GetSettingByName(Constants.WeatherURL_string).GetValue());
                WeatherService weatherService = new WeatherService();
                weatherService.Save(weatherAPI.GetWeatherAsync().Result);
            }
            lq.SaveItem(locationViewModel);
            return returnNew? new LocationViewModel(): locationViewModel;
        }
        public LocationViewModel Save(LocationViewModel locationViewModel)
        {
            GeoLocationAPI geoLocationAPI = new GeoLocationAPI(ref locationViewModel);
            geoLocationAPI.GetCityAndState(locationViewModel.Lattitude, locationViewModel.Longitude);


            lq.SaveItem(locationViewModel);
            return locationViewModel;
        }
        public LocationViewModel Save(LocationViewModel locationViewModel, bool returnNew)
        {
            Save(locationViewModel);
            return new LocationViewModel();

        }

        public LocationViewModel Get(int id){

            return lq.GetItem<LocationViewModel>(id);
        }
        public LocationViewModel GetLocation(double latitude, double longitude)
        {
            return lq.GetItem<LocationViewModel>(latitude, longitude);
        }
        public bool Delete(LocationViewModel locationViewModel)
        {
            var x = lq.DeleteItem<LocationViewModel>(locationViewModel);
            return x != 420? true: false;
        }
        public bool Delete(int id)
        {
            var y = Get(id);
            return lq.DeleteItem<LocationViewModel>(y) == 420 ? false: true ;
        }
        public bool Delete(double latitude, double longitude)
        {
            var x = lq.GetItem<LocationViewModel>(latitude, longitude);
            return Delete(x.Id);

        }
        public bool Update(LocationViewModel locationViewModel)
        {
            lq.Update(locationViewModel);
            return true;
        }
        public List<LocationViewModel> GetLocations()
        {
            return lq.GetItems<LocationViewModel>().Where(x=>x.IsDeleted == false).ToList();
        }
    }
}
