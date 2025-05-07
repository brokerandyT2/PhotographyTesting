using EncryptedSQLite;
using Locations.Core.Shared;
using Locations.Core.Shared.StorageSvc;
using Locations.Core.Shared.ViewModels;

namespace  Locations.Core.Data.Queries
{
    public class SettingsQuery<T> : QueryBase<T> where T : SettingViewModel, new()
    {
        /// <summary>
        /// Use this constructor for encrypted database.
        /// </summary>
        /// <param name="alertServ"></param>
        /// <param name="loggerService"></param>
        /// <param name="email"></param>
        /// <exception cref="ArgumentException"></exception>
       
        /// <summary>
        /// Use this constructor for unencrypted database.
        /// </summary>
        /// <param name="alertServ"></param>
        /// <param name="loggerService"></param>
        public SettingsQuery() :base()
        {
            dataB = DataEncrypted.GetAsyncConnection(KEY);
        }

        /// <summary>
        /// Do NOT TOUCH / DO NOT USE
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>

        public override T GetItem<T>(int id)
        {
            return (T)Convert.ChangeType(dataB.Table<SettingViewModel>().Where(x => x.Id == id).FirstOrDefaultAsync().Result, typeof(T));
        }

        public string GetValueBy<T>(string name)
        {
            return dataB.Table<SettingViewModel>().Where(x=> x.Name == name).FirstOrDefaultAsync().Result.Value;
        }
        public override string GetValueByString<T>(string name)
        {
            return dataB.Table<SettingViewModel>().Where(x => x.Name == name).FirstOrDefaultAsync().Result.Value;
        }
        public override IList<T> GetItems<T>()
        {
            IList<T> results = (IList<T>)dataB.Table<SettingViewModel>().Where(x => x == x).ToListAsync().Result;
           
            return results;
        }
        public override T GetItemByString<T>(string name)
        {
            return (T)Convert.ChangeType(dataB.Table<SettingViewModel>().Where(x=>x.Name == name).FirstOrDefaultAsync().Result, typeof(T));    
        }

    }
}
