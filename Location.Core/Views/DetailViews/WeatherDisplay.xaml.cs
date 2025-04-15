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
    public WeatherDisplay(WeatherViewModel weather) : this()
    {
        this.BindingContext = weather;
    }
    public WeatherDisplay(LocationViewModel name) : this()
    {
        var x = ws.GetWeather(name.Lattitude, name.Longitude);
        // WeatherControl.BindingContext = x;
        this.BindingContext = x;
    }
    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        var x = ss.GetSettingByName(MagicStrings.WeatherDisplayViewed);
        var z = ss.GetSettingByName(MagicStrings.FreePremiumAdSupported);
        var isAds = z.ToBoolean();
        if (x.ToBoolean() == false)
        {
            Navigation.PushModalAsync(new Views.DetailViews.HoldingPage(0));
            x.Value = MagicStrings.True_string;
            ss.UpdateSetting(x);

        }

    }
}