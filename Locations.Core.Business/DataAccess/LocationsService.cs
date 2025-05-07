using Locations.Core.Business.DataAccess.Interfaces;
using Locations.Core.Business.GeoLocation;
using Locations.Core.Business.Logging.Interfaces;
using Locations.Core.Business.StorageSvc;
using Locations.Core.Business.Weather;
using Locations.Core.Data.Queries;
using Locations.Core.Shared;
using Locations.Core.Shared.Customizations.Alerts.Implementation;
using Locations.Core.Shared.Customizations.Alerts.Interfraces;
using Locations.Core.Shared.Helpers;
using Locations.Core.Shared.ViewModels;

namespace Locations.Core.Business.DataAccess
{
    public class LocationsService : ServiceBase<LocationViewModel>, ILocationService<LocationViewModel>
    {
        private LocationQuery<LocationViewModel> lq;
        private IAlertService alertServ;

        private string email;
        private string guid;
        public LocationsService()
        {

            email = NativeStorageService.GetSetting(MagicStrings.Email);
            guid = NativeStorageService.GetSetting(MagicStrings.UniqueID);
            lq = new Data.Queries.LocationQuery<LocationViewModel>();
            AlertRaised += LocationsService_AlertRaised;
            if (string.IsNullOrEmpty(email))
            {
                //loggerService.LogWarning($"Email is not set.  Cannot use encrypted database. Email Address {x}");
                throw new ArgumentException("Email is not set.  Cannot use encrypted database.");
            }
            lq = new LocationQuery<LocationViewModel>();
        }


        private void LocationsService_AlertRaised(object? sender, AlertEventArgs e)
        {
            RaiseError(new Exception(e.Title));
        }

        public LocationsService(IAlertService alertServ, ILoggerService loggerService) : this()
        {
            this.alertServ = alertServ;
        }
        public LocationsService(IAlertService alertServ, ILoggerService loggerService, string email) : this(alertServ, loggerService)
        {
            var q = new SettingsQuery<SettingViewModel>();
            var x = q.GetItemByString<SettingViewModel>(MagicStrings.Email).Value;

        }
        public LocationViewModel SaveSettingWithObjectReturn(LocationViewModel s)
        {
            try
            {
                var x = lq.SaveWithIDReturn(s);
                if (x.IsError)
                {
                    RaiseAlert(this, new AlertEventArgs(x.alertEventArgs.Title, x.alertEventArgs.Message, true));
                }
                return s;
            }
            catch (Exception ex)
            {
                RaiseError(ex);

                return s;
            }
        }
        public LocationViewModel Save(LocationViewModel locationViewModel, bool getWeather, bool returnNew)
        {
            try
            {
                if (getWeather)
                {
                    SettingsService set = new Business.DataAccess.SettingsService();

                    WeatherAPI weatherAPI = new WeatherAPI(set.GetSettingByName(MagicStrings.Weather_API_Key).GetValue(), locationViewModel.Lattitude, locationViewModel.Longitude, set.GetSettingByName(MagicStrings.WeatherURL).GetValue());
                    WeatherService weatherService = new WeatherService();
                    weatherService.Save(weatherAPI.GetWeatherAsync().Result);
                }
                lq.SaveItem(locationViewModel);
                if (lq.IsError)
                {
                    RaiseAlert(this, new AlertEventArgs(lq.alertEventArgs.Title, lq.alertEventArgs.Message, true));
                }
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
                if (lq.IsError)
                {
                    RaiseAlert(this, new AlertEventArgs(lq.alertEventArgs.Title, lq.alertEventArgs.Message));
                }
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
                var x = Save(locationViewModel);
                if (x.IsError)
                {
                    RaiseAlert(this, new AlertEventArgs(x.alertEventArgs.Title, x.alertEventArgs.Message, true));
                }
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
                var x = lq.GetItem<LocationViewModel>(id);
                if (x.IsError)
                {
                    RaiseAlert(this, new AlertEventArgs(x.alertEventArgs.Title, x.alertEventArgs.Message, true));
                }
                return x;
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
                var x = lq.GetItem<LocationViewModel>(latitude, longitude);
                if (x.IsError)
                {
                    RaiseAlert(this, new AlertEventArgs(x.alertEventArgs.Title, x.alertEventArgs.Message, true));
                }
                return x;
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
                if (lq.IsError)
                {
                    RaiseAlert(this, new AlertEventArgs(lq.alertEventArgs.Title, lq.alertEventArgs.Message, true));
                }
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
                var x = lq.DeleteItem<LocationViewModel>(y);
                if (lq.IsError)
                {
                    RaiseAlert(this, new AlertEventArgs(lq.alertEventArgs.Title, lq.alertEventArgs.Message, true));
                }
                return x == 420 ? false : true;
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
                var z= Delete(x.Id);
                if (lq.IsError)
                {
                    RaiseAlert(this, new AlertEventArgs(lq.alertEventArgs.Title, lq.alertEventArgs.Message, true));
                }
                return z;
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
                if (lq.IsError)
                {
                    RaiseAlert(this, new AlertEventArgs(lq.alertEventArgs.Title, lq.alertEventArgs.Message, true));
                }
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
              var x =   lq.GetItems<LocationViewModel>().Where(x => x.IsDeleted == false).ToList();
                if (lq.IsError)
                {
                    RaiseAlert(this, new AlertEventArgs(lq.alertEventArgs.Title, lq.alertEventArgs.Message, true)) ;
                }
                return x ;
            }
            catch (Exception ex)
            {
                RaiseError(ex);
                return new List<LocationViewModel>();
            }
        }
    }
}
