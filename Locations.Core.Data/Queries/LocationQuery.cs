using EncryptedSQLite;
using Locations.Core.Shared.ViewModels;
namespace  Locations.Core.Data.Queries
{
    public class LocationQuery<T> : QueryBase<T> where T : LocationViewModel, new()
    {
        /// <summary>
        /// This constructor will force the use of the encrypted database.  
        /// </summary>
        /// <param name="alertServ"></param>
        /// <param name="loggerService"></param>
        /// <param name="email"></param>
        public LocationQuery(string email, string guid) :base()
        {
            SettingsQuery<SettingViewModel> settings = new SettingsQuery<SettingViewModel>();
            

            if (string.IsNullOrEmpty(email))
            {
                //LoggerService.LogWarning($"Email is not set.  Cannot use encrypted database.");
                throw new ArgumentException("Email is not set.  Cannot use encrypted database.");
            }
            dataB = DataEncrypted.GetAsyncConnection(KEY);

        }
        public LocationQuery() : base()
        { }
       
        public T GetItem<T>(double latitude, double longitude)
            {
            return (T)Convert.ChangeType(dataB.Table<LocationViewModel>().Where(x => x.Lattitude == latitude && x.Longitude == longitude).FirstOrDefaultAsync().Result, typeof(T));
        }
        public override T GetItem<T>(int id)
        {
            return (T)Convert.ChangeType(dataB.Table<LocationViewModel>().Where( x => x.Id == id).FirstOrDefaultAsync().Result, typeof(T));
        }

        public override string GetValueByString<T>(string name)
        {
            return string.Empty;
        }

        public override IList<T> GetItems<T>()
        {
            IList<T> results = (IList<T>)dataB.Table<LocationViewModel>().Where(x => x.Id == x.Id).ToListAsync().Result;
            return results;
        }

        public override T GetItemByString<T>(string name)
        {
            throw new NotImplementedException();
        }
    }
}
