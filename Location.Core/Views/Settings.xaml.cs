using Location.Core.Helpers;
using Location.Core.Helpers.AlertService;
using Location.Core.Resources;
using Locations.Core.Business.DataAccess.Interfaces;
using Locations.Core.Shared;
using Locations.Core.Shared.Enums;
using Locations.Core.Shared.ViewModels;

namespace Location.Core.Views;

public partial class Settings : ContentPageBase
{
    #region Services

    private readonly ISettingService<SettingViewModel> _settingsService;

    #endregion

    #region Constructors

    /// <summary>
    /// Default constructor for design-time and XAML preview
    /// </summary>
    public Settings() : base()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Main constructor with DI
    /// </summary>
    public Settings(
        ISettingService<SettingViewModel> settingsService,
        IAlertService alertService) : base(settingsService, alertService, PageEnums.Settings, false)
    {
        _settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));

        InitializeComponent();
    }

    #endregion

    #region Data Loading

    /// <summary>
    /// Loads settings from the service
    /// </summary>
    private void GetSettings()
    {
        try
        {
            if (_settingsService == null)
                return;

            var settings = _settingsService.GetAllSettings();
            BindingContext = settings;

            // Set wind direction label text
            if (settings.WindDirection?.Value == MagicStrings.TowardsWind)
            {
                WindDirection.Text = AppResources.TowardsWind;
            }
            else
            {
                WindDirection.Text = AppResources.WithWind;
            }
        }
        catch (Exception ex)
        {
            HandleError(ex, "Error loading settings");
        }
    }

    /// <summary>
    /// Update a setting with proper error handling
    /// </summary>
    private async Task<bool> UpdateSettingAsync(SettingViewModel setting)
    {
        try
        {
            if (_settingsService == null)
                return false;

            var result = await _settingsService.UpdateAsync(setting);

            if (!result.IsSuccess)
            {
                // DataOperationResult doesn't have ErrorMessage property, use Message instead
                // or just display a general error message
                await DisplayAlert(AppResources.Error,
                    AppResources.ErrorUpdatingSetting,
                    AppResources.OK);
                return false;
            }

            return true;
        }
        catch (Exception ex)
        {
            await DisplayAlert(AppResources.Error, AppResources.ErrorUpdatingSetting, AppResources.OK);
            HandleError(ex, $"Error updating setting {setting.Key}");
            return false;
        }
    }

    #endregion

    #region Event Handlers

    /// <summary>
    /// Handle hemisphere switch toggle
    /// </summary>
    private async void HemisphereSwitch_Toggled(object sender, ToggledEventArgs e)
    {
        try
        {
            if (_settingsService == null)
                return;

            var setting = _settingsService.GetSettingByName(MagicStrings.Hemisphere);
            setting.Value = e.Value ? Hemisphere.HemisphereChoices.North.Name() : Hemisphere.HemisphereChoices.South.Name();

            bool success = await UpdateSettingAsync(setting);

            if (success)
            {
                GetSettings();
            }
        }
        catch (Exception ex)
        {
            HandleError(ex, "Error handling hemisphere switch");
        }
    }

    /// <summary>
    /// Handle time format switch toggle
    /// </summary>
    private async void TimeSwitch_Toggled(object sender, ToggledEventArgs e)
    {
        try
        {
            if (_settingsService == null)
                return;

            var setting = _settingsService.GetSettingByName(MagicStrings.TimeFormat);
            setting.Value = e.Value ? MagicStrings.USTimeformat_Pattern : MagicStrings.InternationalTimeFormat_Pattern;

            bool success = await UpdateSettingAsync(setting);

            if (success)
            {
                GetSettings();
            }
        }
        catch (Exception ex)
        {
            HandleError(ex, "Error handling time switch");
        }
    }

    /// <summary>
    /// Handle date format switch toggle
    /// </summary>
    private async void DateFormat_Toggled(object sender, ToggledEventArgs e)
    {
        try
        {
            if (_settingsService == null)
                return;

            var setting = _settingsService.GetSettingByName(MagicStrings.DateFormat);
            setting.Value = e.Value ? MagicStrings.USDateFormat : MagicStrings.InternationalFormat;

            bool success = await UpdateSettingAsync(setting);

            if (success)
            {
                GetSettings();
            }
        }
        catch (Exception ex)
        {
            HandleError(ex, "Error handling date format switch");
        }
    }

    /// <summary>
    /// Handle wind direction switch toggle
    /// </summary>
    private async void WindDirectionSwitch_Toggled(object sender, ToggledEventArgs e)
    {
        try
        {
            if (_settingsService == null)
                return;

            var setting = _settingsService.GetSettingByName(MagicStrings.WindDirection);
            setting.Value = e.Value ? MagicStrings.TowardsWind : MagicStrings.WithWind;

            bool success = await UpdateSettingAsync(setting);

            if (success)
            {
                GetSettings();
            }
        }
        catch (Exception ex)
        {
            HandleError(ex, "Error handling wind direction switch");
        }
    }

    /// <summary>
    /// Handle temperature format switch toggle
    /// </summary>
    private async void TempFormatSwitch_Toggled(object sender, ToggledEventArgs e)
    {
        try
        {
            if (_settingsService == null)
                return;

            var setting = _settingsService.GetSettingByName(MagicStrings.TemperatureType);
            setting.Value = e.Value ? MagicStrings.Fahrenheit : MagicStrings.Celsius;

            bool success = await UpdateSettingAsync(setting);

            if (success)
            {
                GetSettings();
            }
        }
        catch (Exception ex)
        {
            HandleError(ex, "Error handling temperature format switch");
        }
    }

    /// <summary>
    /// Handle ad support switch toggle
    /// </summary>
    private async void adsupport_Toggled(object sender, ToggledEventArgs e)
    {
        try
        {
            if (_settingsService == null)
                return;

            var setting = _settingsService.GetSettingByName(MagicStrings.FreePremiumAdSupported);
            setting.Value = e.Value ? MagicStrings.True_string : MagicStrings.False_string;

            bool success = await UpdateSettingAsync(setting);

            if (success)
            {
                GetSettings();
            }
        }
        catch (Exception ex)
        {
            HandleError(ex, "Error handling ad support switch");
        }
    }

    #endregion

    #region Lifecycle Methods

    /// <summary>
    /// Load settings when navigated to
    /// </summary>
    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        // Check for first visit
        PageHelpers.CheckVisit(MagicStrings.SettingsViewed, PageEnums.Settings, _settingsService, Navigation);

        // Load settings
        GetSettings();
    }

    #endregion
}