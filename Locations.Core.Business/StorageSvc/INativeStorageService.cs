using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locations.Core.Business.StorageSvc
{
    public interface INativeStorageService
    {
     /*   public static string GetSetting(string key)
        {
            return SecureStorage.GetAsync(key).Result;
        }
        public async Task SaveSetting(string key, string value)
        {
            var x = SecureStorage.SetAsync(key, value);
            return;
        }
        public Task UpdateSetting(string key, string oldValue, string newValue)
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
        }
        public bool DeleteSetting(string key)
        {
            return SecureStorage.Remove(key);
        }
        public Task UpdateSetting(string key, object oldValue, object newValue)
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
        } */
    }
}
