using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Locations.Core.Shared.DTO;
using Locations.Core.Shared.ViewModels.Interface;
using Locations.Core.Shared.ViewModelServices;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Locations.Core.Shared.ViewModels
{
    public partial class LocationViewModel : LocationDTO, ILocationViewModel, IValidatable
    {



        // Services injected via DI
        private readonly ILocationService? _locationService;
        private readonly IMediaService? _mediaService;
        private readonly IGeolocationService? _geolocationService;

        // Event for error handling
        public event EventHandler<OperationErrorEventArgs>? ErrorOccurred;

        // UI state properties with completely unique names
        private bool _vmIsBusy;
        public bool VmIsBusy
        {
            get => _vmIsBusy;
            set
            {
                _vmIsBusy = value;
                OnPropertyChanged(nameof(VmIsBusy));
            }
        }

        private string _vmErrorMessage = string.Empty;
        public string VmErrorMessage
        {
            get => _vmErrorMessage;
            set
            {
                _vmErrorMessage = value;
                OnPropertyChanged(nameof(VmErrorMessage));
            }
        }

        private bool _vmIsNewLocation = true;
        public bool VmIsNewLocation
        {
            get => _vmIsNewLocation;
            set
            {
                _vmIsNewLocation = value;
                OnPropertyChanged(nameof(VmIsNewLocation));
            }
        }

        private bool _vmIsLocationTracking = false;
        public bool VmIsLocationTracking
        {
            get => _vmIsLocationTracking;
            set
            {
                _vmIsLocationTracking = value;
                OnPropertyChanged(nameof(VmIsLocationTracking));
            }
        }

        // Commands for UI interactions
        public ICommand SaveCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand TakePhotoCommand { get; }
        public ICommand PickPhotoCommand { get; }
        public ICommand StartLocationTrackingCommand { get; }
        public ICommand StopLocationTrackingCommand { get; }

        /// <summary>
        /// Default constructor for design-time and when created by SQLite
        /// </summary>
        public LocationViewModel()
        {
            // Set reasonable defaults
            Timestamp = DateTime.Now;
            DateFormat = "MM/dd/yyyy";

            // Initialize commands
            SaveCommand = new AsyncRelayCommand(SaveAsync, () => !VmIsBusy);
            DeleteCommand = new AsyncRelayCommand(DeleteAsync, () => Id > 0 && !VmIsBusy);
            TakePhotoCommand = new AsyncRelayCommand(TakePhotoAsync, () => !VmIsBusy);
            PickPhotoCommand = new AsyncRelayCommand(PickPhotoAsync, () => !VmIsBusy);
            StartLocationTrackingCommand = new AsyncRelayCommand(StartLocationTrackingAsync, () => !VmIsLocationTracking);
            StopLocationTrackingCommand = new AsyncRelayCommand(StopLocationTrackingAsync, () => VmIsLocationTracking);
        }

        /// <summary>
        /// Constructor with DI for runtime use
        /// </summary>
        public LocationViewModel(
            ILocationService locationService,
            IMediaService mediaService,
            IGeolocationService geolocationService) : this()
        {
            _locationService = locationService;
            _mediaService = mediaService;
            _geolocationService = geolocationService;

            // Subscribe to service events for error handling
            // Cast the event handlers to match the expected delegate type
            if (_geolocationService != null)
                _geolocationService.ErrorOccurred += (sender, e) => OnServiceErrorOccurred(sender,
                    new OperationErrorEventArgs(
                        Locations.Core.Shared.ViewModels.OperationErrorSource.GeolocationService,
                        e.Message,
                        e.Exception));

            if (_mediaService != null)
                _mediaService.ErrorOccurred += (sender, e) => OnServiceErrorOccurred(sender,
                    new OperationErrorEventArgs(
                        Locations.Core.Shared.ViewModels.OperationErrorSource.MediaService,
                        e.Message,
                        e.Exception));
        }

        /// <summary>
        /// Initialize from an existing LocationDTO
        /// </summary>
        public void InitializeFromDTO(LocationDTO dto)
        {
            if (dto == null) return;

            // Copy properties from the DTO
            Id = dto.Id;
            City = dto.City;
            State = dto.State;
            Lattitude = dto.Lattitude;
            Longitude = dto.Longitude;
            Title = dto.Title;
            Description = dto.Description;
            Photo = dto.Photo;
            Timestamp = dto.Timestamp;
            IsDeleted = dto.IsDeleted;
            DateFormat = dto.DateFormat;

            // This is now an existing location
            VmIsNewLocation = false;
        }

        /// <summary>
        /// Save the location
        /// </summary>
        private async Task SaveAsync()
        {
            if (_locationService == null)
                return;

            try
            {
                VmIsBusy = true;
                VmErrorMessage = string.Empty;

                // Validate data first
                if (!Validate(out var errors))
                {
                    VmErrorMessage = string.Join(Environment.NewLine, errors);
                    return;
                }

                // Save based on whether this is a new or existing location
                OperationResult<LocationViewModel> result;

                if (VmIsNewLocation)
                {
                    // Using existing service methods with our ViewModel
                    // Implementation pending
                    await Task.Delay(500); // Simulating save operation
                    result = OperationResult<LocationViewModel>.Success(this);
                    VmIsNewLocation = false;
                }
                else
                {
                    // Using existing service methods with our ViewModel
                    // Implementation pending
                    await Task.Delay(500); // Simulating update operation
                    result = OperationResult<LocationViewModel>.Success(this);
                }

                if (!result.IsSuccess)
                {
                    VmErrorMessage = result.ErrorMessage ?? "Failed to save location";
                    OnErrorOccurred(new OperationErrorEventArgs(
                        Locations.Core.Shared.ViewModels.OperationErrorSource.Unknown,
                        VmErrorMessage,
                        result.Exception));
                }
            }
            catch (Exception ex)
            {
                VmErrorMessage = $"Error saving location: {ex.Message}";
                OnErrorOccurred(new OperationErrorEventArgs(
                    Locations.Core.Shared.ViewModels.OperationErrorSource.Unknown,
                    VmErrorMessage,
                    ex));
            }
            finally
            {
                VmIsBusy = false;
            }
        }

        /// <summary>
        /// Delete the location
        /// </summary>
        private async Task DeleteAsync()
        {
            if (_locationService == null || Id <= 0)
                return;

            try
            {
                VmIsBusy = true;
                VmErrorMessage = string.Empty;

                // Implementation pending
                await Task.Delay(500); // Simulating delete operation
                var success = true;

                if (!success)
                {
                    VmErrorMessage = "Failed to delete location";
                    OnErrorOccurred(new OperationErrorEventArgs(
                        Locations.Core.Shared.ViewModels.OperationErrorSource.Unknown,
                        VmErrorMessage));
                }
                else
                {
                    // Mark as deleted locally for UI purposes
                    IsDeleted = true;
                }
            }
            catch (Exception ex)
            {
                VmErrorMessage = $"Error deleting location: {ex.Message}";
                OnErrorOccurred(new OperationErrorEventArgs(
                    Locations.Core.Shared.ViewModels.OperationErrorSource.Unknown,
                    VmErrorMessage,
                    ex));
            }
            finally
            {
                VmIsBusy = false;
            }
        }

        /// <summary>
        /// Take a photo using the device camera
        /// </summary>
        private async Task TakePhotoAsync()
        {
            if (_mediaService == null)
                return;

            try
            {
                VmIsBusy = true;
                VmErrorMessage = string.Empty;

                var result = await _mediaService.CapturePhotoAsync();

                if (result.IsSuccess && !string.IsNullOrEmpty(result.Data))
                {
                    Photo = result.Data;
                }
                else
                {
                    VmErrorMessage = result.ErrorMessage ?? "Failed to capture photo";
                    OnErrorOccurred(new OperationErrorEventArgs(
                        Locations.Core.Shared.ViewModels.OperationErrorSource.MediaService,
                        VmErrorMessage));
                }
            }
            catch (Exception ex)
            {
                VmErrorMessage = $"Error taking photo: {ex.Message}";
                OnErrorOccurred(new OperationErrorEventArgs(
                    Locations.Core.Shared.ViewModels.OperationErrorSource.Unknown,
                    VmErrorMessage,
                    ex));
            }
            finally
            {
                VmIsBusy = false;
            }
        }

        /// <summary>
        /// Pick a photo from the device gallery
        /// </summary>
        private async Task PickPhotoAsync()
        {
            if (_mediaService == null)
                return;

            try
            {
                VmIsBusy = true;
                VmErrorMessage = string.Empty;

                var result = await _mediaService.PickPhotoAsync();

                if (result.IsSuccess && !string.IsNullOrEmpty(result.Data))
                {
                    Photo = result.Data;
                }
                else
                {
                    VmErrorMessage = result.ErrorMessage ?? "Failed to pick photo";
                    OnErrorOccurred(new OperationErrorEventArgs(
                        Locations.Core.Shared.ViewModels.OperationErrorSource.MediaService,
                        VmErrorMessage));
                }
            }
            catch (Exception ex)
            {
                VmErrorMessage = $"Error picking photo: {ex.Message}";
                OnErrorOccurred(new OperationErrorEventArgs(
                    Locations.Core.Shared.ViewModels.OperationErrorSource.Unknown,
                    VmErrorMessage,
                    ex));
            }
            finally
            {
                VmIsBusy = false;
            }
        }

        /// <summary>
        /// Start tracking the device location
        /// </summary>
        private async Task StartLocationTrackingAsync()
        {
            if (_geolocationService == null)
                return;

            try
            {
                VmIsBusy = true;
                VmErrorMessage = string.Empty;

                var result = await _geolocationService.StartTrackingAsync();

                if (result.IsSuccess)
                {
                    VmIsLocationTracking = true;
                    // Use lambda to adapt the event handler
                    _geolocationService.LocationChanged += LocationChangedHandler;
                }
                else
                {
                    VmErrorMessage = result.ErrorMessage ?? "Failed to start location tracking";
                    OnErrorOccurred(new OperationErrorEventArgs(
                        Locations.Core.Shared.ViewModels.OperationErrorSource.GeolocationService,
                        VmErrorMessage));
                }
            }
            catch (Exception ex)
            {
                VmErrorMessage = $"Error starting location tracking: {ex.Message}";
                OnErrorOccurred(new OperationErrorEventArgs(
                    Locations.Core.Shared.ViewModels.OperationErrorSource.Unknown,
                    VmErrorMessage,
                    ex));
            }
            finally
            {
                VmIsBusy = false;
            }
        }

        /// <summary>
        /// Stop tracking the device location
        /// </summary>
        private async Task StopLocationTrackingAsync()
        {
            if (_geolocationService == null || !VmIsLocationTracking)
                return;

            try
            {
                VmIsBusy = true;
                VmErrorMessage = string.Empty;

                await _geolocationService.StopTrackingAsync();

                _geolocationService.LocationChanged -= LocationChangedHandler;
                VmIsLocationTracking = false;
            }
            catch (Exception ex)
            {
                VmErrorMessage = $"Error stopping location tracking: {ex.Message}";
                OnErrorOccurred(new OperationErrorEventArgs(
                    Locations.Core.Shared.ViewModels.OperationErrorSource.Unknown,
                    VmErrorMessage,
                    ex));
            }
            finally
            {
                VmIsBusy = false;
            }
        }

        /// <summary>
        /// Handler for location changes that adapts to expected delegate type
        /// </summary>
        private void LocationChangedHandler(object sender, Locations.Core.Shared.ViewModelServices.LocationChangedEventArgs e)
        {
            // Convert the service event args to our local type
            // Only passing the properties that exist in both versions
            OnLocationChanged(sender, new Locations.Core.Shared.ViewModels.LocationChangedEventArgs(
                e.Latitude,
                e.Longitude
            // Skip other properties that don't exist in the target class
            ));
        }

        /// <summary>
        /// Handle location changes from the geolocation service
        /// </summary>
        private void OnLocationChanged(object? sender, Locations.Core.Shared.ViewModels.LocationChangedEventArgs e)
        {
            Lattitude = Math.Round(e.Latitude, 6);
            Longitude = Math.Round(e.Longitude, 6);
        }

        /// <summary>
        /// Handle errors from services
        /// </summary>
        private void OnServiceErrorOccurred(object sender, OperationErrorEventArgs e)
        {
            VmErrorMessage = e.Message;
            OnErrorOccurred(e);
        }

        /// <summary>
        /// Raise the error event
        /// </summary>
        protected virtual void OnErrorOccurred(OperationErrorEventArgs e)
        {
            ErrorOccurred?.Invoke(this, e);
        }

        /// <summary>
        /// Validate the location data
        /// </summary>
        public bool Validate(out List<string> errors)
        {
            errors = new List<string>();

            if (string.IsNullOrWhiteSpace(Title))
            {
                errors.Add("Title is required");
            }

            if (string.IsNullOrWhiteSpace(Description))
            {
                errors.Add("Description is required");
            }

            // Basic latitude/longitude validation
            if (Lattitude < -90 || Lattitude > 90)
            {
                errors.Add("Latitude must be between -90 and 90");
            }

            if (Longitude < -180 || Longitude > 180)
            {
                errors.Add("Longitude must be between -180 and 180");
            }

            return errors.Count == 0;
        }
    }
}