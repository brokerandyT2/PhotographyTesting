using Location.Core.Helpers;
using Location.Core.Helpers.AlertService;
using Location.Photography.Base;
using Locations.Core.Business.DataAccess.Interfaces;
using Locations.Core.Shared;
using Locations.Core.Shared.ViewModels;

namespace Location.Photography.Premium;

public partial class ExposureCalculator : ContentPageBase
{
    #region Fields

    private bool _skipCalculations = true;

    #endregion

    #region Constructors

    /// <summary>
    /// Default constructor for design-time and XAML preview
    /// </summary>
    public ExposureCalculator() : base()
    {
        InitializeComponent();

        // Create a design-time view model for the preview
        var viewModel = new Location.Photography.Shared.ViewModels.ExposureCalculator();
        BindingContext = viewModel;

        CloseButton.IsVisible = false;
    }

    /// <summary>
    /// Main constructor with DI
    /// </summary>
    public ExposureCalculator(
        ISettingService<SettingViewModel> settingsService,
        IAlertService alertService) : base(settingsService, alertService, PageEnums.ExposureCalculator, true)
    {
        InitializeComponent();
        InitializeViewModel();
    }

    /// <summary>
    /// Constructor for use when coming from Tips
    /// </summary>
    public ExposureCalculator(
        ISettingService<SettingViewModel> settingsService,
        IAlertService alertService,
        int tipID,
        bool isFromTips = false) : base(settingsService, alertService, PageEnums.ExposureCalculator, true)
    {
        InitializeComponent();

        // Show the close button if opened from tips
        CloseButton.IsVisible = isFromTips;

        // Load tip data and initialize
        InitializeViewModelFromTip(tipID);
    }

    #endregion

    #region Initialization

    /// <summary>
    /// Initialize the view model with default values
    /// </summary>
    private void InitializeViewModel()
    {
        try
        {
            // Create and configure the view model
            var viewModel = new Location.Photography.Shared.ViewModels.ExposureCalculator();
            BindingContext = viewModel;

            // Set initial values
            _skipCalculations = true;

            // Default to Full stop increments
            viewModel.FullHalfThirds = Location.Photography.Shared.ViewModels.ExposureCalculator.Divisions.Full;

            // Select defaults from the available options
            if (viewModel.ShutterSpeedsForPicker.Length > 0)
                ShutterSpeed_Picker.SelectedIndex = 0;

            if (viewModel.ApeaturesForPicker.Length > 0)
                fstop_Picker.SelectedIndex = 0;

            if (viewModel.ISOsForPicker.Length > 0)
                ISO_Picker.SelectedIndex = 0;

            // Store the initial values
            viewModel.ShutterSpeedSelected = ShutterSpeed_Picker.SelectedItem?.ToString();
            viewModel.FStopSelected = fstop_Picker.SelectedItem?.ToString();
            viewModel.ISOSelected = ISO_Picker.SelectedItem?.ToString();

            viewModel.OldShutterSpeed = viewModel.ShutterSpeedSelected;
            viewModel.OldFstop = viewModel.FStopSelected;
            viewModel.OldISO = viewModel.ISOSelected;

            // Default to calculating shutter speed
            viewModel.ToCalculate = Location.Photography.Shared.ViewModels.ExposureCalculator.FixedValue.ShutterSpeeds;
            ShutterSpeed_Picker.IsEnabled = false;
            fstop_Picker.IsEnabled = true;
            ISO_Picker.IsEnabled = true;

            // Now enable calculations
            _skipCalculations = false;

            // Perform the initial calculation
            viewModel.Calculate();
        }
        catch (Exception ex)
        {
            HandleError(ex, "Error initializing exposure calculator");
        }
    }

    /// <summary>
    /// Initialize the view model from a tip
    /// </summary>
    private void InitializeViewModelFromTip(int tipID)
    {
        try
        {
            // This would typically retrieve the tip from a service
            // For now, we'll create a simple view model with default values
            var viewModel = new Location.Photography.Shared.ViewModels.ExposureCalculator();
            BindingContext = viewModel;

            // Default to Full stop increments
            _skipCalculations = true;
            viewModel.FullHalfThirds = Location.Photography.Shared.ViewModels.ExposureCalculator.Divisions.Full;
            exposurefull.IsChecked = true;

            // Select defaults from the available options
            if (viewModel.ShutterSpeedsForPicker.Length > 0)
                ShutterSpeed_Picker.SelectedIndex = 0;

            if (viewModel.ApeaturesForPicker.Length > 0)
                fstop_Picker.SelectedIndex = 0;

            if (viewModel.ISOsForPicker.Length > 0)
                ISO_Picker.SelectedIndex = 0;

            // TODO: Set values from the tip service
            // For now, setting some defaults
            viewModel.ShutterSpeedSelected = ShutterSpeed_Picker.SelectedItem?.ToString();
            viewModel.FStopSelected = fstop_Picker.SelectedItem?.ToString();
            viewModel.ISOSelected = ISO_Picker.SelectedItem?.ToString();

            viewModel.OldShutterSpeed = viewModel.ShutterSpeedSelected;
            viewModel.OldFstop = viewModel.FStopSelected;
            viewModel.OldISO = viewModel.ISOSelected;

            // Default to calculating shutter speed
            viewModel.ToCalculate = Location.Photography.Shared.ViewModels.ExposureCalculator.FixedValue.ShutterSpeeds;
            shutter.IsChecked = true;
            ShutterSpeed_Picker.IsEnabled = false;
            fstop_Picker.IsEnabled = true;
            ISO_Picker.IsEnabled = true;

            // Now enable calculations
            _skipCalculations = false;

            // Perform the initial calculation
            viewModel.Calculate();
        }
        catch (Exception ex)
        {
            HandleError(ex, "Error initializing exposure calculator from tip");
        }
    }

    #endregion

    #region Event Handlers

    /// <summary>
    /// Handle changes in the exposure steps (full, half, third)
    /// </summary>
    private void exposuresteps_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (_skipCalculations || !e.Value)
            return;

        try
        {
            // Get the sender as RadioButton
            RadioButton radioButton = (RadioButton)sender;

            // Get the view model
            var viewModel = (Location.Photography.Shared.ViewModels.ExposureCalculator)BindingContext;

            // Set the divisions based on the selected radio button
            if (radioButton == exposurefull)
                viewModel.FullHalfThirds = Location.Photography.Shared.ViewModels.ExposureCalculator.Divisions.Full;
            else if (radioButton == exposurehalfstop)
                viewModel.FullHalfThirds = Location.Photography.Shared.ViewModels.ExposureCalculator.Divisions.Half;
            else if (radioButton == exposurethirdstop)
                viewModel.FullHalfThirds = Location.Photography.Shared.ViewModels.ExposureCalculator.Divisions.Thirds;

            // Update the values from the pickers
            PopulateViewModel();

            // Recalculate
            viewModel.Calculate();
        }
        catch (Exception ex)
        {
            HandleError(ex, "Error changing exposure steps");
        }
    }

    /// <summary>
    /// Handle changes in what to calculate (shutter, aperture, ISO)
    /// </summary>
    private void calculate_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (_skipCalculations || !e.Value)
            return;

        try
        {
            // Get the sender as RadioButton
            RadioButton radioButton = (RadioButton)sender;

            // Get the view model
            var viewModel = (Location.Photography.Shared.ViewModels.ExposureCalculator)BindingContext;

            // Parse the selected value to determine what to calculate
            if (int.TryParse(radioButton.Value?.ToString(), out int value))
            {
                viewModel.ToCalculate = (Location.Photography.Shared.ViewModels.ExposureCalculator.FixedValue)value;

                // Enable/disable the appropriate pickers
                ShutterSpeed_Picker.IsEnabled = value != 0; // Enable if not calculating shutter
                fstop_Picker.IsEnabled = value != 3;        // Enable if not calculating aperture
                ISO_Picker.IsEnabled = value != 1;          // Enable if not calculating ISO

                // Update the values from the pickers
                PopulateViewModel();

                // Recalculate
                viewModel.Calculate();
            }
        }
        catch (Exception ex)
        {
            HandleError(ex, "Error changing calculation type");
        }
    }

    /// <summary>
    /// Handle changes in shutter speed selection
    /// </summary>
    private void ShutterSpeed_Picker_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (_skipCalculations)
            return;

        try
        {
            // Get the view model
            var viewModel = (Location.Photography.Shared.ViewModels.ExposureCalculator)BindingContext;

            // Store the old value
            viewModel.OldShutterSpeed = viewModel.ShutterSpeedSelected;

            // Set the new value
            viewModel.ShutterSpeedSelected = ShutterSpeed_Picker.SelectedItem?.ToString();

            // Recalculate
            viewModel.Calculate();
        }
        catch (Exception ex)
        {
            HandleError(ex, "Error selecting shutter speed");
        }
    }

    /// <summary>
    /// Handle changes in aperture selection
    /// </summary>
    private void fstop_Picker_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (_skipCalculations)
            return;

        try
        {
            // Get the view model
            var viewModel = (Location.Photography.Shared.ViewModels.ExposureCalculator)BindingContext;

            // Store the old value
            viewModel.OldFstop = viewModel.FStopSelected;

            // Set the new value
            viewModel.FStopSelected = fstop_Picker.SelectedItem?.ToString();

            // Recalculate
            viewModel.Calculate();
        }
        catch (Exception ex)
        {
            HandleError(ex, "Error selecting aperture");
        }
    }

    /// <summary>
    /// Handle changes in ISO selection
    /// </summary>
    private void ISO_Picker_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (_skipCalculations)
            return;

        try
        {
            // Get the view model
            var viewModel = (Location.Photography.Shared.ViewModels.ExposureCalculator)BindingContext;

            // Store the old value
            viewModel.OldISO = viewModel.ISOSelected;

            // Set the new value
            viewModel.ISOSelected = ISO_Picker.SelectedItem?.ToString();

            // Recalculate
            viewModel.Calculate();
        }
        catch (Exception ex)
        {
            HandleError(ex, "Error selecting ISO");
        }
    }

    /// <summary>
    /// Handle close button press when opened from Tips
    /// </summary>
    private void CloseButton_Pressed(object sender, EventArgs e)
    {
        Navigation.PopModalAsync();
    }

    #endregion

    #region Helper Methods

    /// <summary>
    /// Updates the view model with the current picker values
    /// </summary>
    private void PopulateViewModel()
    {
        if (BindingContext is Location.Photography.Shared.ViewModels.ExposureCalculator viewModel)
        {
            // Store the current values for comparison
            string oldShutter = viewModel.ShutterSpeedSelected;
            string oldFStop = viewModel.FStopSelected;
            string oldISO = viewModel.ISOSelected;

            // Update the view model with the current picker values
            if (ShutterSpeed_Picker.SelectedItem != null)
                viewModel.ShutterSpeedSelected = ShutterSpeed_Picker.SelectedItem.ToString();

            if (fstop_Picker.SelectedItem != null)
                viewModel.FStopSelected = fstop_Picker.SelectedItem.ToString();

            if (ISO_Picker.SelectedItem != null)
                viewModel.ISOSelected = ISO_Picker.SelectedItem.ToString();

            // Update the old values
            viewModel.OldShutterSpeed = oldShutter;
            viewModel.OldFstop = oldFStop;
            viewModel.OldISO = oldISO;
        }
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
            // If the view model isn't initialized yet, initialize it
            if (BindingContext == null)
            {
                InitializeViewModel();
            }
        }
        catch (Exception ex)
        {
            HandleError(ex, "Error during page appearing");
        }
    }

    /// <summary>
    /// Handle navigation to the page and check first visit
    /// </summary>
    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        // Show tutorial if this is the first visit
        //PageHelpers.CheckVisit(MagicStrings.ExposureCalcViewed, PageEnums.ExposureCalculator, _settingsService, Navigation);
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
        if (BindingContext is Location.Photography.Shared.ViewModels.ExposureCalculator viewModel)
        {
            viewModel.ShowError = true;

            // Since the viewModel doesn't have a direct ErrorMessage property,
            // we'll display the error message in the UI directly
            if (errorLabel != null)
            {
                errorLabel.Text = $"{message}: {ex.Message}";
            }
        }
    }

    #endregion
}