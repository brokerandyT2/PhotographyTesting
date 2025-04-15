using Location.Core.Views;
using Locations.Core.Business.DataAccess;
using Locations.Core.Shared;
using Locations.Core.Business.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Location.Core.Helpers
{
    public class PageHelpers
    {
        public static void CheckVisit(string pageName, PageEnums pageEnum, SettingsService settingsService, INavigation navigation)
        {

            var page = settingsService.GetSettingByName(pageName);
            if (!page.ToBoolean())
            {
                navigation.PushModalAsync(new PageTutorialModal(pageEnum));
                page.Value = MagicStrings.True_string;
                settingsService.Save(page);
            }
        }

        internal static void ShowAD(bool isAds, INavigation navigation)
        {
            throw new NotImplementedException();
        }
    }
}
