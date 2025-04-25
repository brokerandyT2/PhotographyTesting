using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locations.Core.Shared
{
    public static class MagicStrings
    {
        public static string SunCalculatorViewed = "suncalculatorviewed";
        public static string SunLocationViewed = "suncalculatorviewed";
        public static string SceneEvaluationViewed = "sceneevaluationviewed";
        public static readonly string HomePageViewed = "homepageviewed";
        public static readonly string ExposureCalcViewed = "exposurecalcviewed";
        public static readonly string LightMeterViewed = "lightmeterviewed";
        public static readonly string DateFormat = "dateformat";
        public static readonly string USDateFormat = "MMM/dd/yyyy";
        public static readonly string InternationalFormat = "dd/MMM/yyyy";
        public static readonly string Hemisphere = "hemisphere";
        public static readonly string SubscriptionType = "subscription_type";
        public static readonly string SubscriptionExpiration = "subscription_expiration";
        public static readonly string Email = "Email";
        public static readonly string FirstName = "First_Name";
        public static readonly string LastName = "Last_Name";
        public static readonly string North = "north";
        public static readonly string South = "south";
        public static readonly string Free = "free";
        public static readonly string Pro = "pro";
        public static readonly string Premium = "premium";
        public static readonly string UniqueID = "uniqueID";
        public static readonly string DeviceInformation = "deviceinformation";
        public static readonly string LastBulkWeatherUpdate = "lastbulkweatherupdate";
        public static readonly string TimeFormat = "timeformat";
        public static readonly string Weather_API_Key = "weatherapikey";
        public static readonly string WeatherURL = "weatherurl";
        public static readonly string English_for_i8n = "en-US";
        public static readonly string DeviceInfo = "deviceinformation";
        public static readonly string False_string = false.ToString();
        public static readonly string True_string = true.ToString();
        public static readonly string TowardsWind = "towardsWind";
        public static readonly string WithWind = "withwind";
        public static readonly string CameraRefresh = "camerarefresh";

        public const string ExposureCalculator = "exposurecalculator";
        public static string LocationListViewed = "locationlistviewed";

        public static string TipsViewed = "tipsviewed";
        public static string DefaultLanguage = "defaultLanguage";
        public static string WindDirection = "winddirection";

        public static string AppOpenCounter = "appopencounter";
#if PHOTOGRAPHY
        public static string DataBasePath = Path.Combine(FileSystem.AppDataDirectory,"photography.db3");

        public static string USTimeformat_Pattern = "hh:mm tt";
        public static string InternationalTimeFormat_Pattern = "HH:mm";
        public static string FreePremiumAdSupported = "free_premium_ad_supported";

        public static string TemperatureType = "temperaturetype";
        public static string Fahrenheit = "F";
        public static string Celsius = "C";

        public static string AddLocationViewed = "addlocationviewed";

        public static string WeatherDisplayViewed = "weatherdisplayviewed";

        public static string SettingsViewed = "settingsviewed";

        public static string AdGivesHours = "hoursPerAd";
        public static string ExposureCalcAdViewed_TimeStamp = "exposureCalcAdViewedTimeStamp";
        public static string LightMeterAdViewed_TimeStamp = "lightmeterAdViewedTimeStamp";
        public static string SceneEvaluationAdViewed_TimeStamp = "SceneEvaluationAdViewedTimeStamp";
        public static string SunCalculatorViewed_TimeStamp = "SunCalculatorAdViewedTimeStamp";
        public static string SunLocationAdViewed_TimeStamp = "SunLocationAdViewedTimeStamp";
        public static string WeatherDisplayAdViewed_TimeStamp = "WeatherDisplayAdViewedTimeStamp";
        // public static string U
#endif
#if FISHING

#endif
#if CAMPING

#endif
    }
}
