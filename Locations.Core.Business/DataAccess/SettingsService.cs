using Locations.Core.Business.DataAccess.Interfaces;
using Locations.Core.Business.Logging.Implementation;
using Locations.Core.Business.Logging.Interfaces;
using Locations.Core.Business.StorageSvc;
using Locations.Core.Data.Queries;
using Locations.Core.Shared;
using Locations.Core.Shared.Customizations.Alerts.Implementation;
using Locations.Core.Shared.Customizations.Alerts.Interfraces;
using Locations.Core.Shared.ViewModels;
using static Locations.Core.Shared.Customizations.Alerts.Implementation.AlertService;
namespace Locations.Core.Business.DataAccess
{
    public class SettingsService : ServiceBase<SettingViewModel>, ISettingService<SettingViewModel>
    {
        private SettingsQuery<SettingViewModel> _query;
        public event EventHandler<AlertEventArgs> AlertRaised;
        private IAlertService alertServ;
        private ILoggerService loggerService;

        
        public SettingsService() 
        {
            alertServ = new AlertService();
            loggerService = new LoggerService();

            _query = new SettingsQuery<SettingViewModel>( );

        }
       
        public SettingsService(IAlertService alert, string email) :this()
        {
            var q = new SettingsQuery<SettingViewModel>();
            var x = NativeStorageService.GetSetting(MagicStrings.Email);
            if (string.IsNullOrEmpty(x))
            {
                throw new ArgumentException("Email is not set.  Cannot use encrypted database.");
            }
            _query = new SettingsQuery<SettingViewModel>();
        }

        public SettingsService(bool v)
        {
            _query = new SettingsQuery<SettingViewModel>(true);
        }

        private void SettingsService_AlertRaised(object? sender, AlertEventArgs e)
        {
            RaiseError(new Exception(e.Title));
        }

        /// <summary>
        /// Returns the entire ViewModel
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public SettingViewModel GetSettingByName(string name)
        {
            try
            {
                return _query.GetItemByString<SettingViewModel>(name);
            }
            catch (Exception ex)
            {
                RaiseError(ex);
                return new SettingViewModel();
            }
        }

        public bool UpdatePageView(string page)
        {
            try
            {
                var s = GetSettingByName(page);
                s.Timestamp = DateTime.Now;
                s.Value = true.ToString();
                _query.Update(s);
                return true;
            }
            catch (Exception ex)
            {
                RaiseError(ex);
                return false;
            }
        }
        public SettingViewModel GetSetting(string key)
        {
            try
            {
                return _query.GetItemByString<SettingViewModel>(key);
            }
            catch (Exception ex)
            {
                RaiseError(ex);
                return new SettingViewModel();
            }

        }
        public void UpdateSetting(SettingViewModel setting)
        {
            try
            {
                setting.Timestamp = DateTime.Now;
                _query.Update(setting);
            }
            catch (Exception ex)
            {
                RaiseError(ex);
            }
        }
        public void DeleteSetting(string key)
        {
            try
            {
                _query.DeleteItem(GetSetting(key));
            }
            catch (Exception ex)
            {
                RaiseError(ex);
            }

        }
        public SettingViewModel Save(SettingViewModel model)
        {
            try
            {
                model.Timestamp = DateTime.Now;
                _query.SaveItem(model);
                return model;
            }
            catch (Exception ex)
            {
                RaiseError(ex);
                return model;
            }
        }
        public SettingViewModel Save(SettingViewModel model, bool returnNew)
        {
            try
            {
                model.Timestamp = DateTime.Now;
                _query.SaveItem(model);
                return returnNew ? new SettingViewModel() : model;
            }
            catch (Exception ex)
            {
                RaiseError(ex);
                return model;
            }
        }
        public SettingViewModel Get(int id)
        {
            try
            {
                return _query.GetItem<SettingViewModel>(id);
            }
            catch (Exception ex)
            {
                RaiseError(ex);
                return new SettingViewModel();
            }
        }

        public bool Delete(SettingViewModel model)
        {
            try
            {
                return _query.DeleteItem<SettingViewModel>(model) != 420 ? true : false;
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
                var model = Get(id);
                return _query.DeleteItem<SettingViewModel>(model) != 420 ? true : false;
            }
            catch (Exception ex)
            {

                RaiseError(ex);
                return false;
            }
        }
        public bool Delete(double latitude, double longitude)
        {
            throw new NotImplementedException();
        }

        public SettingsViewModel GetAllSettings()
        {
            try
            {
                SettingsViewModel s = new SettingsViewModel();
                s.TimeFormat = GetSettingByName(MagicStrings.TimeFormat);
                s.DateFormat = GetSettingByName(MagicStrings.DateFormat);
                s.LastBulkWeatherUpdate = GetSettingByName(MagicStrings.LastBulkWeatherUpdate);
                s.SubscriptionExpiration = GetSettingByName(MagicStrings.SubscriptionExpiration);
                s.DeviceInfo = GetSettingByName(MagicStrings.DeviceInfo);
                s.Email = GetSettingByName(MagicStrings.Email);
                s.ExposureCalcViewed = GetSettingByName(MagicStrings.ExposureCalcViewed);
                s.FirstName = GetSettingByName(MagicStrings.FirstName);
                s.Hemisphere = GetSettingByName(MagicStrings.Hemisphere);
                s.HomePageViewed = GetSettingByName(MagicStrings.HomePageViewed);
                s.Language = GetSettingByName(MagicStrings.DefaultLanguage);
                s.LastName = GetSettingByName(MagicStrings.LastName);
                s.LightMeterViewed = GetSettingByName(MagicStrings.LightMeterViewed);
                s.ListLocationsViewed = GetSettingByName(MagicStrings.LocationListViewed);
                s.SceneEvaluationViewed = GetSettingByName(MagicStrings.SceneEvaluationViewed);
                s.SubscriptionExpiration = GetSettingByName(MagicStrings.SubscriptionExpiration);
                s.TipsViewed = GetSettingByName(MagicStrings.TipsViewed);
                s.UniqeID = GetSettingByName(MagicStrings.UniqueID);
                s.AdSupport = GetSettingByName(MagicStrings.FreePremiumAdSupported);
                s.SubscriptionType = GetSettingByName(MagicStrings.SubscriptionType);
                s.SunCalculationViewed = GetSettingByName(MagicStrings.SunCalculatorViewed);
                s.WindDirection = GetSettingByName(MagicStrings.WindDirection);
                s.TemperatureFormat = GetSettingByName(MagicStrings.TemperatureType);
                return s;
            }
            catch (Exception ex)
            {
                AlertRaised?.Invoke(this, new AlertEventArgs("Error", "Unable to load settings", AlertType.Error));
                 
                return new SettingsViewModel();
            }
        }
        public SettingViewModel SaveSettingWithObjectReturn(SettingViewModel s)
        {
            try
            {
                s.Timestamp = DateTime.Now;
                return _query.SaveWithIDReturn(s);
            }
            catch (Exception ex)
            {
                RaiseError(ex);
                return s;
            }
        }

    }
}
