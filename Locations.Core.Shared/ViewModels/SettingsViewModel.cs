using Locations.Core.Shared.ViewModels.Interface;
using Locations.Core.Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Locations.Core.Shared.Enums;
using static Locations.Core.Shared.Enums.Hemisphere;
using System.ComponentModel;

namespace Locations.Core.Shared.ViewModels
{
    public class SettingsViewModel : ISettingsViewModel, INotifyPropertyChanged
    {

        public SettingsViewModel() { }
        /// <summary>
        /// True is North, False is South
        /// </summary>
        public SettingViewModel Hemisphere
        {
            get => _hemisphere;
            set
            {
                _hemisphere = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Hemisphere)));
            }
        }
        private SettingViewModel _hemisphere;
        public bool HemisphereNorth { get { return Hemisphere.Value == HemisphereChoices.North.Name() ? true : false; } }
        public SettingViewModel FirstName { get; set; }
        public SettingViewModel LastName { get; set; }
        public SettingViewModel Email { get; set; }
        public SettingViewModel SubscriptionExpiration
        {
            get => _subscriptionExpiration;
            set
            {
                _subscriptionExpiration = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SubscriptionExpiration)));
            }
        }
        private SettingViewModel _subscriptionExpiration;
        public SettingViewModel SubscriptionType
        {
            get => _subscriptionType;
            set
            {
                _subscriptionType = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SubscriptionType)));
            }
        }
        private SettingViewModel _subscriptionType;
        public SettingViewModel UniqeID { get; set; }
        private SettingViewModel _uniqeID;
        public SettingViewModel DeviceInfo { get; set; }
        private SettingViewModel _deviceInfo;
        public bool DateFormatToggle { get { return DateFormat.Value == MagicStrings.USDateFormat ? true : false; } }
        public SettingViewModel TimeFormat
        {
            get => _timeFormat;
            set
            {
                _timeFormat = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TimeFormat)));
            }
        }
        private SettingViewModel _timeFormat;
        public bool TimeFormatToggle { get { return TimeFormat.Value == MagicStrings.USTimeformat_Pattern ? true : false; } }
        public SettingViewModel DateFormat
        {
            get => _dateFormat;
            set
            {
                _dateFormat = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DateFormat)));
            }
        }
        private SettingViewModel _dateFormat;
        public SettingViewModel HomePageViewed { get; set; }
        public SettingViewModel ListLocationsViewed { get; set; }

        public SettingViewModel TipsViewed { get; set; }
        public SettingViewModel ExposureCalcViewed { get; set; }
        public SettingViewModel LightMeterViewed { get; set; }
        public SettingViewModel SceneEvaluationViewed { get; set; }
        public SettingViewModel LastBulkWeatherUpdate { get; set; }
        public SettingViewModel Language
        {
            get => _language;
            set
            {
                _language = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Language)));
            }
        }
        private SettingViewModel _language;
        public SettingViewModel AdSupport
        {
            get => _adSupport; 
            set
            {
                _adSupport = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AdSupport)));
            }
        }
        public bool AdSupportboolean { get {
                return AdSupport.Value == MagicStrings.True_string ? true : false;
            } }
        private SettingViewModel _adSupport;

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
