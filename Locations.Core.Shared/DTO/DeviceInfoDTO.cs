using Locations.Core.Shared.DTO.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Controls;
using Microsoft.VisualBasic;
using System.Reflection;

namespace Locations.Core.Shared.DTO
{
    public partial class DeviceInfoDTO : DTOBase, IDeviceInfoDTO
    {
        // Original properties
        [ObservableProperty]
        private string _deviceModel;

        [ObservableProperty]
        private string _deviceManufacturer;

        [ObservableProperty]
        private string _deviceName;

        [ObservableProperty]
        private string _deviceVersion;

        [ObservableProperty]
        private string _devicePlatform;

        [ObservableProperty]
        private DateTime _currentDateTime = DateTime.Now;

        [ObservableProperty]
        private string _deviceType;

        [ObservableProperty]
        private string _deviceIdiom;

        [ObservableProperty]
        private bool _isAndroid;

        [ObservableProperty]
        private bool _isiOS;

        [ObservableProperty]
        private string _batteryChargeLevel;

        [ObservableProperty]
        private string _batteryState;

        [ObservableProperty]
        private string _batteryPowerSource;

        [ObservableProperty]
        private string _batteryEnergySaverStatus;

        [ObservableProperty]
        private bool _hasFlashlight;

        [ObservableProperty]
        private bool _isVibrationSupported;

        [ObservableProperty]
        private bool _isHapticFeedbackSupported;

        [ObservableProperty]
        private bool _isGeolocationListeningForeground;

        // Add the missing properties that are expected by other code
        [ObservableProperty]
        private string _deviceId;

        [ObservableProperty]
        private string _manufacturer;

        [ObservableProperty]
        private string _model;

        [ObservableProperty]
        private string _osVersion;

        [ObservableProperty]
        private string _appVersion;

        public DeviceInfoDTO()
        {
            // Initialize properties with device info
            DeviceModel = DeviceInfo.Current.Model;
            DeviceManufacturer = DeviceInfo.Current.Manufacturer;
            DeviceName = DeviceInfo.Current.Name;
            DeviceVersion = DeviceInfo.Current.Version.ToString();
            DevicePlatform = DeviceInfo.Current.Platform.ToString();
            DeviceType = DeviceInfo.Current.DeviceType.ToString();
            DeviceIdiom = DeviceInfo.Current.Idiom.ToString();
            IsAndroid = Device.RuntimePlatform == Device.Android;
            IsiOS = Device.RuntimePlatform == Device.iOS;
            BatteryChargeLevel = Microsoft.Maui.Devices.Battery.ChargeLevel.ToString();
            BatteryState = Microsoft.Maui.Devices.Battery.State.ToString();
            BatteryPowerSource = Microsoft.Maui.Devices.Battery.PowerSource.ToString();
            BatteryEnergySaverStatus = Microsoft.Maui.Devices.Battery.EnergySaverStatus.ToString();
            HasFlashlight = Microsoft.Maui.Devices.Flashlight.IsSupportedAsync().Result;
            IsVibrationSupported = Microsoft.Maui.Devices.Vibration.Default.IsSupported;
            IsHapticFeedbackSupported = Microsoft.Maui.Devices.HapticFeedback.Default.IsSupported;
            IsGeolocationListeningForeground = Geolocation.Default.IsListeningForeground;

           
        }

        public string OSVersion { get; set; }
    }
}