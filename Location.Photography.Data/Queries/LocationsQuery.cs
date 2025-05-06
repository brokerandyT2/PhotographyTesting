using EncryptedSQLite;
using Location.Photography.Shared;
using Locations.Core.Shared.Customizations.Alerts.Implementation;
using Locations.Core.Shared.Customizations.Logging.Implementation;
using Locations.Core.Shared.ViewModels;
using NormalSQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Location.Photography.Data.Queries
{
    public class LocationsQuery<T> : QueryBase<T> where T : LocationViewModel, new()
    {

        public LocationsQuery(string email)
        {
            var x = new AlertService();
            var y = new LoggerService();
            SettingsQuery<SettingViewModel> settings = new SettingsQuery<SettingViewModel>(x,y);
            var addy = settings.GetItemByString<SettingViewModel>(Locations.Core.Shared.MagicStrings.Email).Value;

            if (string.IsNullOrEmpty(addy))
            {
                y.LogWarning($"Email is not set.  Cannot use encrypted database. Email Address {addy}");
                throw new ArgumentException("Email is not set.  Cannot use encrypted database.");
            }
            dataB = DataEncrypted.GetConnection(email);
        }
        public LocationsQuery()
        {
            dataB = DataUnEncrypted.GetConnection();
        }
        public override T GetItem<T>(int id)
        {
            throw new NotImplementedException();
        }

        public override IList<T> GetItems<T>()
        {
           return (IList<T>)dataB.Table<LocationViewModel>().Where(x=> x.IsDeleted == false).ToListAsync().Result;
        }

        public override string GetValueByString<T>(string name)
        {
            throw new NotImplementedException();
        }
    }
}
