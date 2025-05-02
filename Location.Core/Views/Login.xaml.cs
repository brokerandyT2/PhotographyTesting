using Location.Core.Helpers;
using Location.Core.Resources;
using Locations.Core.Business.DataAccess;
using Locations.Core.Shared;
using Locations.Core.Shared.Enums;
using Microsoft.Maui.Authentication;
using Microsoft.Maui.Controls;
using System.Diagnostics;
using System.Net.Mail;

namespace Location.Core.Views;

public partial class Login : ContentPage
{
    Locations.Core.Business.DataAccess.SettingsService ss = new Locations.Core.Business.DataAccess.SettingsService();
    public Login()
    {
        InitializeComponent();
    }

   

    private void HemisphereSwitch_Toggled(object sender, ToggledEventArgs e)
    {
        var x = ss.GetSettingByName(MagicStrings.Hemisphere);
        x.Value = e.Value ? Hemisphere.HemisphereChoices.North.Name() : Hemisphere.HemisphereChoices.South.Name();
        ss.UpdateSetting(x);
        
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
        ss.UpdateSetting(x);
        GetSetting();
    }

    private void DateFormat_Toggled(object sender, ToggledEventArgs e)
    {
        var x = ss.GetSettingByName(MagicStrings.DateFormat);
        x.Value = e.Value ? MagicStrings.USDateFormat : MagicStrings.InternationalFormat;
        ss.UpdateSetting(x);
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
    }

    private void WindDirectionSwitch_Toggled(object sender, ToggledEventArgs e)
    {
        var x = ss.GetSettingByName(MagicStrings.WindDirection);
        x.Value = e.Value ? MagicStrings.TowardsWind : MagicStrings.WithWind;
        ss.UpdateSetting(x);
        GetSetting();

    }

    private void TempFormatSwitch_Toggled(object sender, ToggledEventArgs e)
    {
        var x = ss.GetSettingByName(MagicStrings.TemperatureType);
        x.Value = e.Value ? MagicStrings.Fahrenheit : MagicStrings.Celsius;
        ss.UpdateSetting(x);
        GetSetting();
    }

    private  void save_Pressed(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(emailAddress.Text))
        {
            var x = ss.GetSettingByName(MagicStrings.Email);
            x.Value = string.IsNullOrEmpty(emailAddress.Text)? MagicStrings.NoEmailEntered: emailAddress.Text;
            ss.UpdateSetting(x);
        }
        
        Navigation.PushAsync(new NavigationPage(new MainPage()));
        //Navigation.PushAsync(new MainPage());
    }
}
