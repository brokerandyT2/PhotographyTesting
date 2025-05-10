using Location.Core.Views;
using Locations.Core.Business.DataAccess.Interfaces;
using Locations.Core.Shared;
using Locations.Core.Shared.Enums;
using Locations.Core.Shared.ViewModels;
using static Locations.Core.Shared.Enums.SubscriptionType;

namespace Location.Core.Helpers
{
    [Obsolete("This class really isn't obsolete.  This is just here so you can remember that there is a Base.ContentPageBase in every UI project.")]
    public class PageHelpers
    {
        [Obsolete("This class really isn't obsolete.  This is just here so you can remember that there is a Base.ContentPageBase in every UI project.")]
        public static void CheckVisit(string pageName, PageEnums pageEnum, ISettingService<SettingViewModel> settingsService, INavigation navigation)
        {
            //CheckSubscription(pageEnum, settingsService, navigation);

            // Get setting by name instead of trying to convert to an ID
            var page = settingsService.GetSettingByName(pageName);
            bool _result;
            bool.TryParse(page.Value, out _result);
            if (_result == false)
            {
                navigation.PushModalAsync(new PageTutorialModal(pageEnum));
                page.Value = MagicStrings.True_string;

                // Save the setting back to the database
                Task.Run(async () => await settingsService.UpdateAsync(page)).Wait();
            }
        }

        public static void CheckSubscription(PageEnums pageEnums, ISettingService<SettingViewModel> settingService, INavigation navigation)
        {
            var sub = settingService.GetSettingByName(MagicStrings.SubscriptionType);
            var exp = settingService.GetSettingByName(MagicStrings.SubscriptionExpiration);

            List<PageEnums> free = new() { PageEnums.AddLocation, PageEnums.ListLocations, PageEnums.Tips, PageEnums.Settings, PageEnums.WeatherDisplay };
            List<PageEnums> premium = new() { PageEnums.ExposureCalculator, PageEnums.LightMeter, PageEnums.SunLocation };
            List<PageEnums> pro = new List<PageEnums>() { PageEnums.SceneEvaluation, PageEnums.SunCalculations, PageEnums.WeatherDisplay };

            premium.AddRange(pro);
            premium.AddRange(free);

            pro.AddRange(free);

            var lastAd = GetAdTime(pageEnums, settingService);
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
                ShowAD(pageEnums, navigation, settingService);
            }
        }

        private static DateTime GetAdTime(PageEnums page, ISettingService<SettingViewModel> settingService)
        {
            SettingViewModel value = new SettingViewModel();

            switch (page)
            {
                case PageEnums.WeatherDisplay:
                    value = settingService.GetSettingByName(MagicStrings.WeatherDisplayAdViewed_TimeStamp);
                    break;
                case PageEnums.ExposureCalculator:
                    value = settingService.GetSettingByName(MagicStrings.ExposureCalcAdViewed_TimeStamp);
                    break;
                case PageEnums.LightMeter:
                    value = settingService.GetSettingByName(MagicStrings.LightMeterAdViewed_TimeStamp);
                    break;
                case PageEnums.SunLocation:
                    value = settingService.GetSettingByName(MagicStrings.SunLocationAdViewed_TimeStamp);
                    break;
                case PageEnums.SceneEvaluation:
                    value = settingService.GetSettingByName(MagicStrings.SceneEvaluationAdViewed_TimeStamp);
                    break;
                case PageEnums.SunCalculations:
                    value = settingService.GetSettingByName(MagicStrings.SunCalculatorViewed_TimeStamp);
                    break;
                default:
                    value.Value = DateTime.Now.ToString();
                    break;
            }

            if (string.IsNullOrEmpty(value.Value))
                return DateTime.MinValue;

            return DateTime.Parse(value.Value);
        }

        internal static void ShowAD(PageEnums page, INavigation navigation, ISettingService<SettingViewModel> settingService)
        {
            SettingViewModel value = new SettingViewModel();

            switch (page)
            {
                case PageEnums.WeatherDisplay:
                    value = settingService.GetSettingByName(MagicStrings.WeatherDisplayAdViewed_TimeStamp);
                    break;
                case PageEnums.ExposureCalculator:
                    value = settingService.GetSettingByName(MagicStrings.ExposureCalcAdViewed_TimeStamp);
                    break;
                case PageEnums.LightMeter:
                    value = settingService.GetSettingByName(MagicStrings.LightMeterAdViewed_TimeStamp);
                    break;
                case PageEnums.SunLocation:
                    value = settingService.GetSettingByName(MagicStrings.SunLocationAdViewed_TimeStamp);
                    break;
                case PageEnums.SceneEvaluation:
                    value = settingService.GetSettingByName(MagicStrings.SceneEvaluationAdViewed_TimeStamp);
                    break;
                case PageEnums.SunCalculations:
                    value = settingService.GetSettingByName(MagicStrings.SunCalculatorViewed_TimeStamp);
                    break;
                default:
                    break;
            }

            // Set current timestamp and save it back
            value.Value = DateTime.Now.ToString();
            Task.Run(async () => await settingService.UpdateAsync(value)).Wait();

            // Code to actually show the ad would go here
            // This might involve showing an AdMob interstitial or banner ad
        }
    }
}