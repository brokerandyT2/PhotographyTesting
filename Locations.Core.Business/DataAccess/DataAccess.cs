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
#if PHOTOGRAPHY
                    DataPopulation.PopulateData();
#endif
                }
            }



        }
    }
}
