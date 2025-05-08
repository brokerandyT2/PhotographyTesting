using AndroidX.Camera.View.Internal.Compat.Quirk;
using CommunityToolkit.Maui.Behaviors;
using Location.Core.Resources;
using Locations.Core.Business;
using Locations.Core.Business.StorageSvc;
using Locations.Core.Shared;
using Locations.Core.Shared.Customizations.Alerts.Implementation;
using Locations.Core.Shared.Customizations.Alerts.Interfraces;
using Locations.Core.Shared.Enums;
using Locations.Core.Shared.ViewModels;
namespace Location.Core.Views;

public partial class Login : ContentPage
{
    Locations.Core.Business.DataAccess.SettingsService ss = new Locations.Core.Business.DataAccess.SettingsService();
    private IAlertService alertServ;

    public Login()
    {
        InitializeComponent();
        ss.AlertRaised += Ss_AlertRaised;
        // BindingContext = new SettingsViewModel();
        GetSetting();
    }

    private void Ss_AlertRaised(object? sender, AlertEventArgs e)
    {
        DisplayAlert(e.Title, e.Message, AppResources.OK);
    }

    public Login(IAlertService alert) : this()
    {
        alertServ = alert;

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
        var x = new Style(Content.GetType());



        var entry = new Entry();

        var validStyle = new Style(typeof(Entry));
        validStyle.Setters.Add(new Setter
        {
            Property = Entry.TextColorProperty,
            Value = Colors.Green
        });

        var invalidStyle = new Style(typeof(Entry));
        invalidStyle.Setters.Add(new Setter
        {
            Property = Entry.TextColorProperty,
            Value = Colors.Red
        });

        var textValidationBehavior = new TextValidationBehavior
        {
            InvalidStyle = invalidStyle,
            ValidStyle = validStyle,
            Flags = ValidationFlags.ValidateOnValueChanged,
            MinimumLength = 1,
            MaximumLength = 10,
            RegexPattern = MagicStrings.RegEx_Email
        };

        entry.Behaviors.Add(textValidationBehavior);

        Content = entry;
    }

    private void WindDirectionSwitch_Toggled(object sender, ToggledEventArgs e)
    {

        ((SettingsViewModel)BindingContext).WindDirection.Value = e.Value ? MagicStrings.TowardsWind : MagicStrings.WithWind;
        if (((SettingsViewModel)BindingContext).WindDirection.Value == MagicStrings.TowardsWind)
        {
            WindDirection.Text = AppResources.TowardsWind.FirstCharToUpper();
        }
        else
        {
            WindDirection.Text = AppResources.WithWind.FirstCharToUpper();
        }

    }

    private void TempFormatSwitch_Toggled(object sender, ToggledEventArgs e)
    {
        ((SettingsViewModel)BindingContext).TemperatureFormat.Value = e.Value ? MagicStrings.Fahrenheit : MagicStrings.Celsius;

    }

    private void save_Pressed(object sender, EventArgs e)
    {
        /* Validation i
         * 
         * 
         * is 
         * 
         * 
         * not 
         * 
         * 
         * working */
        var validationBehavior = emailAddress.Behaviors.OfType<TextValidationBehavior>().FirstOrDefault();
        bool isValid = validationBehavior?.IsValid ?? false;

        if (isValid)
        {
            //  DataPopulation.PopulateData(HemisphereSwitch.IsToggled ? MagicStrings.North : MagicStrings.South,                TempFormatSwitch.IsToggled ? MagicStrings.Fahrenheit : MagicStrings.Celsius,           DateFormat.IsToggled ? MagicStrings.USTimeformat_Pattern : MagicStrings.InternationalFormat,TimeSwitch.IsToggled ? MagicStrings.USTimeformat_Pattern : MagicStrings.InternationalTimeFormat_Pattern, WindDirectionSwitch.IsToggled ? MagicStrings.TowardsWind : MagicStrings.WithWind, emailAddress.Text);
            //  Navigation.PushAsync(new NavigationPage(new MainPage()));
        }





        if (!string.IsNullOrEmpty(emailAddress.Text))
        {
            UpdateEmail();
           
        }
    }



    private void UpdateEmail()
    {

        var x = ss.GetSettingByName(MagicStrings.Email);
        x.Value = emailAddress.Text;
        SecureStorage.SetAsync(MagicStrings.Email, x.Value);
        try
        {
            ss.UpdateSetting(x);
            if (x.IsError)
            {
                DisplayAlert(AppResources.Error, x.alertEventArgs.Message, AppResources.OK);
            }
        }
        catch (Exception ex)
        {
            DisplayAlert(AppResources.Error, AppResources.ErrorUpdatingSetting, AppResources.OK);

        }
        if (x.IsError)
        {
            DisplayAlert(AppResources.Error, x.alertEventArgs.Message, AppResources.OK);
        }
    }
}
