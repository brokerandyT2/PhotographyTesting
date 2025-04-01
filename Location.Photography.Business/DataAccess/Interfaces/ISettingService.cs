
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Locations.Core.Shared.ViewModels;

namespace Location.Photography.Business.DataAccess.Interfaces
{
    interface ISettingService<T> : IBaseService<T> where T : class, new()
    {
        public SettingViewModel GetSettingByName(string name);
        public SettingsViewModel GetAllSettings();
    }
}
