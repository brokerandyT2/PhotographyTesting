using Locations.Core.Business.DataAccess;
using Locations.Core.Shared.Enums;
using Locations.Core.Shared.Helpers;
using Locations.Core.Shared.ViewModels;
using Locations.Core.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Locations.Core.Business
{
    public static class Constants
    {
#if PHOTOGRAPHY
        private static string DatabaseFilename = "photography.db3";
#endif
        public static string DatabasePath => Path.Combine(FileSystem.AppDataDirectory, DatabaseFilename);

        public const SQLite.SQLiteOpenFlags Flags =
            // open the database in read/write mode
            SQLite.SQLiteOpenFlags.ReadWrite |
            // create the database if it doesn't exist
            SQLite.SQLiteOpenFlags.Create |
            // enable multi-threaded database access
            SQLite.SQLiteOpenFlags.SharedCache;

        private static SettingsService ss = new SettingsService();
        public static string userSubscriptionType_string = ss.GetSettingByName(MagicStrings.SubscriptionType).Value;//.Value;

        public static string userSubscriptionExpiration_string = DateTime.Parse(ss.GetSettingByName(MagicStrings.SubscriptionExpiration).Value).ToString(ss.GetSettingByName(MagicStrings.TimeFormat).Value);
       
        public static string DefaultHemisphere = Hemisphere.HemisphereChoices.North.Name();
        public static string DateFormat_string => ss.GetSettingByName(MagicStrings.DateFormat).Value;
        public static string Time_Format_string => ss.GetSettingByName(MagicStrings.TimeFormat).Value;
        public static string SubscriptionType => ss.GetSettingByName(MagicStrings.SubscriptionType).Value;
        public static string FirstName => ss.GetSettingByName(MagicStrings.FirstName).Value;
        public static string LastName => ss.GetSettingByName(MagicStrings.LastName).Value;
        public static string TimeFormat_string => ss.GetSettingByName(MagicStrings.TimeFormat).Value;

        public static string DevInfo_string = Newtonsoft.Json.JsonConvert.SerializeObject(new DeviceInformation()).ToString();
        public static readonly string Weather_API_Key_string = ss.GetSettingByName(MagicStrings.Weather_API_Key).Value;
        public static readonly string WeatherURL_string = ss.GetSettingByName(MagicStrings.WeatherURL).Value;
        public static readonly string UniqueID_string = ss.GetSettingByName(MagicStrings.UniqueID).Value;
        public static readonly string LastBulkWeatherUpdate_string = ss.GetSettingByName(MagicStrings.LastBulkWeatherUpdate).Value;
        public static readonly string DeviceInformation_string = ss.GetSettingByName(MagicStrings.DeviceInformation).Value;
        public static readonly string Hemisphere_string = ss.GetSettingByName(MagicStrings.Hemisphere).Value;
        public static readonly string SubscriptionType_string = ss.GetSettingByName(MagicStrings.SubscriptionType).Value;

    }
}
