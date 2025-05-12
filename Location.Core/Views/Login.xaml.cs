﻿using Location.Core.Resources;
using Locations.Core.Business;
using Locations.Core.Business.StorageSvc;
using Locations.Core.Shared;
using Locations.Core.Shared.Enums;
using Locations.Core.Shared.ViewModels;

namespace Location.Core.Views;

public partial class Login : ContentPage
{
    public Login()
    {
        InitializeComponent();
    }

    private void HemisphereSwitch_Toggled(object sender, ToggledEventArgs e)
    {
        ((SettingsViewModel)BindingContext).Hemisphere.Value = e.Value ? MagicStrings.North : MagicStrings.South;
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        GetSetting();
    }

    private void TimeSwitch_Toggled(object sender, ToggledEventArgs e)
    {
        ((SettingsViewModel)BindingContext).TimeFormat.Value = e.Value ? MagicStrings.USTimeformat_Pattern : MagicStrings.InternationalTimeFormat_Pattern;
    }

    private void DateFormat_Toggled(object sender, ToggledEventArgs e)
    {
        ((SettingsViewModel)BindingContext).DateFormat.Value = e.Value ? MagicStrings.USDateFormat : MagicStrings.InternationalFormat;
    }

    private void GetSetting()
    {
        if (emailAddress != null)
        {
            emailAddress.TextChanged += EmailAddress_TextChanged;
        }

        SettingsViewModel svm = new SettingsViewModel();
        svm.Hemisphere = new SettingViewModel();
        svm.TimeFormat = new SettingViewModel();
        svm.DateFormat = new SettingViewModel();
        svm.Email = new SettingViewModel();
        svm.WindDirection = new SettingViewModel();
        svm.TemperatureFormat = new SettingViewModel();
        svm.Hemisphere.Value = MagicStrings.North;
        svm.TimeFormat.Value = MagicStrings.USTimeformat_Pattern;
        svm.DateFormat.Value = MagicStrings.USDateFormat;
        svm.WindDirection.Value = MagicStrings.TowardsWind;
        svm.TemperatureFormat.Value = MagicStrings.Fahrenheit;
        BindingContext = svm;

        if (svm.WindDirection.Value == MagicStrings.TowardsWind)
        {
            WindDirection.Text = AppResources.TowardsWind.FirstCharToUpper();
        }
        else
        {
            WindDirection.Text = AppResources.WithWind.FirstCharToUpper();
        }
    }

    private void EmailAddress_TextChanged(object sender, TextChangedEventArgs e)
    {
        // Update validation message visibility when text changes
        if (emailAddress.Text.Contains('.') && emailAddress.Text.Split('.')[1].Length >= 2)
        {
            // only execute validation if someone has entered at least 2 charaters (minimum) after a period.  This way we aren't showing an error until the user has had the chance to put in an actual address
            UpdateValidationMessageVisibility();
        }
    }

    private void UpdateValidationMessageVisibility()
    {
        var validationBehavior = emailAddress.Behaviors.OfType<CommunityToolkit.Maui.Behaviors.TextValidationBehavior>().FirstOrDefault();
        bool isValid = validationBehavior?.IsValid ?? false;
        bool hasText = !string.IsNullOrWhiteSpace(emailAddress.Text);

        // Show validation message only if there's text AND it's invalid
        // OR if save button was pressed (tracked by a separate flag)
        emailValidationMessage.IsVisible = (hasText && !isValid) || _saveAttempted;

        // You could customize the message based on the error
        if (string.IsNullOrWhiteSpace(emailAddress.Text) && _saveAttempted)
        {
            emailValidationMessage.Text = "Email is required";
        }
        else if (!isValid)
        {
            emailValidationMessage.Text = "Please enter a valid email address";
        }
    }

    // Add a field to track if save was attempted
    private bool _saveAttempted = false;

    private async void save_Pressed(object sender, EventArgs e)
    {
        _saveAttempted = true;

        var validationBehavior = emailAddress.Behaviors.OfType<CommunityToolkit.Maui.Behaviors.TextValidationBehavior>().FirstOrDefault();
        bool isValid = validationBehavior?.IsValid ?? false;
        bool hasText = !string.IsNullOrWhiteSpace(emailAddress.Text);

        UpdateValidationMessageVisibility();

        if (isValid && hasText)
        {
            // Reset the save attempted flag
            _saveAttempted = false;

            // Extract settings values for clarity
            string hemisphere = HemisphereSwitch.IsToggled ? MagicStrings.North : MagicStrings.South;
            string temperatureFormat = TempFormatSwitch.IsToggled ? MagicStrings.Fahrenheit : MagicStrings.Celsius;
            string dateFormat = DateFormat.IsToggled ? MagicStrings.USDateFormat : MagicStrings.InternationalFormat;
            string timeFormat = TimeSwitch.IsToggled ? MagicStrings.USTimeformat_Pattern : MagicStrings.InternationalTimeFormat_Pattern;
            string windDirection = WindDirectionSwitch.IsToggled ? MagicStrings.TowardsWind : MagicStrings.WithWind;
            string email = emailAddress.Text;

            // Show processing indicator
            processingOverlay.IsVisible = true;
            await NativeStorageService.SaveSetting(MagicStrings.Email, email);
            await NativeStorageService.SaveSetting(MagicStrings.UniqueID, Guid.NewGuid().ToString());

            try
            {
                // Run DataPopulation on a background thread
                await Task.Run(() => {
                    DataPopulation.PopulateData(
                        hemisphere,
                        temperatureFormat,
                        dateFormat,
                        timeFormat,
                        windDirection,
                        email
                    );
                    Task.Delay(3000).Wait();
                });

                // Navigate to the main page on the UI thread
                await Navigation.PushAsync(new NavigationPage(new MainPage()));
            }
            catch (Exception ex)
            {
                // Handle any errors
                string errorMessage = "Error processing data";
                if (AppResources.ResourceManager.GetString("ErrorProcessingData") != null)
                {
                    errorMessage = AppResources.ResourceManager.GetString("ErrorProcessingData");
                }

                await DisplayAlert(AppResources.Error,
                                  $"{errorMessage}: {ex.Message}",
                                  AppResources.OK);
            }
            finally
            {
                // Hide processing indicator
                processingOverlay.IsVisible = false;
            }
        }
    }

    private void WindDirectionSwitch_Toggled(object sender, ToggledEventArgs e)
    {

        var vm = (SettingsViewModel)BindingContext;

        vm.WindDirection.Value = e.Value ? MagicStrings.TowardsWind : MagicStrings.WithWind;

        // Notify that the derived bool property changed
     

        WindDirection.Text = e.Value
            ? AppResources.TowardsWind.FirstCharToUpper()
            : AppResources.WithWind.FirstCharToUpper();
    }

    private void TempFormatSwitch_Toggled(object sender, ToggledEventArgs e)
    {
        ((SettingsViewModel)BindingContext).TemperatureFormat.Value = e.Value ? MagicStrings.Fahrenheit : MagicStrings.Celsius;
    }
}