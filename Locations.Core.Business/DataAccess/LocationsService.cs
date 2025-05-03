using Locations.Core.Business.DataAccess.Interfaces;
using Locations.Core.Business.GeoLocation;
using Locations.Core.Business.Weather;
using Locations.Core.Data.Queries;
using Locations.Core.Shared.Customizations.Alerts.Implementation;
using Locations.Core.Shared.Customizations.Alerts.Interfraces;
using Locations.Core.Shared.Customizations.Logging.Implementation;
using Locations.Core.Shared.Customizations.Logging.Interfaces;
using Locations.Core.Shared.Helpers;
using Locations.Core.Shared.ViewModels;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locations.Core.Business.DataAccess
{
    public class LocationsService : ServiceBase<LocationViewModel>, ILocationService<LocationViewModel>
    {
        private LocationQuery<LocationViewModel> lq = new Data.Queries.LocationQuery<LocationViewModel>(new AlertService(), new LoggerService(new ServiceCollection().AddLogging().BuildServiceProvider().GetRequiredService<ILogger<LoggerService>>()));
        private IAlertService alertServ;
        private ILoggerService loggerService;
        public event EventHandler<AlertEventArgs> AlertRaised;
        public LocationsService() {

            AlertRaised += LocationsService_AlertRaised;
        }


        private void LocationsService_AlertRaised(object? sender, AlertEventArgs e)
        {
            RaiseError(new Exception(e.Title));
        }

        public LocationsService(IAlertService alertServ, ILoggerService loggerService):this()
        {
            this.alertServ = alertServ;
            this.loggerService = loggerService;
        }
        public LocationViewModel SaveSettingWithObjectReturn(LocationViewModel s)
        {
            try
            {
                return lq.SaveWithIDReturn(s);
            }
            catch (Exception ex)
            {
               RaiseError(ex);

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
                RaiseError(ex);
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
           RaiseError(ex);
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
                RaiseError(ex);
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
                RaiseError(ex);
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
                RaiseError(ex);
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
                RaiseError(ex);
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
                RaiseError(ex);
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
                RaiseError(ex);
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
                RaiseError(ex);
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
                RaiseError(ex);
                return new List<LocationViewModel>();
            }
        }
    }
}
