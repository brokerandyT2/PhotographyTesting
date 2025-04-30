using Location.Photography.Business.DataAccess.Interfaces;
using Location.Photography.Data.Queries;
using Locations.Core.Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Location.Photography.Business.DataAccess
{

    public class SettingsService : ISettingService<SettingViewModel>
    {
        SettingsQuery<SettingViewModel> sq = new SettingsQuery<SettingViewModel>();
        Locations.Core.Business.DataAccess.SettingsService ss = new Locations.Core.Business.DataAccess.SettingsService();
        public SettingsService() { }

        public bool Delete(SettingViewModel model)
        {
            throw new NotImplementedException();
        }

        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public bool Delete(double latitude, double longitude)
        {
            throw new NotImplementedException();
        }

        public SettingViewModel Get(int id)
        {
            throw new NotImplementedException();
        }

        public SettingsViewModel GetAllSettings()
        {
            throw new NotImplementedException();
        }

        public SettingViewModel GetSettingByName(string name)
        {
            try
            {
                return ss.GetSetting(name);
            }
            catch (Exception ex)
            {
                Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
                return new SettingViewModel();
            }
        }


        public object GetSettingByName(object cameraRefresh)
        {
            throw new NotImplementedException();
        }

        public string GetSettingWithMagicString(string key)
        {
            try
            {
                return GetSettingByName(key).Value;
            }
            catch (Exception ex)
            {
                Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
                return string.Empty;
            }
        }



        public SettingViewModel Save(SettingViewModel model)
        {
            throw new NotImplementedException();
        }

        public SettingViewModel Save(SettingViewModel model, bool returnNew)
        {
            throw new NotImplementedException();
        }

        SettingsViewModel ISettingService<SettingViewModel>.GetAllSettings()
        {
            throw new NotImplementedException();
        }

        SettingViewModel ISettingService<SettingViewModel>.GetSettingByName(string name)
        {
            throw new NotImplementedException();
        }
    }
}
