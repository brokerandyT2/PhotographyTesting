using CommunityToolkit.Maui.Behaviors;
using Location.Core.Resources;
using Locations.Core.Shared;
using Locations.Core.Shared.Customizations.Alerts.Implementation;
using Locations.Core.Shared.Customizations.Alerts.Interfraces;
using Locations.Core.Shared.Enums;
namespace Location.Core.Views;

public partial class Login : ContentPage
{
    Locations.Core.Business.DataAccess.SettingsService ss = new Locations.Core.Business.DataAccess.SettingsService();
    private IAlertService alertServ;

    public Login()
    {
        InitializeComponent();
        ss.AlertRaised += Ss_AlertRaised;
    }

    private void Ss_AlertRaised(object? sender, AlertEventArgs e)
    {
        DisplayAlert(e.Title, e.Message, AppResources.OK);
    }

    public Login(IAlertService alert): this()
    {
        alertServ = alert;

        InitializeComponent();
    }



    private void HemisphereSwitch_Toggled(object sender, ToggledEventArgs e)
    {
        var x = ss.GetSettingByName(MagicStrings.Hemisphere);
        x.Value = e.Value ? Hemisphere.HemisphereChoices.North.Name() : Hemisphere.HemisphereChoices.South.Name();
        try
        {
            ss.UpdateSetting(x);
        }
        catch (Exception ex)
        {
            DisplayAlert(AppResources.Error, AppResources.ErrorUpdatingSetting, AppResources.OK);

        }
        if (x.IsError)
        {
            DisplayAlert(AppResources.Error, x.alertEventArgs.Message, AppResources.OK);
        }

        GetSetting();

    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        GetSetting();
    }
    private void TimeSwitch_Toggled(object sender, ToggledEventArgs e)
    {
        var x = ss.GetSettingByName(MagicStrings.TimeFormat);
        x.Value = e.Value ? MagicStrings.USTimeformat_Pattern : MagicStrings.InternationalTimeFormat_Pattern;
        try
        {
            ss.UpdateSetting(x);
        }
        catch (Exception ex)
        {
            DisplayAlert(AppResources.Error, AppResources.ErrorUpdatingSetting, AppResources.OK);

        }
        if (x.IsError)
        {
            DisplayAlert(AppResources.Error, x.alertEventArgs.Message, AppResources.OK);
        }
        GetSetting();
    }

    private void DateFormat_Toggled(object sender, ToggledEventArgs e)
    {
        var x = ss.GetSettingByName(MagicStrings.DateFormat);
        x.Value = e.Value ? MagicStrings.USDateFormat : MagicStrings.InternationalFormat;
        try
        {
            ss.UpdateSetting(x);
        }
        catch (Exception ex)
        {
            DisplayAlert(AppResources.Error, AppResources.ErrorUpdatingSetting, AppResources.OK);

        }
        if (x.IsError)
        {
            DisplayAlert(AppResources.Error, x.alertEventArgs.Message, AppResources.OK);
        }
        GetSetting();
    }

    private void GetSetting()
    {
        var settings = ss.GetAllSettings();
        BindingContext = settings;

        if (settings.WindDirection.Value == MagicStrings.TowardsWind)
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
        var x = ss.GetSettingByName(MagicStrings.WindDirection);
        x.Value = e.Value ? MagicStrings.TowardsWind : MagicStrings.WithWind;
        try
        {
            ss.UpdateSetting(x);
        }
        catch (Exception ex)
        {
            DisplayAlert(AppResources.Error, AppResources.ErrorUpdatingSetting, AppResources.OK);

        }
        if (x.IsError)
        {
            DisplayAlert(AppResources.Error, x.alertEventArgs.Message, AppResources.OK);
        }
        GetSetting();

    }

    private void TempFormatSwitch_Toggled(object sender, ToggledEventArgs e)
    {
        var x = ss.GetSettingByName(MagicStrings.TemperatureType);
        x.Value = e.Value ? MagicStrings.Fahrenheit : MagicStrings.Celsius;
        try
        {
            ss.UpdateSetting(x);
        }
        catch (Exception ex)
        {
            DisplayAlert(AppResources.Error, AppResources.ErrorUpdatingSetting, AppResources.OK);

        }
        if (x.IsError)
        {
            DisplayAlert(AppResources.Error, x.alertEventArgs.Message, AppResources.OK);
        }
        GetSetting();
    }

    private void save_Pressed(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(emailAddress.Text))
        {
            UpdateEmail();
        }

        Navigation.PushAsync(new NavigationPage(new MainPage()));

    }

    private void ImageButton_Pressed(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(emailAddress.Text))
        {

            Navigation.PopModalAsync();


        }
        else
        {
            UpdateEmail();
        }

    }

    private void UpdateEmail()
    {
        
        var x = ss.GetSettingByName(MagicStrings.Email);
        x.Value = string.IsNullOrEmpty(emailAddress.Text) ? MagicStrings.NoEmailEntered : emailAddress.Text;
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
