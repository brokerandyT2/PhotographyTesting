using EncryptedSQLite;
using Locations.Core.Shared.ViewModels;
using SQLite;

namespace Location.Photography.Data.Queries
{
    public class SettingsQuery<T> : QueryBase<T> where T : SettingViewModel, new()
    {

        SettingsQuery<SettingViewModel> settings;

        /// <summary>
        /// Use this constructor for encrypted database.
        /// </summary>
        /// <param name="alertServ"></param>
        /// <param name="loggerService"></param>
        /// <param name="email"></param>
        /// <exception cref="ArgumentException"></exception>
        public SettingsQuery() : base()
        {
            settings = new SettingsQuery<SettingViewModel>();
         
      
        }

        public override T GetItem<T>(int id)
        {
            return (T)Convert.ChangeType(dataAsync.Table<SettingViewModel>().Where(x => x.Id == id).FirstOrDefaultAsync().Result, typeof(T));
        }

        public string GetValueBy<T>(string name)
        {
            return dataAsync.Table<SettingViewModel>().Where(x => x.Name == name).FirstOrDefaultAsync().Result.Value;

        }
        public override string GetValueByString<T>(string name)
        {
            return dataAsync.Table<SettingViewModel>().Where(x => x.Name == name).FirstOrDefaultAsync().Result.Value;
        }
        public override IList<T> GetItems<T>()
        {
            IList<T> results = (IList<T>)dataAsync.Table<SettingViewModel>().Where(x => x == x).ToListAsync().Result;

            return results;
        }

    }
}
