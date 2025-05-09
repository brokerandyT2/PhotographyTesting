using Location.Core.Helpers;
using Location.Core.Helpers.AlertService;
using Location.Core.Resources;
using Locations.Core.Business.DataAccess.Interfaces;
using Locations.Core.Shared.ViewModels;
using Locations.Core.Shared.ViewModelServices;

namespace Location.Core.Views
{
    public partial class ListLocations : ContentPageBase
    {
        private LocationsListViewModel _viewModel;

        /// <summary>
        /// Default constructor for design-time and XAML preview
        /// </summary>
        public ListLocations() : base()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Main constructor with DI
        /// </summary>
        public ListLocations(
            ISettingService<SettingViewModel> settingsService,
            IAlertService alertService,
            ILocationService locationService) : base(settingsService, alertService, PageEnums.ListLocations, false)
        {
           
            InitializeComponent();

            // Create ViewModel and set as BindingContext
            _viewModel = new LocationsListViewModel(locationService);
            BindingContext = _viewModel;

            // Subscribe to ViewModel events
            _viewModel.ErrorOccurred += ViewModel_ErrorOccurred;

            // Subscribe to navigation messages
            MessagingCenter.Subscribe<LocationsListViewModel, int>(this, "NavigateToLocation", OnNavigateToLocation);
            MessagingCenter.Subscribe<LocationsListViewModel, LocationViewModel>(this, "OpenMap", OnOpenMap);
        }

        /// <summary>
        /// Handle ViewModel error events
        /// </summary>
        private void ViewModel_ErrorOccurred(object sender, Locations.Core.Shared.ViewModels.OperationErrorEventArgs e)
        {
            MainThread.BeginInvokeOnMainThread(async () => {
                await DisplayAlert(AppResources.Error, e.Message, AppResources.OK);
            });
        }

        /// <summary>
        /// Handle navigation to location details
        /// </summary>
        private void OnNavigateToLocation(LocationsListViewModel sender, int locationId)
        {
            MainThread.BeginInvokeOnMainThread(async () => {
                await Navigation.PushModalAsync(new NavigationPage(new Views.EditLocation(locationId)));
            });
        }

        /// <summary>
        /// Handle opening map for location
        /// </summary>
        private void OnOpenMap(LocationsListViewModel sender, LocationViewModel location)
        {
            MainThread.BeginInvokeOnMainThread(async () => {
                try
                {
                    var mapLocation = new Microsoft.Maui.Devices.Sensors.Location(location.Lattitude, location.Longitude);
                    var options = new MapLaunchOptions { Name = location.Title };

                    await Map.Default.OpenAsync(mapLocation, options);
                }
                catch (Exception ex)
                {
                    await DisplayAlert(AppResources.Error, "No Map Application", AppResources.OK);
                    HandleError(ex, "Error opening map");
                }
            });
        }

        /// <summary>
        /// Load data when navigated to
        /// </summary>
        protected override void OnNavigatedTo(NavigatedToEventArgs args)
        {
            base.OnNavigatedTo(args);

            // Load data through the ViewModel
            if (_viewModel != null)
            {
                _viewModel.LoadLocationsCommand.Execute(null);
            }
        }

        /// <summary>
        /// Clean up resources when page disappears
        /// </summary>
        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            // Unsubscribe from ViewModel events
            if (_viewModel != null)
            {
                _viewModel.ErrorOccurred -= ViewModel_ErrorOccurred;
            }

            // Unsubscribe from messaging center
            MessagingCenter.Unsubscribe<LocationsListViewModel, int>(this, "NavigateToLocation");
            MessagingCenter.Unsubscribe<LocationsListViewModel, LocationViewModel>(this, "OpenMap");
        }
    }
}