using Locations.Core.Shared.ViewModels;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using Microsoft.Data.Sqlite;
using Locations.Core.Shared;
using Locations.Core.Shared.Customizations.Alerts.Interfraces;
using Locations.Core.Shared.Customizations.Logging.Interfaces;
using EncryptedSQLite;
using NormalSQLite;

namespace Locations.Core.Data.Queries
{
    public class TipTypesQuery<T> : QueryBase<T> where T : TipTypeViewModel, new()
    {
        /// <summary>
        /// Use this constructor for encrypted database.
        /// </summary>
        /// <param name="alertServ"></param>
        /// <param name="loggerService"></param>
        /// <param name="email"></param>
        /// <exception cref="ArgumentException"></exception>
        public TipTypesQuery(IAlertService alertServ, ILoggerService loggerService, string email) : this(alertServ, loggerService)
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
        public TipTypesQuery(IAlertService alertServ, ILoggerService loggerService) : base(alertServ, loggerService)
        {
            dataB = DataUnEncrypted.GetConnection();
        }

        public override T GetItem<T>(int id)
        {
            return (T)Convert.ChangeType(dataB.Table<TipTypeViewModel>().Where(x => x.Id == id), typeof(T));
        }
       
        public override T GetItemByString<T>(string name)
        {
            throw new NotImplementedException();
        }

        public override IList<T> GetItems<T>()
        {
            return (IList<T>)dataB.Table<TipTypeViewModel>().Where(x => x == x).ToListAsync().Result;
        }

        public override string GetValueByString<T>(string name)
        {
            throw new NotImplementedException();
        }
    }
}
