namespace Locations.Core.Shared.StorageSvc
{
    public class NativeStorageService : INativeStorageService
    {



        public NativeStorageService()
        {

        }
        public static bool DeleteSetting(string key)
        {
            try
            {
                return SecureStorage.Remove(key);
            }
            catch (Exception ex)
            {
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

                return;
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
                return;
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
                return Task.FromResult(false);
            }
        }


    }
}
