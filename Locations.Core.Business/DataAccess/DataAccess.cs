using EncryptedSQLite;
using Locations.Core.Business.Logging.Implementation;
using Locations.Core.Business.StorageSvc;
using Locations.Core.Data.Queries;
using Locations.Core.Shared;
using Locations.Core.Shared.Customizations.Alerts.Implementation;
using Locations.Core.Shared.Helpers;
using Locations.Core.Shared.ViewModels;
using Microsoft.Extensions.Logging;
using SQLite;
namespace Locations.Core.Business.DataAccess
{
    public class DataAccess: DataAccessBase
    {
        public event EventHandler<AlertEventArgs> RaiseAlert;
        public bool IsError { get; set; } = false;
        public AlertEventArgs alertEventArgs;
        SQLiteAsyncConnection _connection;

        public DataAccess()
        {
            

           
            if (string.IsNullOrEmpty(KEY))
            {
                RaiseError(new ArgumentException("Eror"));
            }
             
            _connection = EncryptedSQLite.DataEncrypted.GetAsyncConnection(KEY);
                
           
                CheckForDB();
            
        }

        private void DataAccess_RaiseAlert(object? sender, AlertEventArgs e)
        {
            throw new NotImplementedException();
        }

        public ISQLiteAsyncConnection Connection
        {
            get
            {
                if (_connection != null)
                {
                    CheckForDB();
                    return _connection;
                }
                else
                {
                    return DataEncrypted.GetAsyncConnection(NativeStorageService.GetSetting(MagicStrings.UniqueID)+NativeStorageService.GetSetting(MagicStrings.UniqueID));
                }
            }
        }
        private void CheckForDB()
        {
            //_connection = new SQLiteAsyncConnection(MagicStrings.DataBasePath, Constants.Flags);
            var services = new ServiceCollection();
            services.AddLogging(); // Registers ILogger<T>

            // Build it
            var provider = services.BuildServiceProvider();

            // Get ILogger<LoggerService>
            var logger = provider.GetRequiredService<ILogger<LoggerService>>();

            // Now you can construct LoggerService
            var loggerService = new LoggerService(logger);
            SettingsQuery<SettingViewModel> setting = new SettingsQuery<SettingViewModel>();
            string x = string.Empty;
            try
            {
                x = setting.GetItemByString<SettingViewModel>(MagicStrings.Email).GetValue();
                return;
            }
            catch (Exception ex)
            {
                if (x == string.Empty)
                {
                    var a = _connection.CreateTableAsync<SettingViewModel>().Result;
                    var b = _connection.CreateTableAsync<WeatherViewModel>().Result;
                    var c = _connection.CreateTableAsync<TipViewModel>().Result;
                    var d = _connection.CreateTableAsync<TipTypeViewModel>().Result;
                    var e = _connection.CreateTableAsync<LocationViewModel>().Result;
                    var f = _connection.CreateTableAsync<Log>().Result;
#if PHOTOGRAPHY
                    //DataPopulation.PopulateData();
#endif
                }
            }



        }
    }
}
