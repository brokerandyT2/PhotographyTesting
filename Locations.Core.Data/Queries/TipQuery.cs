using EncryptedSQLite;
using Locations.Core.Shared;
using Locations.Core.Shared.Customizations.Alerts.Interfraces;
using Locations.Core.Shared.Customizations.Logging.Interfaces;
using Locations.Core.Shared.ViewModels;
using NormalSQLite;
namespace  Locations.Core.Data.Queries
{
    public class TipQuery<T> : QueryBase<T> where T : TipViewModel, new()
    {
        /// <summary>
        /// Use this constructor for encrypted database.
        /// </summary>
        /// <param name="alertServ"></param>
        /// <param name="loggerService"></param>
        /// <param name="email"></param>
        /// <exception cref="ArgumentException"></exception>
        public TipQuery(IAlertService alertServ, ILoggerService loggerService, string email) : this(alertServ, loggerService)
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
        public TipQuery(IAlertService alertServ, ILoggerService loggerService) : base(alertServ, loggerService)
        {
            dataB = DataUnEncrypted.GetConnection();
        }

        public override T GetItem<T>(int id)
        {
            return (T)Convert.ChangeType(dataB.Table<TipViewModel>().Where(x => x.ID == id).FirstOrDefaultAsync().Result,typeof(T));
        }

        public override T GetItemByString<T>(string title)
        {
            return (T)Convert.ChangeType(dataB.Table<TipViewModel>().Where(x => x.Title == title).FirstOrDefaultAsync().Result, typeof(T));
        }

        public string GetSettingByString<T1>(string name)
        {
            throw new NotImplementedException();
        }

        public override IList<T> GetItems<T>()
        {
            return (IList<T>)dataB.Table<TipViewModel>().ToListAsync().Result;
        }

        public override string GetValueByString<T>(string name)
        {
            throw new NotImplementedException();
        }
    }
}
