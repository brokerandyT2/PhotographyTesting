using EncryptedSQLite;
using Locations.Core.Shared.ViewModels;

namespace Location.Photography.Data.Queries
{
    public class LocationsQuery<T> : QueryBase<T> where T : LocationViewModel, new()
    {
        SettingsQuery<SettingViewModel> settings;
        public LocationsQuery(string email):this()
        {

      
        }
        public LocationsQuery()
        {
            settings = new SettingsQuery<SettingViewModel>();

        }
        public override T GetItem<T>(int id)
        {
            throw new NotImplementedException();
        }

        public override IList<T> GetItems<T>()
        {
           return (IList<T>)dataAsync.Table<LocationViewModel>().Where(x=> x.IsDeleted == false).ToListAsync().Result;
        }

        public override string GetValueByString<T>(string name)
        {
            throw new NotImplementedException();
        }
    }
}
