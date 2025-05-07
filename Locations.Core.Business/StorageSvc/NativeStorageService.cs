using Locations.Core.Business.DataAccess;
using Locations.Core.Business.Logging.Interfaces;
using Locations.Core.Shared.Customizations.Alerts.Interfraces;
using Locations.Core.Shared.Customizations.Logging.Interfaces;
using Locations.Core.Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Locations.Core.Business.StorageSvc
{
    public class NativeStorageService : ServiceBase<DummyViewModel>, INativeStorageService
    {

        private static IAlertService _alertService;
        private static ILoggerService _loggerService;

        public NativeStorageService(IAlertService alertService, ILoggerService loggerService)
        {
            _alertService = alertService;
            _loggerService = loggerService;
        }
        public static bool DeleteSetting(string key)
        {
            try
            {
                return SecureStorage.Remove(key);
            }
            catch (Exception ex)
            {
                RaiseError(ex);
                _loggerService.LogError(ex.Message);

                return false;
            }
        }

        public static string GetSetting(string key)
        {
            try
            {
                return SecureStorage.GetAsync(key).Result;
            }
            catch (Exception ex)
            {
                RaiseError(ex);
                _loggerService.LogError(ex.Message);
                return string.Empty;
            }
            return SecureStorage.GetAsync(key).Result;
        }

        public static async Task SaveSetting(string key, string value)
        {
            try
            {
                var x = SecureStorage.SetAsync(key, value);
                return;
            }
            catch (Exception ex)
            {
                RaiseError(ex);
                _loggerService.LogError(ex.Message);
            }
        }
        public static async Task SaveSetting(string key, object value)
        {
            try
            {
                var x = SecureStorage.SetAsync(key, value.ToString());
                return;
            }
            catch (Exception ex)
            {
                RaiseError(ex);
                _loggerService.LogError(ex.Message);
            }
        }
        public static Task UpdateSetting(string key, string oldValue, string newValue)
        {
            try
            {
                var oldSetting = SecureStorage.GetAsync(key).Result;
                if (oldSetting != oldValue)
                {
                    SecureStorage.Remove(key);

                    return SecureStorage.SetAsync(key, newValue);
                }
                else
                {
                    return Task.FromResult(false);
                }
            }catch(Exception ex)
            {
                RaiseError(ex);
                _loggerService.LogError(ex.Message);
                return Task.FromResult(false);
            }
        }

        public static Task UpdateSetting(string key, object oldValue, object newValue)
        {
            try
            {
                var z = SecureStorage.GetAsync(key).Result;
                if (z != oldValue.ToString())
                {
                    SecureStorage.Remove(key);
                    return SecureStorage.SetAsync(key, newValue.ToString());
                }
                else
                {
                    return Task.FromResult(false);
                }
            }catch (Exception ex)
            {
                RaiseError(ex);
                _loggerService.LogError(ex.Message);
                return Task.FromResult(false);
            }
        }


    }
}
