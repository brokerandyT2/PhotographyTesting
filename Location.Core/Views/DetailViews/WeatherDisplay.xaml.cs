using Location.Core.Helpers;
using Location.Photography.Business.DataAccess;
using Locations.Core.Business.DataAccess;
using Locations.Core.Shared;
using Locations.Core.Shared.ViewModels;

namespace Location.Core.Views.DetailViews;

public partial class WeatherDisplay : ContentPage
{
    WeatherService ws = new WeatherService();
    Locations.Core.Business.DataAccess.SettingsService ss = new Locations.Core.Business.DataAccess.SettingsService();
    public WeatherDisplay()
    {
        InitializeComponent();
    }
    public WeatherDisplay(WeatherViewModel weather) : this()
    {
        this.BindingContext = weather;
    }
    public WeatherDisplay(LocationViewModel name) : this()
    {
        var x = ws.GetWeather(name.Lattitude, name.Longitude);
        x.WindDirectionArrow = ss.GetSettingByName(MagicStrings.WindDirection).Value;
        // WeatherControl.BindingContext = x;
        this.BindingContext = x;
    }
    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        PageHelpers.CheckVisit(MagicStrings.WeatherDisplayViewed, PageEnums.WeatherDisplay, ss, Navigation);
    }
}