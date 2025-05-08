using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Locations.Core.Shared.DTO;
using Locations.Core.Shared.ViewModelServices;
using Locations.Core.Shared.ViewModels.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Locations.Core.Shared.ViewModels
{
    public class LocationViewModel : LocationDTO, ILocationViewModel, IValidatable
    {
        // Services
        private readonly IMediaService? _mediaService;
        private readonly IGeolocationService? _geolocationService;

        // Events for error propagation
        public event EventHandler<OperationErrorEventArgs>? ErrorOccurred;

        // Additional ViewModel-specific properties
        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                _isBusy = value;
                PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(nameof(IsBusy)));
            }
        }

        private string _alertTitle = string.Empty;
        public string AlertTitle
        {
            get => _alertTitle;
            set
            {
                _alertTitle = value;
                PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(nameof(AlertTitle)));
            }
        }

        private string _alertMessage = string.Empty;
        public string AlertMessage
        {
            get => _alertMessage;
            set
            {
                _alertMessage = value;
                PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(nameof(AlertMessage)));
            }
        }

        // Commands
        public ICommand TakePhotoCommand { get; }
        public ICommand SaveCommand { get; }

        /// <summary>
        /// Constructor for design-time and when created by SQLite
        /// </summary>
        public LocationViewModel()
        {
            // Initialize default values
            DateFormat = "MM/dd/yyyy hh:mm tt";
            City = string.Empty;
            State = string.Empty;

            // Initialize commands
            TakePhotoCommand = new AsyncRelayCommand(TakePhotoAsync);
            SaveCommand = new AsyncRelayCommand(SaveAsync);
        }

        /// <summary>
        /// Constructor with dependencies for runtime use
        /// </summary>
        public LocationViewModel(
            IMediaService mediaService,
            IGeolocationService geolocationService) : this()
        {
            _mediaService = mediaService;
            _geolocationService = geolocationService;

            // Subscribe to service error events
            if (_mediaService != null)
                _mediaService.ErrorOccurred += OnServiceErrorOccurred;

            if (_geolocationService != null)
                _geolocationService.ErrorOccurred += OnServiceErrorOccurred;

            // Start location tracking
            StartGPSAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Handle errors from services
        /// </summary>
        private void OnServiceErrorOccurred(object sender, OperationErrorEventArgs e)
        {
            AlertTitle = "Error";
            AlertMessage = e.Message;

            // Bubble up the error
            OnErrorOccurred(e);
        }

        /// <summary>
        /// Raise error event
        /// </summary>
        protected virtual void OnErrorOccurred(OperationErrorEventArgs e)
        {
            ErrorOccurred?.Invoke(this, e);
        }

        /// <summary>
        /// Start tracking device location asynchronously
        /// </summary>
        private async Task StartGPSAsync()
        {
            try
            {
                if (_geolocationService == null)
                {
                    AlertTitle = "Service Error";
                    AlertMessage = "Location service is not available";
                    OnErrorOccurred(new OperationErrorEventArgs(
                        OperationErrorSource.Unknown,
                        "Location service is not available"));
                    return;
                }

                // Start tracking with medium accuracy
                var result = await _geolocationService.StartTrackingAsync(GeolocationAccuracy.Medium);

                if (!result.IsSuccess)
                {
                    AlertTitle = "Location Error";
                    AlertMessage = result.ErrorMessage ?? "Could not start location tracking";
                    OnErrorOccurred(new OperationErrorEventArgs(
                        result.Source,
                        result.ErrorMessage ?? "Could not start location tracking",
                        result.Exception));
                    return;
                }

                // Subscribe to location change events
                _geolocationService.LocationChanged += OnLocationChanged;
            }
            catch (Exception ex)
            {
                AlertTitle = "Error";
                AlertMessage = $"Failed to start GPS: {ex.Message}";
                OnErrorOccurred(new OperationErrorEventArgs(
                    OperationErrorSource.Unknown,
                    $"Failed to start GPS: {ex.Message}",
                    ex));
            }
        }

        /// <summary>
        /// Handle location updates
        /// </summary>
        private void OnLocationChanged(object? sender, LocationChangedEventArgs e)
        {
            // Update view model properties with location data
            Lattitude = Math.Round(e.Latitude, 4);
            Longitude = Math.Round(e.Longitude, 4);

            // Update timestamp when location changes
            Timestamp = e.Timestamp.DateTime;
        }

        /// <summary>
        /// Take a photo using the media service
        /// </summary>
        private async Task TakePhotoAsync()
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;
                AlertTitle = string.Empty;
                AlertMessage = string.Empty;

                if (_mediaService == null)
                {
                    AlertTitle = "Service Error";
                    AlertMessage = "Media service is not available";
                    OnErrorOccurred(new OperationErrorEventArgs(
                        OperationErrorSource.Unknown,
                        "Media service is not available"));
                    return;
                }

                var result = await _mediaService.CapturePhotoAsync();

                if (result.IsSuccess && result.Data != null)
                {
                    Photo = result.Data;
                }
                else
                {
                    AlertTitle = "Photo Error";
                    AlertMessage = result.ErrorMessage ?? "Failed to take photo";
                    OnErrorOccurred(new OperationErrorEventArgs(
                        result.Source,
                        result.ErrorMessage ?? "Failed to take photo",
                        result.Exception));
                }
            }
            catch (Exception ex)
            {
                AlertTitle = "Error";
                AlertMessage = $"Error taking photo: {ex.Message}";
                OnErrorOccurred(new OperationErrorEventArgs(
                    OperationErrorSource.Unknown,
                    $"Error taking photo: {ex.Message}",
                    ex));
            }
            finally
            {
                IsBusy = false;
            }
        }

        /// <summary>
        /// Save the location
        /// </summary>
        private async Task SaveAsync()
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;
                AlertTitle = string.Empty;
                AlertMessage = string.Empty;

                // Perform validation
                if (!Validate(out List<string> errors))
                {
                    AlertTitle = "Validation Error";
                    AlertMessage = string.Join(", ", errors);
                    OnErrorOccurred(new OperationErrorEventArgs(
                        OperationErrorSource.ModelValidation,
                        AlertMessage));
                    return;
                }

                // Typically this would call your business service to save the location
                // For example:
                // var result = await _locationService.SaveLocationAsync(this);

                // For now, simulate a successful save
                await Task.Delay(500); // Simulate network call

                AlertTitle = "Success";
                AlertMessage = "Location saved successfully";
            }
            catch (Exception ex)
            {
                AlertTitle = "Error";
                AlertMessage = $"Error saving location: {ex.Message}";
                OnErrorOccurred(new OperationErrorEventArgs(
                    OperationErrorSource.Unknown,
                    $"Error saving location: {ex.Message}",
                    ex));
            }
            finally
            {
                IsBusy = false;
            }
        }

        /// <summary>
        /// Validate the location data
        /// </summary>
        public bool Validate(out List<string> errors)
        {
            errors = new List<string>();

            // Check required fields
            if (string.IsNullOrWhiteSpace(Title))
                errors.Add("Title is required");

            // Validate coordinates
            if (Lattitude < -90 || Lattitude > 90)
                errors.Add("Latitude must be between -90 and 90");

            if (Longitude < -180 || Longitude > 180)
                errors.Add("Longitude must be between -180 and 180");

            return errors.Count == 0;
        }
    }
}