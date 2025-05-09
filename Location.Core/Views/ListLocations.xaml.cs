using Location.Core.Helpers;
using Location.Core.Helpers.AlertService;
using Location.Core.Resources;
using Locations.Core.Business.DataAccess.Interfaces;
using Locations.Core.Shared.ViewModels;
using Locations.Core.Shared.ViewModelServices;
using System.Collections.ObjectModel;

namespace Location.Core.Views;

public partial class ListLocations : ContentPageBase
{
    #region Services

    private readonly ILocationService _locationService;
    private ObservableCollection<LocationViewModel> _items = [];
    public ObservableCollection<LocationViewModel> Items { get { return _items; } set { _items = value; } }

    #endregion

    #region Constructors

    /// <summary>
    /// Default constructor for design-time and XAML preview
    /// </summary>
    public ListLocations() : base()
    {
        InitializeComponent();
        BindingContext = this;
    }

    /// <summary>
    /// Main constructor with DI
    /// </summary>
    public ListLocations(
        ISettingService<SettingViewModel> settingsService,
        IAlertService alertService,
        ILocationService locationService) : base(settingsService, alertService, PageEnums.ListLocations, false)
    {
        _locationService = locationService ?? throw new ArgumentNullException(nameof(locationService));

        InitializeComponent();
        BindingContext = this;
    }

    #endregion

    #region Data Loading

    /// <summary>
    /// Populates location data from the service
    /// </summary>
    private async void PopulateData()
    {
        try
        {
            if (_locationService == null)
            {
                await DisplayAlert(AppResources.Error, AppResources.ErrorUpdatingSetting, AppResources.OK);
                return;
            }

            var result = await _locationService.GetLocationsAsync();

            Items.Clear();

            if (result.IsSuccess && result.Data != null)
            {
                foreach (var location in result.Data)
                {
                    _items.Add(location);
                }
            }
            else
            {
                // Handle error
                await DisplayAlert(AppResources.Error,
                    result.ErrorMessage ?? AppResources.ErrorUpdatingSetting,
                    AppResources.OK);
            }

            BindingContext = this;
            cv.ItemsSource = Items;
        }
        catch (Exception ex)
        {
            await DisplayAlert(AppResources.Error, AppResources.ErrorUpdatingSetting, AppResources.OK);
            HandleError(ex, "Error loading locations");
        }
    }

    #endregion

    #region Event Handlers

    /// <summary>
    /// Opens the map for the selected location
    /// </summary>
    private async void ImageButton_Pressed(object sender, EventArgs e)
    {
        if (_locationService == null)
            return;

        try
        {
            var button = (Microsoft.Maui.Controls.ImageButton)sender;
            var id = (int)button.CommandParameter;

            var result = await _locationService.GetLocationAsync(id);

            if (!result.IsSuccess || result.Data == null)
            {
                await DisplayAlert(AppResources.Error,
                    result.ErrorMessage ?? AppResources.ErrorUpdatingSetting,
                    AppResources.OK);
                return;
            }

            var locationData = result.Data;
            var location = new Microsoft.Maui.Devices.Sensors.Location(locationData.Lattitude, locationData.Longitude);
            var options = new MapLaunchOptions { Name = locationData.Title };

            try
            {
                await Map.Default.OpenAsync(location, options);
            }
            catch (Exception ex)
            {
                await DisplayAlert(AppResources.Error, "No Map Application", AppResources.OK);
                HandleError(ex, "Error opening map");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert(AppResources.Error, AppResources.ErrorUpdatingSetting, AppResources.OK);
            HandleError(ex, "Error handling map button");
        }
    }

    /// <summary>
    /// Navigate to location details when an item is selected
    /// </summary>
    private void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is not LocationViewModel item)
            return;

        var id = item.Id;
        Navigation.PushModalAsync(new NavigationPage(new Views.EditLocation(id)));
    }

    #endregion

    #region Lifecycle Methods

    /// <summary>
    /// Load data when navigated to
    /// </summary>
    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        // Populate data whenever we navigate to this page
        PopulateData();
    }

    #endregion
}