using Locations.Core.Business.DataAccess;
using Locations.Core.Shared;
using Locations.Core.Shared.ViewModels;

namespace Location.Core.Views.DetailViews;

public partial class WeatherDisplay : ContentPage
{
    WeatherService ws = new WeatherService();
    SettingsService ss = new SettingsService();
    public WeatherDisplay()
    {
        InitializeComponent();
    }

    public WeatherDisplay(LocationViewModel name)
    {
        var x = ws.GetWeather(name.Lattitude, name.Longitude);
        x.DateFormat = ss.GetSettingByName(MagicStrings.DateFormat).Value;
        
        BindingContext = x;
    }
}