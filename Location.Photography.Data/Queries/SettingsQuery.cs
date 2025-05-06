using EncryptedSQLite;
using Locations.Core.Shared;
using Locations.Core.Shared.Customizations.Alerts.Interfraces;
using Locations.Core.Shared.Customizations.Logging.Interfaces;
using Locations.Core.Shared.ViewModels;
using NormalSQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Location.Photography.Data.Queries
{
    public class SettingsQuery<T> : QueryBase<T> where T : SettingViewModel, new()
    {


        private IAlertService alertServ;
        private ILoggerService loggerService;
        /// <summary>
        /// Use this constructor for encrypted database.
        /// </summary>
        /// <param name="alertServ"></param>
        /// <param name="loggerService"></param>
        /// <param name="email"></param>
        /// <exception cref="ArgumentException"></exception>
        public SettingsQuery(IAlertService alertServ, ILoggerService loggerService, string email) : this(alertServ, loggerService)
        {
            SettingsQuery<SettingViewModel> settings = new SettingsQuery<SettingViewModel>(alertServ, loggerService);
            var addy = settings.GetItemByString<SettingViewModel>(MagicStrings.Email).Value;

            if (string.IsNullOrEmpty(addy))
            {
                loggerService.LogWarning($"Email is not set.  Cannot use encrypted database. Email Address {addy}");
                throw new ArgumentException("Email is not set.  Cannot use encrypted database.");
            }
            dataB = DataEncrypted.GetConnection(email);
        }
        /// <summary>
        /// Use this constructor for unencrypted database.
        /// </summary>
        /// <param name="alertServ"></param>
        /// <param name="loggerService"></param>
        public SettingsQuery(IAlertService alertServ, ILoggerService loggerService) 
        {
            this.alertServ = alertServ;
            this.loggerService = loggerService;
            dataB = DataUnEncrypted.GetConnection();
        }

        public override T GetItem<T>(int id)
        {
            return (T)Convert.ChangeType(dataB.Table<SettingViewModel>().Where(x => x.Id == id).FirstOrDefaultAsync().Result, typeof(T));
        }

        public string GetValueBy<T>(string name)
        {
            return dataB.Table<SettingViewModel>().Where(x => x.Name == name).FirstOrDefaultAsync().Result.Value;

        }
        public override string GetValueByString<T>(string name)
        {
            return dataB.Table<SettingViewModel>().Where(x => x.Name == name).FirstOrDefaultAsync().Result.Value;
        }
        public override IList<T> GetItems<T>()
        {
            IList<T> results = (IList<T>)dataB.Table<SettingViewModel>().Where(x => x == x).ToListAsync().Result;

            return results;
        }

    }
}
