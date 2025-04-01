using Locations.Core.Business.DataAccess.Interfaces;
using Locations.Core.Data.Queries;
using Locations.Core.Shared.ViewModels;
using Locations.Core.Shared;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Locations.Core.Business.DataAccess
{
    public class SettingsService : ISettingService<SettingViewModel>
    {
        private SettingsQuery<SettingViewModel> _query = new SettingsQuery<SettingViewModel>();
        public SettingsService()
        {
        }
        /// <summary>
        /// Returns the entire ViewModel
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public SettingViewModel GetSettingByName(string name)
        {  var x = _query.GetItemByString<SettingViewModel>(name);
            return x;
        }
        public SettingViewModel GetSetting(string key)
        {

            return _query.GetItemByString<SettingViewModel>(key);
        }
        public void UpdateSetting(SettingViewModel setting)
        {
            _query.Update(setting);
        }
        public void DeleteSetting(string key)
        {
            _query.DeleteItem(GetSetting(key));

        }
        public SettingViewModel Save(SettingViewModel model)
        {
            _query.SaveItem(model);
            return model;
        }
        public SettingViewModel Save(SettingViewModel model, bool returnNew)
        {

            _query.SaveItem(model);
            return returnNew ? new SettingViewModel() : model;
        }
        public SettingViewModel Get(int id)
        {

            return _query.GetItem<SettingViewModel>(id);
        }

        public bool Delete(SettingViewModel model)
        {

            return _query.DeleteItem<SettingViewModel>(model) != 420 ? true : false;
        }
        public bool Delete(int id)
        {
            var model = Get(id);
            return _query.DeleteItem<SettingViewModel>(model) != 420 ? true : false;
        }
        public bool Delete(double latitude, double longitude)
        {
            throw new NotImplementedException();
        }

        public SettingsViewModel GetAllSettings()
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
            s.SubscriptionExpiration = GetSettingByName(MagicStrings.SubscriptionExpiration) ;
            s.TipsViewed = GetSettingByName(MagicStrings.TipsViewed);
            s.UniqeID = GetSettingByName(MagicStrings.UniqueID) ;
            
            return s;
        }
        public SettingViewModel SaveSettingWithObjectReturn(SettingViewModel s)
        {
            return _query.SaveWithIDReturn(s);
        }

    }
}
