using Location.Photography.Business.DataAccess.Interfaces;
using Location.Photography.Data.Queries;
using Locations.Core.Business.DataAccess;
using Locations.Core.Shared;
using Locations.Core.Shared.Customizations.Alerts.Implementation;
using Locations.Core.Shared.Customizations.Alerts.Interfraces;
using Locations.Core.Shared.Customizations.Logging.Implementation;
using Locations.Core.Shared.Customizations.Logging.Interfaces;
using Locations.Core.Shared.ViewModels;
using NormalSQLite;
namespace Location.Photography.Business.DataAccess
{

    public class SettingsService : ServiceBase<SettingViewModel>, ISettingService<SettingViewModel>
    {
        SettingsQuery<SettingViewModel> sq;
        Locations.Core.Business.DataAccess.SettingsService ss = new Locations.Core.Business.DataAccess.SettingsService();
        private IAlertService alertServ;
        private ILoggerService loggerService;
        public SettingsService() { }
        public SettingsService(IAlertService alert, ILoggerService logger): this()
        {
            alertServ = alert;
            loggerService = logger;
            sq.dataB = NormalSQLite.DataUnEncrypted.GetConnection();
        }
        public SettingsService(IAlertService alert, ILoggerService logger, SettingsQuery<SettingViewModel> query)
        {
            alertServ = alert;
            loggerService = logger;
            sq = query;
        }
        public SettingsService(IAlertService alert, ILoggerService logger, string email)
        {
            alertServ = alert;
            loggerService = logger;
            var x = GetSettingByName(MagicStrings.Email).Value;
            if (string.IsNullOrEmpty(x))
            {
                loggerService.LogWarning($"Email is not set.  Cannot use encrypted database. Email Address {x}");
                RaiseError(new ArgumentException("Email is not set.  Cannot use encrypted database."));
            }

            sq.dataB = EncryptedSQLite.DataEncrypted.GetConnection(email);
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
