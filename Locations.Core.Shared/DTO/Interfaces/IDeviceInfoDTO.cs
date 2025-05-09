using System;

namespace Locations.Core.Shared.DTO.Interfaces
{
    public interface IDeviceInfoDTO
    {
        // Original properties from DeviceInfoDTO
        string DeviceModel { get; set; }
        string DeviceManufacturer { get; set; }
        string DeviceName { get; set; }
        string DeviceVersion { get; set; }
        string DevicePlatform { get; set; }
        DateTime CurrentDateTime { get; set; }
        string DeviceType { get; set; }
        string DeviceIdiom { get; set; }
        bool IsAndroid { get; set; }
        bool IsiOS { get; set; }
        string BatteryChargeLevel { get; set; }
        string BatteryState { get; set; }
        string BatteryPowerSource { get; set; }
        string BatteryEnergySaverStatus { get; set; }
        bool HasFlashlight { get; set; }
        bool IsVibrationSupported { get; set; }
        bool IsHapticFeedbackSupported { get; set; }
        bool IsGeolocationListeningForeground { get; set; }

        // Properties that appear to be expected elsewhere in the codebase
        string DeviceId { get; set; }
        string Manufacturer { get; set; }
        string Model { get; set; }
        string OSVersion { get; set; }
        string AppVersion { get; set; }
    }
}