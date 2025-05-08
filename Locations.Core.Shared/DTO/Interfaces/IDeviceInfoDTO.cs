using System;

namespace Locations.Core.Shared.DTO.Interfaces
{
    public interface IDeviceInfoDTO : IDTOBase
    {
        string DeviceModel { get; }
        string DeviceManufacturer { get; }
        string DeviceName { get; }
        string DeviceVersion { get; }
        string DevicePlatform { get; }
        DateTime CurrentDateTime { get; }
        string DeviceType { get; }
        string DeviceIdiom { get; }
        bool IsAndroid { get; }
        bool IsiOS { get; }
        string BatteryChargeLevel { get; }
        string BatteryState { get; }
        string BatteryPowerSource { get; }
        string BatteryEnergySaverStatus { get; }
        bool HasFlashlight { get; }
        bool IsVibrationSupported { get; }
        bool IsHapticFeedbackSupported { get; }
        bool IsGeolocationListeningForeground { get; }
    }
}