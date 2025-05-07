using Location.Photography.Business.DataAccess.Interfaces;
using Location.Photography.Data.Queries;
using Locations.Core.Business.DataAccess;
using Locations.Core.Business.Logging.Interfaces;

using Locations.Core.Shared.ViewModels;
namespace Location.Photography.Business.DataAccess
{

    public class SettingsService : ServiceBase<SettingViewModel>, ISettingService<SettingViewModel>
    {
        SettingsQuery<SettingViewModel> sq;
        Locations.Core.Business.DataAccess.SettingsService ss = new Locations.Core.Business.DataAccess.SettingsService();


        public SettingsService() : base()
        {

        }

        public SettingsService( ILoggerService logger, SettingsQuery<SettingViewModel> query)
        {
            throw new NotImplementedException();
        }
        public SettingsService( ILoggerService logger, string email) : this()
        {
            throw new NotImplementedException();


        }
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
                RaiseError(ex);
                return default;
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
                RaiseError(ex);// return string.Empty;
                return default;
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
