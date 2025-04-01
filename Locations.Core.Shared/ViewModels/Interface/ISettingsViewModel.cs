using Locations.Core.Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locations.Core.Shared.ViewModels.Interface
{
    public interface ISettingsViewModel
    {


        public SettingViewModel Hemisphere { get; set; }
        public SettingViewModel FirstName { get; set; }
        public SettingViewModel LastName { get; set; }
        public SettingViewModel Email { get; set; }
        public SettingViewModel SubscriptionExpiration { get; set; }
        public SettingViewModel UniqeID { get; set; }
        public SettingViewModel DeviceInfo { get; set; }

        public SettingViewModel TimeFormat { get; set; }
        public SettingViewModel DateFormat { get; set; }
        public SettingViewModel HomePageViewed { get; set; }
        public SettingViewModel ListLocationsViewed { get; set; }

        public SettingViewModel TipsViewed { get; set; }
        public SettingViewModel ExposureCalcViewed { get; set; }
        public SettingViewModel LightMeterViewed { get; set; }
        public SettingViewModel SceneEvaluationViewed { get; set; }
        public SettingViewModel LastBulkWeatherUpdate { get; set; }
        public SettingViewModel Language { get; set; }

    }
}
