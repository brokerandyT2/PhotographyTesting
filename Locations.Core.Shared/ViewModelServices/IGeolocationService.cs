using Locations.Core.Shared.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locations.Core.Shared.ViewModelServices
{
    /// <summary>
    /// Event arguments for location changes
    /// </summary>
    public class LocationChangedEventArgs : EventArgs
    {
        /// <summary>
        /// The current latitude
        /// </summary>
        public double Latitude { get; }

        /// <summary>
        /// The current longitude
        /// </summary>
        public double Longitude { get; }

        /// <summary>
        /// The altitude in meters, if available
        /// </summary>
        public double? Altitude { get; }

        /// <summary>
        /// The accuracy of the location in meters
        /// </summary>
        public double? Accuracy { get; }

        /// <summary>
        /// The timestamp when this location was determined
        /// </summary>
        public DateTimeOffset Timestamp { get; }

        public LocationChangedEventArgs(
            double latitude,
            double longitude,
            double? altitude = null,
            double? accuracy = null,
            DateTimeOffset? timestamp = null)
        {
            Latitude = latitude;
            Longitude = longitude;
            Altitude = altitude;
            Accuracy = accuracy;
            Timestamp = timestamp ?? DateTimeOffset.Now;
        }
    }

    /// <summary>
    /// Accuracy level for location tracking
    /// </summary>
    public enum GeolocationAccuracy
    {
        /// <summary>
        /// Lowest accuracy, uses minimal power
        /// </summary>
        Low,

        /// <summary>
        /// Medium accuracy, balanced power usage
        /// </summary>
        Medium,

        /// <summary>
        /// Highest accuracy, uses more power
        /// </summary>
        High
    }

    /// <summary>
    /// Location data class
    /// </summary>
    public class LocationData
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double? Altitude { get; set; }
        public double? Accuracy { get; set; }
        public DateTimeOffset Timestamp { get; set; }

        public LocationData(
            double latitude,
            double longitude,
            double? altitude = null,
            double? accuracy = null,
            DateTimeOffset? timestamp = null)
        {
            Latitude = latitude;
            Longitude = longitude;
            Altitude = altitude;
            Accuracy = accuracy;
            Timestamp = timestamp ?? DateTimeOffset.Now;
        }
    }

    public interface IGeolocationService
    {
        Action<object, object> ErrorOccurred { get; set; }

        /// <summary>
        /// Event raised when the device location changes
        /// </summary>
        event EventHandler<LocationChangedEventArgs> LocationChanged;

        /// <summary>
        /// Event for error notification
        /// </summary>
        //event OperationErrorEventHandler ErrorOccurred;

        /// <summary>
        /// Starts tracking the device location
        /// </summary>
        /// <param name="accuracy">The desired accuracy (High, Medium, Low)</param>
        /// <returns>Task representing the asynchronous operation</returns>
        Task<OperationResult<bool>> StartTrackingAsync(GeolocationAccuracy accuracy = GeolocationAccuracy.Medium);

        /// <summary>
        /// Stops tracking the device location
        /// </summary>
        /// <returns>Task representing the asynchronous operation</returns>
        Task StopTrackingAsync();

        /// <summary>
        /// Gets the current location of the device
        /// </summary>
        /// <returns>Result containing the current location</returns>
        Task<OperationResult<LocationData>> GetCurrentLocationAsync();

        /// <summary>
        /// Checks if location services are enabled on the device
        /// </summary>
        /// <returns>True if enabled, false otherwise</returns>
        Task<bool> IsLocationEnabledAsync();

        /// <summary>
        /// Checks if the app has the necessary location permissions
        /// </summary>
        /// <returns>True if permissions are granted, false otherwise</returns>
        Task<bool> CheckPermissionsAsync();

        /// <summary>
        /// Requests location permissions from the user
        /// </summary>
        /// <returns>True if permissions are granted, false otherwise</returns>
        Task<bool> RequestPermissionsAsync();
    }
}