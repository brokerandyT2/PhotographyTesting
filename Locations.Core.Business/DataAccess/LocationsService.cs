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
            try
            {
                return lq.SaveWithIDReturn(s);
            }
            catch (Exception ex)
            {
                Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
                return new LocationViewModel();
            }
        }
        public LocationViewModel Save(LocationViewModel locationViewModel, bool getWeather, bool returnNew)
        {
            try
            {
                if (getWeather)
                {
                    SettingsService set = new Business.DataAccess.SettingsService();

                    WeatherAPI weatherAPI = new WeatherAPI(set.GetSettingByName(Constants.Weather_API_Key_string).GetValue(), locationViewModel.Lattitude, locationViewModel.Longitude, set.GetSettingByName(Constants.WeatherURL_string).GetValue());
                    WeatherService weatherService = new WeatherService();
                    weatherService.Save(weatherAPI.GetWeatherAsync().Result);
                }
                lq.SaveItem(locationViewModel);
                return returnNew ? new LocationViewModel() : locationViewModel;
            }
            catch (Exception ex)
            {
                Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
                return new LocationViewModel();
            }
        }
        public LocationViewModel Save(LocationViewModel locationViewModel)
        {
            try
            {
                GeoLocationAPI geoLocationAPI = new GeoLocationAPI(ref locationViewModel);
                geoLocationAPI.GetCityAndState(locationViewModel.Lattitude, locationViewModel.Longitude);


                lq.SaveItem(locationViewModel);
                return locationViewModel;
            }
            catch (Exception ex)
            {
                Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
                return new LocationViewModel();
            }
        }
        public LocationViewModel Save(LocationViewModel locationViewModel, bool returnNew)
        {
            try
            {
                Save(locationViewModel);
                return new LocationViewModel();
            }
            catch (Exception ex)
            {
                Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
                return new LocationViewModel();
            }

        }

        public LocationViewModel Get(int id)
        {

            try
            {
                return lq.GetItem<LocationViewModel>(id);
            }
            catch (Exception ex)
            {
                Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
                return new LocationViewModel();
            }
        }
        public LocationViewModel GetLocation(double latitude, double longitude)
        {
            try
            {
                return lq.GetItem<LocationViewModel>(latitude, longitude);
            }
            catch (Exception ex)
            {
                Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
                return new LocationViewModel();
            }
        }
        public bool Delete(LocationViewModel locationViewModel)
        {
            try
            {
                var x = lq.DeleteItem<LocationViewModel>(locationViewModel);
                return x != 420 ? true : false;
            }
            catch (Exception ex)
            {
                Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
                return false;
            }
        }
        public bool Delete(int id)
        {
            try
            {
                var y = Get(id);
                return lq.DeleteItem<LocationViewModel>(y) == 420 ? false : true;
            }
            catch (Exception ex)
            {
                Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
                return false;
            }
        }
        public bool Delete(double latitude, double longitude)
        {
            try
            {
                var x = lq.GetItem<LocationViewModel>(latitude, longitude);
                return Delete(x.Id);
            }
            catch (Exception ex)
            {
                Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
                return false;
            }

        }
        public bool Update(LocationViewModel locationViewModel)
        {
            try
            {
                lq.Update(locationViewModel);
                return true;
            }
            catch (Exception ex)
            {
                Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
                return false;
            }
        }
        public List<LocationViewModel> GetLocations()
        {
            try
            {
                return lq.GetItems<LocationViewModel>().Where(x => x.IsDeleted == false).ToList();
            }
            catch (Exception ex)
            {
                Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
                return new List<LocationViewModel>();
            }
        }
    }
}
