using Microsoft.Maui.Devices.Sensors;
using System;
using System.Threading.Tasks;
using Location.Core.Helpers.LoggingService;

namespace Locations.Core.Shared.ViewModelServices
{
    /// <summary>
    /// Service for handling geolocation operations
    /// </summary>
    public class GeolocationService : IGeolocationService
    {
        private readonly ILoggerService? _loggerService;
        private GeolocationAccuracy _currentAccuracy = GeolocationAccuracy.Medium;
        private bool _isListening = false;
        private CancellationTokenSource? _cts;

        /// <summary>
        /// Event raised when the device location changes
        /// </summary>
        public event EventHandler<LocationChangedEventArgs>? LocationChanged;

        /// <summary>
        /// Event for error notification
        /// </summary>
        public event OperationErrorEventHandler? ErrorOccurred;

        /// <summary>
        /// Constructor with optional logger
        /// </summary>
        public GeolocationService(ILoggerService? loggerService = null)
        {
            _loggerService = loggerService;

            // Subscribe to platform's location changes when the service is created
            Microsoft.Maui.Devices.Sensors.Geolocation.LocationChanged += OnPlatformLocationChanged;
        }

        /// <summary>
        /// Starts tracking the device location
        /// </summary>
        public async Task<OperationResult<bool>> StartTrackingAsync(GeolocationAccuracy accuracy = GeolocationAccuracy.Medium)
        {
            try
            {
                if (_isListening)
                {
                    _loggerService?.LogInformation("Location tracking is already active");
                    return OperationResult<bool>.Success(true);
                }

                // Check permissions
                var permissionStatus = await CheckPermissionsAsync();
                if (!permissionStatus)
                {
                    permissionStatus = await RequestPermissionsAsync();
                    if (!permissionStatus)
                    {
                        string message = "Location permission was denied";
                        _loggerService?.LogWarning(message);
                        OnErrorOccurred(new OperationErrorEventArgs(
                            OperationErrorSource.Permission,
                            message));
                        return OperationResult<bool>.Failure(
                            OperationErrorSource.Permission,
                            message);
                    }
                }

                // Check if location services are enabled
                var locationEnabled = await IsLocationEnabledAsync();
                if (!locationEnabled)
                {
                    string message = "Location services are disabled on the device";
                    _loggerService?.LogWarning(message);
                    OnErrorOccurred(new OperationErrorEventArgs(
                        OperationErrorSource.Permission,
                        message));
                    return OperationResult<bool>.Failure(
                        OperationErrorSource.Permission,
                        message);
                }

                // Convert our accuracy enum to platform accuracy
                Microsoft.Maui.Devices.Sensors.GeolocationAccuracy platformAccuracy = ConvertToPlatformAccuracy(accuracy);
                _currentAccuracy = accuracy;

                // Create a cancellation token source for stopping listening later
                _cts = new CancellationTokenSource();

                // Start polling for location updates
                _isListening = true;
                _loggerService?.LogInformation($"Starting location tracking with accuracy: {accuracy}");

                // Start a background task to poll for location updates
                _ = Task.Run(async () =>
                {
                    try
                    {
                        while (!_cts.Token.IsCancellationRequested)
                        {
                            var request = new GeolocationRequest(platformAccuracy, TimeSpan.FromSeconds(2));
                            var location = await Microsoft.Maui.Devices.Sensors.Geolocation.GetLocationAsync(request, _cts.Token);

                            if (location != null)
                            {
                                // Manually trigger the location changed event
                                var args = new Microsoft.Maui.Devices.Sensors.GeolocationLocationChangedEventArgs(location);
                                OnPlatformLocationChanged(this, args);
                            }

                            // Wait before polling again
                            await Task.Delay(5000, _cts.Token); // Poll every 5 seconds
                        }
                    }
                    catch (TaskCanceledException)
                    {
                        // This is expected when stopping
                    }
                    catch (Exception ex)
                    {
                        _loggerService?.LogError($"Error in location polling: {ex.Message}", ex);
                    }
                });

                return OperationResult<bool>.Success(true);
            }
            catch (FeatureNotSupportedException ex)
            {
                string message = "Geolocation is not supported on this device";
                _loggerService?.LogError(message, ex);
                OnErrorOccurred(new OperationErrorEventArgs(
                    OperationErrorSource.Unknown,
                    message,
                    ex));
                return OperationResult<bool>.Failure(
                    OperationErrorSource.Unknown,
                    message,
                    ex);
            }
            catch (FeatureNotEnabledException ex)
            {
                string message = "Location services are not enabled on this device";
                _loggerService?.LogError(message, ex);
                OnErrorOccurred(new OperationErrorEventArgs(
                    OperationErrorSource.Permission,
                    message,
                    ex));
                return OperationResult<bool>.Failure(
                    OperationErrorSource.Permission,
                    message,
                    ex);
            }
            catch (PermissionException ex)
            {
                string message = "Location permission was denied";
                _loggerService?.LogError(message, ex);
                OnErrorOccurred(new OperationErrorEventArgs(
                    OperationErrorSource.Permission,
                    message,
                    ex));
                return OperationResult<bool>.Failure(
                    OperationErrorSource.Permission,
                    message,
                    ex);
            }
            catch (Exception ex)
            {
                string message = $"Failed to start location tracking: {ex.Message}";
                _loggerService?.LogError(message, ex);
                OnErrorOccurred(new OperationErrorEventArgs(
                    OperationErrorSource.Unknown,
                    message,
                    ex));
                return OperationResult<bool>.Failure(
                    OperationErrorSource.Unknown,
                    message,
                    ex);
            }
        }

        /// <summary>
        /// Stops tracking the device location
        /// </summary>
        public async Task StopTrackingAsync()
        {
            try
            {
                if (!_isListening)
                {
                    _loggerService?.LogInformation("Location tracking is not active");
                    return;
                }

                _loggerService?.LogInformation("Stopping location tracking");

                // Cancel the polling task
                if (_cts != null && !_cts.IsCancellationRequested)
                {
                    _cts.Cancel();
                    _cts.Dispose();
                    _cts = null;
                }

                _isListening = false;
                _loggerService?.LogInformation("Location tracking stopped successfully");

                await Task.CompletedTask; // To keep the method async
            }
            catch (Exception ex)
            {
                string message = $"Failed to stop location tracking: {ex.Message}";
                _loggerService?.LogError(message, ex);
                OnErrorOccurred(new OperationErrorEventArgs(
                    OperationErrorSource.Unknown,
                    message,
                    ex));
            }
        }

        /// <summary>
        /// Gets the current location of the device
        /// </summary>
        public async Task<OperationResult<LocationData>> GetCurrentLocationAsync()
        {
            try
            {
                // Check permissions
                var permissionStatus = await CheckPermissionsAsync();
                if (!permissionStatus)
                {
                    permissionStatus = await RequestPermissionsAsync();
                    if (!permissionStatus)
                    {
                        string message = "Location permission was denied";
                        _loggerService?.LogWarning(message);
                        OnErrorOccurred(new OperationErrorEventArgs(
                            OperationErrorSource.Permission,
                            message));
                        return OperationResult<LocationData>.Failure(
                            OperationErrorSource.Permission,
                            message);
                    }
                }

                // Get location
                Microsoft.Maui.Devices.Sensors.GeolocationAccuracy platformAccuracy = ConvertToPlatformAccuracy(_currentAccuracy);
                var request = new GeolocationRequest(platformAccuracy);
                var location = await Microsoft.Maui.Devices.Sensors.Geolocation.GetLocationAsync(request);

                if (location == null)
                {
                    string message = "Could not get current location";
                    _loggerService?.LogWarning(message);
                    OnErrorOccurred(new OperationErrorEventArgs(
                        OperationErrorSource.Unknown,
                        message));
                    return OperationResult<LocationData>.Failure(
                        OperationErrorSource.Unknown,
                        message);
                }

                var locationData = new LocationData(
                    location.Latitude,
                    location.Longitude,
                    location.Altitude,
                    location.Accuracy,
                    location.Timestamp);

                _loggerService?.LogInformation($"Got current location: {location.Latitude}, {location.Longitude}");
                return OperationResult<LocationData>.Success(locationData);
            }
            catch (FeatureNotSupportedException ex)
            {
                string message = "Geolocation is not supported on this device";
                _loggerService?.LogError(message, ex);
                OnErrorOccurred(new OperationErrorEventArgs(
                    OperationErrorSource.Unknown,
                    message,
                    ex));
                return OperationResult<LocationData>.Failure(
                    OperationErrorSource.Unknown,
                    message,
                    ex);
            }
            catch (FeatureNotEnabledException ex)
            {
                string message = "Location services are not enabled on this device";
                _loggerService?.LogError(message, ex);
                OnErrorOccurred(new OperationErrorEventArgs(
                    OperationErrorSource.Permission,
                    message,
                    ex));
                return OperationResult<LocationData>.Failure(
                    OperationErrorSource.Permission,
                    message,
                    ex);
            }
            catch (PermissionException ex)
            {
                string message = "Location permission was denied";
                _loggerService?.LogError(message, ex);
                OnErrorOccurred(new OperationErrorEventArgs(
                    OperationErrorSource.Permission,
                    message,
                    ex));
                return OperationResult<LocationData>.Failure(
                    OperationErrorSource.Permission,
                    message,
                    ex);
            }
            catch (Exception ex)
            {
                string message = $"Failed to get current location: {ex.Message}";
                _loggerService?.LogError(message, ex);
                OnErrorOccurred(new OperationErrorEventArgs(
                    OperationErrorSource.Unknown,
                    message,
                    ex));
                return OperationResult<LocationData>.Failure(
                    OperationErrorSource.Unknown,
                    message,
                    ex);
            }
        }

        /// <summary>
        /// Checks if location services are enabled on the device
        /// </summary>
        public async Task<bool> IsLocationEnabledAsync()
        {
            try
            {
                // Try to get the last known location with a short timeout
                try
                {
                    var request = new GeolocationRequest(
                        Microsoft.Maui.Devices.Sensors.GeolocationAccuracy.Lowest,
                        TimeSpan.FromSeconds(1));

                    using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
                    var location = await Microsoft.Maui.Devices.Sensors.Geolocation.GetLocationAsync(request, cts.Token);

                    // If we got a location, then services are enabled
                    return location != null;
                }
                catch (FeatureNotEnabledException)
                {
                    return false;
                }
                catch (FeatureNotSupportedException)
                {
                    return false;
                }
                catch (PermissionException)
                {
                    // This means we don't have permission, but the service might be enabled
                    return true;
                }
                catch (TaskCanceledException)
                {
                    // Timeout reached, assume services are enabled but slow
                    return true;
                }
            }
            catch (Exception ex)
            {
                string message = $"Error checking location services: {ex.Message}";
                _loggerService?.LogError(message, ex);
                OnErrorOccurred(new OperationErrorEventArgs(
                    OperationErrorSource.Unknown,
                    message,
                    ex));
                return false;
            }
        }

        /// <summary>
        /// Checks if the app has the necessary location permissions
        /// </summary>
        public async Task<bool> CheckPermissionsAsync()
        {
            try
            {
                var status = await Microsoft.Maui.ApplicationModel.Permissions.CheckStatusAsync<Microsoft.Maui.ApplicationModel.Permissions.LocationWhenInUse>();
                return status == Microsoft.Maui.ApplicationModel.PermissionStatus.Granted;
            }
            catch (Exception ex)
            {
                string message = $"Error checking location permissions: {ex.Message}";
                _loggerService?.LogError(message, ex);
                OnErrorOccurred(new OperationErrorEventArgs(
                    OperationErrorSource.Unknown,
                    message,
                    ex));
                return false;
            }
        }

        /// <summary>
        /// Requests location permissions from the user
        /// </summary>
        public async Task<bool> RequestPermissionsAsync()
        {
            try
            {
                var status = await Microsoft.Maui.ApplicationModel.Permissions.RequestAsync<Microsoft.Maui.ApplicationModel.Permissions.LocationWhenInUse>();
                return status == Microsoft.Maui.ApplicationModel.PermissionStatus.Granted;
            }
            catch (Exception ex)
            {
                string message = $"Error requesting location permissions: {ex.Message}";
                _loggerService?.LogError(message, ex);
                OnErrorOccurred(new OperationErrorEventArgs(
                    OperationErrorSource.Unknown,
                    message,
                    ex));
                return false;
            }
        }

        /// <summary>
        /// Converts our accuracy enum to the platform's accuracy enum
        /// </summary>
        private Microsoft.Maui.Devices.Sensors.GeolocationAccuracy ConvertToPlatformAccuracy(GeolocationAccuracy accuracy)
        {
            return accuracy switch
            {
                GeolocationAccuracy.High => Microsoft.Maui.Devices.Sensors.GeolocationAccuracy.Best,
                GeolocationAccuracy.Medium => Microsoft.Maui.Devices.Sensors.GeolocationAccuracy.Medium,
                GeolocationAccuracy.Low => Microsoft.Maui.Devices.Sensors.GeolocationAccuracy.Lowest,
                _ => Microsoft.Maui.Devices.Sensors.GeolocationAccuracy.Medium
            };
        }

        /// <summary>
        /// Handles platform location change events and forwards them
        /// </summary>
        private void OnPlatformLocationChanged(object? sender, Microsoft.Maui.Devices.Sensors.GeolocationLocationChangedEventArgs e)
        {
            var location = e.Location;

            var locationEventArgs = new LocationChangedEventArgs(
                location.Latitude,
                location.Longitude,
                location.Altitude,
                location.Accuracy,
                location.Timestamp);

            _loggerService?.LogDebug($"Location changed: {location.Latitude}, {location.Longitude}");

            // Forward the event with our custom event args
            OnLocationChanged(locationEventArgs);
        }

        /// <summary>
        /// Raises the LocationChanged event
        /// </summary>
        protected virtual void OnLocationChanged(LocationChangedEventArgs e)
        {
            LocationChanged?.Invoke(this, e);
        }

        /// <summary>
        /// Raises the ErrorOccurred event
        /// </summary>
        protected virtual void OnErrorOccurred(OperationErrorEventArgs e)
        {
            ErrorOccurred?.Invoke(this, e);
        }
    }
}