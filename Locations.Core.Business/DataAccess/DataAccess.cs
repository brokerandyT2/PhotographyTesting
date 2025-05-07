using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using Locations.Core.Data.Queries;
using Locations.Core.Shared;
using Locations.Core.Shared.ViewModels;
using Locations.Core.Shared.DTO;
using Locations.Core.Shared.Helpers;
using Locations.Core.Shared.Customizations.Alerts.Implementation;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using EncryptedSQLite;
using Locations.Core.Business.StorageSvc;
using Locations.Core.Business.Logging.Implementation;
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
            

            var x  = new NativeStorageService(new AlertService(), new LoggerService());
            var email = NativeStorageService.GetSetting(MagicStrings.Email);
            if (string.IsNullOrEmpty(email))
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
                    return new SQLiteAsyncConnection(MagicStrings.DataBasePath, Constants.Flags);
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
                    DataPopulation.PopulateData();
#endif
                }
            }



        }
    }
}
