using Location.Core.Helpers;
using Location.Core.Helpers.AlertService;
using Location.Core.Resources;
using Locations.Core.Business.DataAccess.Interfaces;
using Locations.Core.Shared.ViewModels;
using Locations.Core.Shared.ViewModelServices;
using System;
using System.Threading.Tasks;

namespace Location.Core.Views;

public partial class AddLocation : ContentPageBase
{
    #region Services

    private readonly ILocationService _locationService;
    private readonly IMediaService _mediaService;
    private readonly IGeolocationService _geolocationService;
    private readonly int _locationId;
    private readonly bool _isEditMode;

    #endregion

    #region Constructors

    /// <summary>
    /// Default constructor for design-time and XAML preview
    /// </summary>
    public AddLocation() : base()
    {
        InitializeComponent();

        _locationId = 0;
        _isEditMode = false;

        // Set an empty view model for design-time
        BindingContext = new LocationViewModel();

        // Configure UI based on mode
        CloseModal.IsVisible = _isEditMode;
    }

    /// <summary>
    /// Main constructor with DI
    /// </summary>
    public AddLocation(
        ISettingService<SettingViewModel> settingsService,
        IAlertService alertService,
        ILocationService locationService,
        IMediaService mediaService,
        IGeolocationService geolocationService,
        int locationId = 0,
        bool isEditMode = false) : base(settingsService, alertService, PageEnums.AddLocation, false)
    {
        _locationService = locationService ?? throw new ArgumentNullException(nameof(locationService));
        _mediaService = mediaService ?? throw new ArgumentNullException(nameof(mediaService));
        _geolocationService = geolocationService ?? throw new ArgumentNullException(nameof(geolocationService));
        _locationId = locationId;
        _isEditMode = isEditMode;

        InitializeComponent();

        // Load data and setup UI
        InitializeViewModel();
    }

    #endregion

    #region Setup and Initialization

    /// <summary>
    /// Initializes the ViewModel with proper services and data
    /// </summary>
    private void InitializeViewModel()
    {
        if (_locationId > 0)
        {
            // When editing an existing location, load it
            LoadLocationAsync(_locationId);
        }
        else
        {
            // Create new location ViewModel with services
            var viewModel = new LocationViewModel(
                _locationService,
                _mediaService,
                _geolocationService);

            // Subscribe to error events
            viewModel.ErrorOccurred += ViewModel_ErrorOccurred;

            // Set as binding context
            BindingContext = viewModel;

            // We'll start location tracking when the page appears
        }

        // Configure UI based on mode
        CloseModal.IsVisible = _isEditMode;
    }

    /// <summary>
    /// Loads a location by ID
    /// </summary>
    private async void LoadLocationAsync(int id)
    {
        try
        {
            // Show loading indicator
            var loadingViewModel = new LocationViewModel
            {
                VmIsBusy = true
            };
            BindingContext = loadingViewModel;

            // Load the location
            var result = await _locationService.GetLocationAsync(id);

            if (result.IsSuccess && result.Data != null)
            {
                // Create a fully initialized view model with the loaded data
                var loadedViewModel = new LocationViewModel(
                    _locationService,
                    _mediaService,
                    _geolocationService);

                // Copy properties from the result to the new view model
                loadedViewModel.Id = result.Data.Id;
                loadedViewModel.Title = result.Data.Title;
                loadedViewModel.Description = result.Data.Description;
                loadedViewModel.Lattitude = result.Data.Lattitude;
                loadedViewModel.Longitude = result.Data.Longitude;
                loadedViewModel.City = result.Data.City;
                loadedViewModel.State = result.Data.State;
                loadedViewModel.Photo = result.Data.Photo;
                loadedViewModel.Timestamp = result.Data.Timestamp;
                loadedViewModel.DateFormat = result.Data.DateFormat;

                // Mark as existing location
                loadedViewModel.VmIsNewLocation = false;

                // Subscribe to error events
                loadedViewModel.ErrorOccurred += ViewModel_ErrorOccurred;

                // Set as binding context
                BindingContext = loadedViewModel;
            }
            else
            {
                // Create a new view model with error
                var errorViewModel = new LocationViewModel(
                    _locationService,
                    _mediaService,
                    _geolocationService)
                {
                    VmErrorMessage = result.ErrorMessage ?? "Failed to load location"
                };
                BindingContext = errorViewModel;

                // Display error to user
                await DisplayAlert(
                    AppResources.Error,
                    result.ErrorMessage ?? "Failed to load location",
                    AppResources.OK);
            }
        }
        catch (Exception ex)
        {
            // Handle error loading location
            await DisplayAlert(
                AppResources.Error,
                $"Error loading location: {ex.Message}",
                AppResources.OK);

            // Create a new view model with error
            var errorViewModel = new LocationViewModel(
                _locationService,
                _mediaService,
                _geolocationService)
            {
                VmErrorMessage = $"Error loading location: {ex.Message}"
            };
            BindingContext = errorViewModel;
        }
        finally
        {
            // Ensure busy indicator is hidden
            if (BindingContext is LocationViewModel vm)
            {
                vm.VmIsBusy = false;
            }
        }
    }

    #endregion

    #region Event Handlers

    /// <summary>
    /// Handle save button press
    /// </summary>
    private async void Save_Pressed(object sender, EventArgs e)
    {
        if (BindingContext is LocationViewModel viewModel)
        {
            // Execute the save command
            viewModel.SaveCommand.Execute(null);

            // If save was successful (no error message), reset view or close modal
            if (string.IsNullOrEmpty(viewModel.VmErrorMessage))
            {
                if (_isEditMode)
                {
                    await Navigation.PopModalAsync();
                }
                else
                {
                    // Create a new view model with services for a new location
                    var newViewModel = new LocationViewModel(
                        _locationService,
                        _mediaService,
                        _geolocationService);

                    // Subscribe to error events
                    newViewModel.ErrorOccurred += ViewModel_ErrorOccurred;

                    // Set as binding context
                    BindingContext = newViewModel;

                    // Start getting location for the new entry
                    await GetCurrentLocationAsync();

                    // Show success message
                    await DisplayAlert(
                        "Success",
                        "Location saved successfully",
                        AppResources.OK);
                }
            }
        }
    }

    /// <summary>
    /// Handle add photo button press
    /// </summary>
    private void AddPhoto_Pressed(object sender, EventArgs e)
    {
        if (BindingContext is LocationViewModel viewModel)
        {
            viewModel.TakePhotoCommand.Execute(null);
        }
    }

    /// <summary>
    /// Handle close modal button press
    /// </summary>
    private void CloseModal_Pressed(object sender, EventArgs e)
    {
        Navigation.PopModalAsync();
    }

    /// <summary>
    /// Handle errors from the view model
    /// </summary>
    private void ViewModel_ErrorOccurred(object sender, Locations.Core.Shared.ViewModels.OperationErrorEventArgs e)
    {
        // Display error to user if it's not already displayed in the UI
        MainThread.BeginInvokeOnMainThread(async () => {
            await DisplayAlert(
                AppResources.Error,
                e.Message,
                AppResources.OK);
        });
    }

    #endregion

    #region Location Handling

    /// <summary>
    /// Get the current location
    /// </summary>
    private async Task GetCurrentLocationAsync()
    {
        if (BindingContext is LocationViewModel viewModel && !_isEditMode)
        {
            try
            {
                // Start location tracking on the view model
                viewModel.StartLocationTrackingCommand.Execute(null);

                // For a more immediate response, also try to get the current location directly
                if (_geolocationService != null)
                {
                    var result = await _geolocationService.GetCurrentLocationAsync();
                    if (result.IsSuccess && result.Data != null)
                    {
                        // Update the view model with the current location
                        viewModel.Lattitude = Math.Round(result.Data.Latitude, 6);
                        viewModel.Longitude = Math.Round(result.Data.Longitude, 6);
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the error but don't show it to the user, as it's not critical
                System.Diagnostics.Debug.WriteLine($"Error getting current location: {ex.Message}");
            }
        }
    }

    #endregion

    #region Lifecycle Methods

    /// <summary>
    /// Called when the page appears
    /// </summary>
    protected override async void OnPageAppearing(object sender, EventArgs e)
    {
        base.OnPageAppearing(sender, e);

        // Re-subscribe to ViewModel events in case the binding context changed
        if (BindingContext is LocationViewModel viewModel)
        {
            // Make sure we only subscribe once
            viewModel.ErrorOccurred -= ViewModel_ErrorOccurred;
            viewModel.ErrorOccurred += ViewModel_ErrorOccurred;

            // If this is a new location (not edit mode), get the current location immediately
            if (!_isEditMode && viewModel.VmIsNewLocation && viewModel.Lattitude == 0 && viewModel.Longitude == 0)
            {
                await GetCurrentLocationAsync();
            }
        }
    }

    /// <summary>
    /// Called when the page disappears
    /// </summary>
    protected override void OnPageDisappearing(object sender, EventArgs e)
    {
        base.OnPageDisappearing(sender, e);

        // Unsubscribe from ViewModel events
        if (BindingContext is LocationViewModel viewModel)
        {
            viewModel.ErrorOccurred -= ViewModel_ErrorOccurred;

            // Ensure location tracking is stopped when leaving the page
            if (viewModel.VmIsLocationTracking)
            {
                viewModel.StopLocationTrackingCommand.Execute(null);
            }
        }
    }

    #endregion
}