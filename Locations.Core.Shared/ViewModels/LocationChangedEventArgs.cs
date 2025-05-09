using System;

namespace Locations.Core.Shared.ViewModels
{
    /// <summary>
    /// Event arguments for location changes
    /// </summary>
    public class LocationChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the latitude value
        /// </summary>
        public double Latitude { get; }

        /// <summary>
        /// Gets the longitude value
        /// </summary>
        public double Longitude { get; }

        /// <summary>
        /// Initializes a new instance of the LocationChangedEventArgs class
        /// </summary>
        /// <param name="latitude">The latitude</param>
        /// <param name="longitude">The longitude</param>
        public LocationChangedEventArgs(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }
    }
}