using Location.Core.Helpers;
using Location.Core.Resources;
using Location.Photography.Business.DataAccess;
using Locations.Core.Shared.Customizations.Alerts.Interfraces;
using Locations.Core.Shared.Customizations.Logging.Interfaces;
using Locations.Core.Shared;
using Locations.Core.Shared.Enums;
using Microsoft.Maui.Controls;
using OpenWeatherMap;

namespace Location.Core.Views;

public partial class Settings : ContentPage
{
    Locations.Core.Business.DataAccess.SettingsService ss = new Locations.Core.Business.DataAccess.SettingsService();
    private IAlertService alertServ;
    private ILoggerService loggerService;
    public Settings(IAlertService alertServ, ILoggerService loggerService)
    {
        InitializeComponent();
        this.alertServ = alertServ;
        this.loggerService = loggerService;
        
    }

    public Settings()
	{
		InitializeComponent();
	}
    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        PageHelpers.CheckVisit(MagicStrings.SettingsViewed, PageEnums.Settings, new Locations.Core.Business.DataAccess.SettingsService(), Navigation);
        GetSetting();
    }

    private void GetSetting()
    {
        var settings = ss.GetAllSettings();
        BindingContext = settings;

        if(settings.WindDirection.Value == MagicStrings.TowardsWind)
        {
           WindDirection.Text = AppResources.TowardsWind;
        }
        else
        {
            WindDirection.Text = AppResources.WithWind;
        }
    }

    private void HemisphereSwitch_Toggled(object sender, ToggledEventArgs e)
    {
        var x = ss.GetSettingByName(MagicStrings.Hemisphere);
        x.Value = e.Value ? Hemisphere.HemisphereChoices.North.Name() : Hemisphere.HemisphereChoices.South.Name();
        ss.UpdateSetting(x);
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

    private void adsupport_Toggled(object sender, ToggledEventArgs e)
    {
        var x = ss.GetSettingByName(MagicStrings.FreePremiumAdSupported);
        x.Value = e.Value ? MagicStrings.True_string : MagicStrings.False_string;
        ss.UpdateSetting(x);
        GetSetting();
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
        var x =ss.GetSettingByName(MagicStrings.TemperatureType);
        x.Value = e.Value ? MagicStrings.Fahrenheit : MagicStrings.Celsius;
        ss.UpdateSetting(x);
        GetSetting();
    }
}