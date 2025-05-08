using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Locations.Core.Shared.DTO;
using Locations.Core.Shared.ViewModels.Interface;
using Locations.Core.Shared.ViewModelServices;
using System.Windows.Input;
using Locations.Core.Shared.ViewModelServices;
using Location.Core.Helpers.AlertService;
using System.Xml.Linq;

namespace Locations.Core.Shared.ViewModels
{
    public partial class LocationViewModel : LocationDTO, ILocationViewModel, IValidatable
    {
        // Services
        private readonly ILocationService? _locationService;
        private readonly IMediaService? _mediaService;
        private readonly IGeolocationService? _geolocationService;

        // Events for integration with our error bubbling system
        public event EventHandler<DataErrorEventArgs>? ErrorOccurred;

        // Observable properties
        [ObservableProperty]
        private bool _isBusy;

        [ObservableProperty]
        private string _errorMessage = string.Empty;

        // Commands
        public ICommand TakePhotoCommand { get; }
        public ICommand SaveCommand { get; }

        /// <summary>
        /// Constructor for design-time and when created by SQLite
        /// </summary>
        public LocationViewModel()
        {
            // Set reasonable defaults
            TakePhotoCommand = new AsyncRelayCommand(TakePhotoAsync);
            SaveCommand = new AsyncRelayCommand(SaveAsync);

            // Note: Services will be null here, methods should check for this
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

            // Subscribe to error events
            if (_locationService != null)
                _locationService.ErrorOccurred += OnServiceErrorOccurred;

            // Start location tracking
            StartLocationTrackingAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Handle errors from services
        /// </summary>
        private void OnServiceErrorOccurred(object sender, DataErrorEventArgs e)
        {
            ErrorMessage = e.Message;
            OnErrorOccurred(e);
        }

        /// <summary>
        /// Raise error event
        /// </summary>
        protected virtual void OnErrorOccurred(DataErrorEventArgs e)
        {
            ErrorOccurred?.Invoke(this, e);
        }

        /// <summary>
        /// Start tracking device location
        /// </summary>
        private async Task StartLocationTrackingAsync()
        {
            try
            {
                if (_geolocationService != null)
                {
                    await _geolocationService.StartTrackingAsync();
                    _geolocationService.LocationChanged += OnLocationChanged;
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error starting location tracking: {ex.Message}";
                OnErrorOccurred(new DataErrorEventArgs(
                    ErrorSource.Permission,
                    ErrorMessage,
                    ex));
            }
        }

        private void OnLocationChanged(object? sender, LocationChangedEventArgs e)
        {
            Lattitude = Math.Round(e.Latitude, 4);
            Longitude = Math.Round(e.Longitude, 4);
        }

        public bool Validate(out List<string> errors)
        {
            errors = new List<string>();

            return errors.Count == 0;
        }

    }
}
