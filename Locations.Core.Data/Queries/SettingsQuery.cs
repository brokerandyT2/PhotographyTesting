using  Locations.Core.Data.Queries.Interfaces;
using  Locations.Core.Shared.ViewModels;
using  Locations.Core.Shared.ViewModels.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using  Locations.Core.Shared.Helpers;
using Locations.Core.Shared.Customizations.Alerts.Interfraces;
using Locations.Core.Shared.Customizations.Logging.Interfaces;
namespace  Locations.Core.Data.Queries
{
    public class SettingsQuery<T> : QueryBase<T> where T : SettingViewModel, new()
    {
        public SettingsQuery(IAlertService alertSer, ILoggerService loggerServic) : base(alertSer, loggerServic)
        {
            
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
