using Locations.Core.Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locations.Core.Shared.Helpers
{
    public static class ViewModelHelpers
    {
        public static T ConvertType<T>(this T item, Type type)
        {
            return (T)Convert.ChangeType(item, typeof(T));
        }

        public static string GetValue(this SettingViewModel item)
        {
            return item.Value;
        }
    }
}
