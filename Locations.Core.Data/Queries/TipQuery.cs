using EncryptedSQLite;
using Location.Core.Helpers.AlertService;
using Location.Core.Helpers.LoggingService;
using Locations.Core.Data.Models;
using Locations.Core.Shared;
using Locations.Core.Shared.StorageSvc;
using Locations.Core.Shared.ViewModels;

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
        public TipQuery(IAlertService _alertserv, ILoggerService _logger) : base(_alertserv,_logger)
        {
            SettingsQuery<SettingViewModel> settings = new SettingsQuery<SettingViewModel>();
            var addy = NativeStorageService.GetSetting(MagicStrings.Email);

            if (string.IsNullOrEmpty(addy))
            {
                //loggerService.LogWarning($"Email is not set.  Cannot use encrypted database. Email Address {addy}");
                throw new ArgumentException("Email is not set.  Cannot use encrypted database.");
            }
            else
            {
                dataB = DataEncrypted.GetAsyncConnection();
            }
        }



        /// <summary>
        /// Use this constructor for unencrypted database.
        /// </summary>
        /// <param name="alertServ"></param>
        /// <param name="loggerService"></param>

        public  T GetItem<T>(int id)
        {
            return (T)Convert.ChangeType(dataB.Table<TipViewModel>().Where(x => x.ID == id).FirstOrDefaultAsync().Result,typeof(T));
        }

        public  T GetItemByString<T>(string title)
        {
            return (T)Convert.ChangeType(dataB.Table<TipViewModel>().Where(x => x.Title == title).FirstOrDefaultAsync().Result, typeof(T));
        }

        public string GetSettingByString<T1>(string name)
        {
            throw new NotImplementedException();
        }

        public  IList<T> GetItems<T>()
        {
            return (IList<T>)dataB.Table<TipViewModel>().ToListAsync().Result;
        }

        public  string GetValueByString<T>(string name)
        {
            throw new NotImplementedException();
        }

        public override Task<DataOperationResult<T1>> GetItemAsync<T1>(int id)
        {
            throw new NotImplementedException();
        }

        public override Task<DataOperationResult<string>> GetValueByStringAsync<T1>(string name)
        {
            throw new NotImplementedException();
        }

        public override Task<DataOperationResult<IList<T1>>> GetItemsAsync<T1>()
        {
            throw new NotImplementedException();
        }

        public override Task<DataOperationResult<T1>> GetItemByStringAsync<T1>(string name)
        {
            throw new NotImplementedException();
        }
    }
}
