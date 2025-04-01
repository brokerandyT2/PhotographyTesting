using Locations.Core.Shared.DTO.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locations.Core.Shared.DTO
{
    public class DeviceInfoDTO : IDeviceInfoDTO
    {
        public string DeviceModel
        {
            get => DeviceInfo.Current.Model;
        }
        public string DeviceManufacturer
        {
            get => DeviceInfo.Current.Manufacturer;
        }
        public string DeviceName
        {
            get => DeviceInfo.Current.Name;
        }
        public string DeviceVersion
        {
            get => DeviceInfo.Current.Version.ToString();
        }
        public string DevicePlatform
        {
            get => DeviceInfo.Current.Platform.ToString();
        }
        public DateTime CurrentDateTime = DateTime.Now;
        public string DeviceType { get => DeviceInfo.Current.DeviceType.ToString(); }
        public string DeviceIdiom { get => DeviceInfo.Current.Idiom.ToString(); }
        public bool IsAndroid { get; } = Device.RuntimePlatform == Device.Android;
        public bool IsiOS { get; } = Device.RuntimePlatform == Device.iOS;
        public string BatteryChargeLevel = Microsoft.Maui.Devices.Battery.ChargeLevel.ToString();
        public string BatteryState = Microsoft.Maui.Devices.Battery.State.ToString();
        public string BatteryPowerSource = Microsoft.Maui.Devices.Battery.PowerSource.ToString();
        public string BatteryEnergySaverStatus = Microsoft.Maui.Devices.Battery.EnergySaverStatus.ToString();
        public bool HasFlashlight = Microsoft.Maui.Devices.Flashlight.IsSupportedAsync().Result;
        public bool IsVibrationSupported = Microsoft.Maui.Devices.Vibration.Default.IsSupported;
        public bool IsHapticFeedbackSupported = Microsoft.Maui.Devices.HapticFeedback.Default.IsSupported;
        public bool IsGeolocationListeningForeground = Geolocation.Default.IsListeningForeground;
       // public string LastLocation = Geolocation.Default.GetLastKnownLocationAsync().Result.ToString();
        public DeviceInfoDTO()
        {

        }
    }
}
