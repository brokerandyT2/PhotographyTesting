using Location.Photography.Base;
using Location.Core.Helpers.AlertService;
using Locations.Core.Business.DataAccess.Interfaces;
using Locations.Core.Shared.ViewModels;
using Location.Photography.Shared.ViewModels;
using Location.Core.Helpers;
using System.Threading.Tasks;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Location.Photography.Pro
{
    public partial class SunCalculations : ContentPageBase
    {
        #region Services

        private readonly ILocationService<LocationViewModel> _locationService;
        private readonly ISettingService<SettingViewModel> _settingsService;

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor for design-time and XAML preview
        /// </summary>
        public SunCalculations() : base()
        {
            InitializeComponent();

            // Create a default view model for design-time
            BindingContext = new Location.Photography.Shared.ViewModels.SunCalculations();
        }

        /// <summary>
        /// Main constructor with DI
        /// </summary>
        public SunCalculations(
            ISettingService<SettingViewModel> settingsService,
            IAlertService alertService,
            ILocationService<LocationViewModel> locationService) : base(settingsService, alertService, PageEnums.SunCalculations, true)
        {
            _settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));
            _locationService = locationService ?? throw new ArgumentNullException(nameof(locationService));

            InitializeComponent();

            // Create the view model and set as binding context
            BindingContext = new Location.Photography.Shared.ViewModels.SunCalculations();

            // Subscribe to ViewModel events
            SubscribeToViewModelEvents();
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Handle location picker selection change
        /// </summary>
        private void LocationPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            // The binding will handle updating the ViewModel
        }

        /// <summary>
        /// Handle date picker selection change
        /// </summary>
        private void DatePicker_DateSelected(object sender, DateChangedEventArgs e)
        {
            // The binding will handle updating the ViewModel
        }

        #endregion

        #region Lifecycle Methods

        /// <summary>
        /// Load data when the page appears
        /// </summary>
        protected override async void OnPageAppearing(object sender, EventArgs e)
        {
            base.OnPageAppearing(sender, e);

            try
            {
                // Get the view model
                if (BindingContext is Location.Photography.Shared.ViewModels.SunCalculations viewModel)
                {
                    // Set format settings from app settings
                    if (_settingsService != null)
                    {
                        var dateFormatSetting = _settingsService.GetSettingByName(Locations.Core.Shared.MagicStrings.DateFormat);
                        var timeFormatSetting = _settingsService.GetSettingByName(Locations.Core.Shared.MagicStrings.TimeFormat);

                        viewModel.DateFormat = dateFormatSetting?.Value ?? "MM/dd/yyyy";
                        viewModel.TimeFormat = timeFormatSetting?.Value ?? "hh:mm tt";
                    }

                    // Load locations if not already loaded
                    if ((viewModel.LocationsS == null || viewModel.LocationsS.Count == 0) && _locationService != null)
                    {
                        await LoadLocationsAsync(viewModel);
                    }
                }
            }
            catch (Exception ex)
            {
                HandleError(ex, "Error initializing page");
            }
        }

        /// <summary>
        /// Clean up when navigating away
        /// </summary>
        protected override void OnPageDisappearing(object sender, EventArgs e)
        {
            base.OnPageDisappearing(sender, e);

            // Unsubscribe from ViewModel events
            UnsubscribeFromViewModelEvents();
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Subscribe to ViewModel error events
        /// </summary>
        protected override void SubscribeToViewModelEvents()
        {
            if (BindingContext is Location.Photography.Shared.ViewModels.SunCalculations viewModel)
            {
                viewModel.ErrorOccurred += ViewModel_ErrorOccurred;
            }
        }

        /// <summary>
        /// Unsubscribe from ViewModel error events
        /// </summary>
        protected override void UnsubscribeFromViewModelEvents()
        {
            if (BindingContext is Location.Photography.Shared.ViewModels.SunCalculations viewModel)
            {
                viewModel.ErrorOccurred -= ViewModel_ErrorOccurred;
            }
        }

        /// <summary>
        /// Handle errors from the ViewModel
        /// </summary>
        private void ViewModel_ErrorOccurred(object sender, Locations.Core.Shared.ViewModels.OperationErrorEventArgs e)
        {
            Microsoft.Maui.Controls.Application.Current.Dispatcher.Dispatch(() =>
            {
                DisplayAlert(Location.Photography.Resources.AppResources.Error, e.Message, Location.Photography.Resources.AppResources.OK);
            });
        }

        /// <summary>
        /// Load locations from the service
        /// </summary>
        private async Task LoadLocationsAsync(Location.Photography.Shared.ViewModels.SunCalculations viewModel)
        {
            try
            {
                viewModel.VmIsBusy = true;

                // Get locations with details
                var locations = await _locationService.GetLocationsWithDetailsAsync();

                if (locations != null && locations.Count > 0)
                {
                    // Set locations directly on the view model (avoiding any .Core extension method issue)
                    viewModel.LocationsS = new List<LocationViewModel>(locations);

                    // Select the first location by default
                    if (viewModel.LocationsS.Count > 0)
                    {
                        viewModel.SelectedLocation = viewModel.LocationsS[0];
                    }
                }
                else
                {
                    // No locations found
                    viewModel.VmErrorMessage = "No locations found. Please add locations first.";
                }
            }
            catch (Exception ex)
            {
                viewModel.VmErrorMessage = $"Error loading locations: {ex.Message}";
                HandleError(ex, "Error loading locations");
            }
            finally
            {
                viewModel.VmIsBusy = false;
            }
        }

        #endregion
    }
}