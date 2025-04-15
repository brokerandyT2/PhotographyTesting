using Location.Photography.Business.DataAccess;
using Location.Photography.Business.SunCalculator.Interface;
using lpsv = Location.Photography.Shared.ViewModels;
using Locations.Core.Shared.ViewModels;
using Location.Photography.Shared.ViewModels;
using Locations.Core.Shared.DTO;
using Locations.Core.Shared;
namespace Location.Core.Views.Pro;

public partial class SunCalculations : ContentPage
{
    SettingsService settingsService = new SettingsService();
    LocationService ls = new LocationService();
    public SunCalculations()
	{
		InitializeComponent();
        DoTheNeedful();
        //LocationsPicker.SelectedIndex = 0;
    }
    private void DoTheNeedful()
    {
        DateTime no = DateTime.Now;
        var q = no.ToString("hh:mm tt");

        var x = new lpsv.SunCalculations();
        x.DateFormat = settingsService.GetSettingByName(Locations.Core.Shared.MagicStrings.DateFormat).Value;
        x.TimeFormat = settingsService.GetSettingByName(Locations.Core.Shared.MagicStrings.TimeFormat).Value;
        x.Locations = ls.GetLocations();
       // x.Date = date.Date = DateTime.Now;
       // date.Format = x.DateFormat;

        x.Latitude = x.Locations[0].Lattitude;
        x.Longitude = x.Locations[0].Longitude;
        x.CalculateSun();
        BindingContext = x;
    }
    private void LocationsPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        lpsv.SunCalculations x = ((lpsv.SunCalculations)BindingContext);
        var q = string.Empty;
        var z = ((LocationViewModel)((Picker)sender).SelectedItem);//.Locations[LocationsPicker.SelectedIndex];
        x.Latitude = z.Lattitude;
        x.Longitude = z.Longitude;
        x.CalculateSun();

    }

    private void date_DateSelected(object sender, DateChangedEventArgs e)
    {
        var date = e.NewDate;
        var y = (lpsv.SunCalculations)BindingContext;
        y.Date = date;
        y.CalculateSun();

    }
    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        Locations.Core.Business.DataAccess.SettingsService ss = new Locations.Core.Business.DataAccess.SettingsService();
        base.OnNavigatedTo(args);
        var x = ss.GetSettingByName(MagicStrings.SunCalculatorViewed);
        var z = ss.GetSettingByName(MagicStrings.FreePremiumAdSupported);
        var isAds = z.ToBoolean();
        if (x.ToBoolean() == false)
        {
            //Navigation.PushModalAsync(new Views.DetailViews.HoldingPage(0));
            x.Value = MagicStrings.True_string;
            ss.UpdateSetting(x);
        }

    }
}