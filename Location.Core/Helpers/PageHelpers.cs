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
using Location.Core.Resources;
using Locations.Core.Shared.ViewModels;

namespace Location.Core.Helpers
{
    public class PageHelpers
    {
        public static void CheckVisit(string pageName, PageEnums pageEnum, SettingsService settingsService, INavigation navigation)
        {
            //CheckSubscription(pageEnum, settingsService, navigation);
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

            List<PageEnums> free = new() { PageEnums.AddLocation, PageEnums.ListLocations, PageEnums.Tips, PageEnums.Settings, PageEnums.WeatherDisplay };
            List<PageEnums> premium = new() { PageEnums.ExposureCalculator, PageEnums.LightMeter, PageEnums.SunLocation };
            List<PageEnums> pro = new List<PageEnums>() { PageEnums.SceneEvaluation, PageEnums.SunCalculations, PageEnums.WeatherDisplay };

            premium.AddRange(pro);
            premium.AddRange(free);

            pro.AddRange(free);

            //DateTime lastAd = GetAdTime(pageEnums);
            var lastAd = DateTime.Now;
            var hours = settingService.GetSettingByName(MagicStrings.AdGivesHours).Value;
            var isAdSupported = settingService.GetSettingByName(MagicStrings.FreePremiumAdSupported).Value;

            if (!Convert.ToBoolean(isAdSupported))
            {

                if (sub.Value == SubscriptionType.SubscriptionTypeEnum.Premium.Name())
                {
                    if (!premium.Contains(pageEnums))
                    {

                        if (lastAd < DateTime.Now.AddHours(Convert.ToInt16(hours)))
                        {
                            navigation.PushModalAsync(new SubscriptionOrAdFeature(pageEnums));
                        }
                    }

                }
                else if (sub.Value == SubscriptionTypeEnum.Free.Name())
                {
                    if (!free.Contains(pageEnums))
                    {

                        if (lastAd < DateTime.Now.AddHours(Convert.ToInt16(hours)))
                        {
                            navigation.PushModalAsync(new SubscriptionOrAdFeature(pageEnums));
                        }
                    }

                }
            }
            else
            {
                ShowAD(pageEnums, navigation);
            }

        }

        private static DateTime GetAdTime(PageEnums page)
        {
            SettingsService ss = new SettingsService();
            SettingViewModel value = new SettingViewModel(); 
            switch (page)
            {
                case PageEnums.WeatherDisplay:
                    value = ss.GetSettingByName(MagicStrings.WeatherDisplayAdViewed_TimeStamp);
                    break;
                case PageEnums.ExposureCalculator:
                    value = ss.GetSettingByName(MagicStrings.ExposureCalcAdViewed_TimeStamp);
                    break;
                case PageEnums.LightMeter:
                    value = ss.GetSettingByName(MagicStrings.LightMeterAdViewed_TimeStamp);
                    break;
                case PageEnums.SunLocation:
                    value = ss.GetSettingByName(MagicStrings.SunLocationAdViewed_TimeStamp);
                    break;
                case PageEnums.SceneEvaluation:
                    value = ss.GetSettingByName(MagicStrings.SceneEvaluationAdViewed_TimeStamp);
                    break;
                case PageEnums.SunCalculations:
                    value = ss.GetSettingByName(MagicStrings.SunCalculatorViewed_TimeStamp);
                    break;

                default:
                    value.Value = DateTime.Now.ToString();
                    break;
            }
            // return DateTime.Parse(value.Value);
            return DateTime.Now;
        }

        internal static void ShowAD(PageEnums page, INavigation navigation)
        {
            SettingsService ss = new SettingsService();
            SettingViewModel value = new SettingViewModel();
            switch (page)
            {
                case PageEnums.WeatherDisplay:
                    value = ss.GetSettingByName(MagicStrings.WeatherDisplayAdViewed_TimeStamp);
                    break;
                case PageEnums.ExposureCalculator:
                    value = ss.GetSettingByName(MagicStrings.ExposureCalcAdViewed_TimeStamp);
                    break;
                case PageEnums.LightMeter:
                    value = ss.GetSettingByName(MagicStrings.LightMeterAdViewed_TimeStamp);
                    break;
                case PageEnums.SunLocation:
                    value = ss.GetSettingByName(MagicStrings.SunLocationAdViewed_TimeStamp);
                    break;
                case PageEnums.SceneEvaluation:
                    value = ss.GetSettingByName(MagicStrings.SceneEvaluationAdViewed_TimeStamp);
                    break;
                case PageEnums.SunCalculations:
                    value = ss.GetSettingByName(MagicStrings.SunCalculatorViewed_TimeStamp);
                    break;

                default:

                    break;
            }
        }
    }
}
