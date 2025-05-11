using Location.Core.Helpers;
using Location.Core.Helpers.AlertService;
using Location.Photography.Base;
using Locations.Core.Business.DataAccess.Interfaces;
using Locations.Core.Shared.ViewModels;
using Locations.Core.Shared.ViewModelServices;
using System.Collections.ObjectModel;

namespace Location.Photography.Premium;

public partial class SunLocation : ContentPageBase
{
    #region Services

    private readonly ILocationService _locationService;

    #endregion

    #region Constructors

    /// <summary>
    /// Default constructor for design-time and XAML preview
    /// </summary>
    public SunLocation() : base()
    {
        InitializeComponent();

        // Create a design-time view model
        BindingContext = new Location.Photography.Shared.ViewModels.SunLocation();
    }

    /// <summary>
    /// Main constructor with DI
    /// </summary>
    public SunLocation(
        ISettingService<SettingViewModel> settingsService,
        IAlertService alertService,
        ILocationService locationService) : base(settingsService, alertService, PageEnums.SunLocation, true)
    {
        _locationService = locationService ?? throw new ArgumentNullException(nameof(locationService));

        InitializeComponent();
        InitializeViewModel();
    }

    #endregion

    #region Initialization

    /// <summary>
    /// Sets up the ViewModel with the required services
    /// </summary>
    private void InitializeViewModel()
    {
        try
        {
            // Create the view model
            var viewModel = new Location.Photography.Shared.ViewModels.SunLocation();

            // Subscribe to error events
            viewModel.ErrorOccurred += ViewModel_ErrorOccurred;

            // Set the binding context
            BindingContext = viewModel;

            // Load initial data
            LoadLocationsAsync();
        }
        catch (Exception ex)
        {
            HandleError(ex, "Error initializing view model");
        }
    }

    /// <summary>
    /// Load locations from the location service
    /// </summary>
    private async void LoadLocationsAsync()
    {
        try
        {
            if (BindingContext is Location.Photography.Shared.ViewModels.SunLocation viewModel)
            {
                viewModel.IsBusy = true;

                // Get locations from the service
                var result = await _locationService.GetLocationsAsync();

                if (result.IsSuccess && result.Data != null)
                {
                    // Create an observable collection of locations
                    viewModel.Locations = new ObservableCollection<LocationViewModel>(result.Data);

                    // If there are any locations, select the first one
                    if (viewModel.Locations.Count > 0)
                    {
                        locationPicker.SelectedIndex = 0;
                        var selectedLocation = viewModel.Locations[0];

                        // Set the coordinates from the selected location
                        viewModel.Latitude = selectedLocation.Lattitude;
                        viewModel.Longitude = selectedLocation.Longitude;
                    }
                }
                else
                {
                    // Handle error getting locations
                    viewModel.ErrorMessage = result.ErrorMessage ?? "Failed to load locations";
                }
            }
        }
        catch (Exception ex)
        {
            HandleError(ex, "Error loading locations");
        }
        finally
        {
            if (BindingContext is Location.Photography.Shared.ViewModels.SunLocation viewModel)
            {
                viewModel.IsBusy = false;
            }
        }
    }

    #endregion

    #region Event Handlers

    /// <summary>
    /// Handle location picker selection change
    /// </summary>
    private void locationPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (locationPicker.SelectedItem is LocationViewModel selectedLocation &&
            BindingContext is Location.Photography.Shared.ViewModels.SunLocation viewModel)
        {
            viewModel.Latitude = selectedLocation.Lattitude;
            viewModel.Longitude = selectedLocation.Longitude;
        }
    }

    /// <summary>
    /// Handle date selection change
    /// </summary>
    private void date_DateSelected(object sender, DateChangedEventArgs e)
    {
        // This is handled by the binding to SelectedDate
    }

    /// <summary>
    /// Handle time selection change
    /// </summary>
    private void time_TimeSelected(object sender, TimeChangedEventArgs e)
    {
        // This is handled by the binding to SelectedTime
    }

    /// <summary>
    /// Handle errors from the view model
    /// </summary>
    private void ViewModel_ErrorOccurred(object sender, Locations.Core.Shared.ViewModels.OperationErrorEventArgs e)
    {
        // Display error to user if it's not already displayed in the UI
        MainThread.BeginInvokeOnMainThread(async () => {
            await DisplayAlert(
                Location.Photography.Resources.AppResources.Error,
                e.Message,
                Location.Photography.Resources.AppResources.OK);
        });
    }

    #endregion

    #region Lifecycle Methods

    /// <summary>
    /// Called when the page appears
    /// </summary>
    protected override void OnPageAppearing(object sender, EventArgs e)
    {
        base.OnPageAppearing(sender, e);

        try
        {
            // Get the view model
            if (BindingContext is Location.Photography.Shared.ViewModels.SunLocation viewModel)
            {
                // Re-subscribe to events
                viewModel.ErrorOccurred -= ViewModel_ErrorOccurred;
                viewModel.ErrorOccurred += ViewModel_ErrorOccurred;

                // Start monitoring compass and sensors
                viewModel.BeginMonitoring = true;

                // If there are no locations yet, load them
                if (viewModel.Locations == null || viewModel.Locations.Count == 0)
                {
                    LoadLocationsAsync();
                }
            }
        }
        catch (Exception ex)
        {
            HandleError(ex, "Error during page appearing");
        }
    }

    /// <summary>
    /// Called when the page disappears
    /// </summary>
    protected override void OnPageDisappearing(object sender, EventArgs e)
    {
        base.OnPageDisappearing(sender, e);

        try
        {
            // Stop monitoring sensors when the page disappears
            if (BindingContext is Location.Photography.Shared.ViewModels.SunLocation viewModel)
            {
                // Unsubscribe from events
                viewModel.ErrorOccurred -= ViewModel_ErrorOccurred;

                // Stop monitoring
                viewModel.BeginMonitoring = false;
            }
        }
        catch (Exception ex)
        {
            // Just log the error since we're leaving the page
            System.Diagnostics.Debug.WriteLine($"Error during page disappearing: {ex.Message}");
        }
    }

    #endregion

    #region Error Handling

    /// <summary>
    /// Handle errors during view operations
    /// </summary>
    private void HandleError(Exception ex, string message)
    {
        // Log the error
        System.Diagnostics.Debug.WriteLine($"Error: {message}. {ex.Message}");

        // Display alert to user
        DisplayAlert(Location.Photography.Resources.AppResources.Error, message, Location.Photography.Resources.AppResources.OK);

        // Pass the error to the ViewModel if available
        if (BindingContext is Location.Photography.Shared.ViewModels.SunLocation viewModel)
        {
            viewModel.ErrorMessage = $"{message}: {ex.Message}";
            viewModel.IsBusy = false;
        }
    }

    #endregion
}