using Location.Core.Helpers;
using Location.Core.Resources;
using Locations.Core.Business.DataAccess;
using Locations.Core.Shared;
using Locations.Core.Shared.Customizations.Alerts.Interfraces;
using Locations.Core.Shared.ViewModels;

namespace Location.Core.Views.DetailViews;

public partial class WeatherDisplay : ContentPage
{
    WeatherService ws = new WeatherService();
    Locations.Core.Business.DataAccess.SettingsService ss = new Locations.Core.Business.DataAccess.SettingsService();
    private IAlertService alertServ;

    public WeatherDisplay()
    {
        InitializeComponent();
    }
    public WeatherDisplay(IAlertService alertServ) : this()
    {
        this.alertServ = alertServ;


    }
    public WeatherDisplay(IAlertService alertServ,  WeatherViewModel weather) : this(weather)
    {
        this.alertServ = alertServ;

    }
    public WeatherDisplay(IAlertService alertServ, LocationViewModel name) : this(name)
    {
        this.alertServ = alertServ;
   
    }
    public WeatherDisplay(WeatherViewModel weather) : this()
    {
        this.BindingContext = weather;
    }
    public WeatherDisplay(LocationViewModel name) : this()
    {
        WeatherViewModel x = ws.GetWeather(name.Lattitude, name.Longitude);
        try
        {
            if (x.IsError)
            {
                DisplayAlert(AppResources.Error, x.alertEventArgs.Message, AppResources.OK);
            }
        }
        catch (Exception ex)
        {
            DisplayAlert(AppResources.Error, AppResources.ErrorUpdatingSetting, AppResources.OK);
        }

        x.WindDirectionArrow = ss.GetSettingByName(MagicStrings.WindDirection).Value;
        this.BindingContext = x;
    }
    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        PageHelpers.CheckVisit(MagicStrings.WeatherDisplayViewed, PageEnums.WeatherDisplay, ss, Navigation);
    }

    private void ImageButton_Pressed(object sender, EventArgs e)
    {
        Navigation.PopModalAsync();
    }
}