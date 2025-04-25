using Location.Core.Views;
using Locations.Core.Business.DataAccess;
using Locations.Core.Shared;
using Locations.Core.Business.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Locations.Core.Shared.Enums;
using static Locations.Core.Shared.Enums.SubscriptionType;

namespace Location.Core.Helpers
{
    public class PageHelpers
    {
        public static void CheckVisit(string pageName, PageEnums pageEnum, SettingsService settingsService, INavigation navigation)
        {
            CheckSubscription(pageEnum, settingsService, navigation);
            var page = settingsService.GetSettingByName(pageName);
            if (!page.ToBoolean())
            {
                navigation.PushModalAsync(new PageTutorialModal(pageEnum));
                page.Value = MagicStrings.True_string;
                settingsService.Save(page);
            }
        }

        public static void CheckSubscription(PageEnums pageEnums, SettingsService settingService, INavigation navigation)
        {
            var sub = settingService.GetSettingByName(MagicStrings.SubscriptionType);
            var exp = settingService.GetSettingByName(MagicStrings.SubscriptionExpiration);

            List<PageEnums> free = new() { PageEnums.AddLocation, PageEnums.ListLocations, PageEnums.Tips, PageEnums.Settings };
            List<PageEnums> premium = new() { PageEnums.ExposureCalculator, PageEnums.LightMeter, PageEnums.SunLocation };
            List<PageEnums> pro = new List<PageEnums>() { PageEnums.SceneEvaluation, PageEnums.SunCalculations };
 
            premium.AddRange(pro);
            premium.AddRange(free);

            pro.AddRange(free);


            if (sub.Value == SubscriptionType.SubscriptionTypeEnum.Premium.Name())
            {
                if (!premium.Contains(pageEnums))
                {
                    navigation.PushModalAsync(new SubscriptionOrAdFeature(pageEnums));
                }

            }
            else if (sub.Value == SubscriptionTypeEnum.Free.Name())
            {
                if (!free.Contains(pageEnums))
                {
                    navigation.PushModalAsync(new SubscriptionOrAdFeature(pageEnums));
                }

            }


        }

        internal static void ShowAD(bool isAds, INavigation navigation)
        {
            if (isAds)
            {
                throw new NotImplementedException();
            }
        }
    }
}
