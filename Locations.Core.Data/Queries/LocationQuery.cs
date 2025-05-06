using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using EncryptedSQLite;
using Locations.Core.Shared.Customizations.Alerts.Implementation;
using Locations.Core.Shared.Customizations.Alerts.Interfraces;
using Locations.Core.Shared.Customizations.Logging.Implementation;
using Locations.Core.Shared.Customizations.Logging.Interfaces;
using  Locations.Core.Shared.DTO;
using  Locations.Core.Shared.ViewModels;
using  Locations.Core.Shared.ViewModels.Interface;
using Microsoft.Maui.Platform;
using static System.Runtime.InteropServices.JavaScript.JSType;
using EncryptedSQLite;
using NormalSQLite;
using Locations.Core.Shared;
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
        public LocationQuery(IAlertService alertServ, ILoggerService loggerService, string email) : this(alertServ, loggerService)
        {
            SettingsQuery<SettingViewModel> settings = new SettingsQuery<SettingViewModel>(alertServ, loggerService);
            var addy = settings.GetItemByString<SettingViewModel>(MagicStrings.Email).Value;

            if (string.IsNullOrEmpty(addy))
            {
                loggerService.LogWarning($"Email is not set.  Cannot use encrypted database. Email Address {addy}");
                throw new ArgumentException("Email is not set.  Cannot use encrypted database.");
            }
            dataB = DataEncrypted.GetConnection(email);

        }
        /// <summary>
        /// This constructor will force the use of the unencrypted database.  This is the default constructor.
        /// </summary>
        /// <param name="alertServ"></param>
        /// <param name="loggerService"></param>
        public LocationQuery(IAlertService alertServ, ILoggerService loggerService) : base(alertServ, loggerService)
        {
            dataB = DataUnEncrypted.GetConnection();
        }
       
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
