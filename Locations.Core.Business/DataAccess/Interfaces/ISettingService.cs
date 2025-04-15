using Locations.Core.Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locations.Core.Business.DataAccess.Interfaces
{
    public interface ISettingService<T>: IBaseService<T> where T : class, new()
    {
        public SettingViewModel GetSettingByName(string name);
        public SettingsViewModel GetAllSettings();
    }
}
