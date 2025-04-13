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
    public WeatherDisplay(WeatherViewModel weather): this()
    {
        this.BindingContext = weather;
    }
    public WeatherDisplay(LocationViewModel name) : this()
    {
        var x = ws.GetWeather(name.Lattitude, name.Longitude);
       // WeatherControl.BindingContext = x;
        this.BindingContext = x;
    }
}