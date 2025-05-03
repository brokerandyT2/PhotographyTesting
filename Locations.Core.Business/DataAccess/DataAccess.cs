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
using Locations.Core.Shared.Customizations.Logging.Implementation;
using Locations.Core.Shared.Customizations.Logging.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Locations.Core.Business.DataAccess
{
    public class DataAccess
    {
        SQLiteAsyncConnection _connection;
        public DataAccess()
        {
            
                _connection = new SQLiteAsyncConnection(MagicStrings.DataBasePath, Constants.Flags);
                
           
                CheckForDB();
            
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
            SettingsQuery<SettingViewModel> setting = new SettingsQuery<SettingViewModel>(new AlertService(), loggerService);
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
#if PHOTOGRAPHY
                    DataPopulation.PopulateData();
#endif
                }
            }



        }
    }
}
