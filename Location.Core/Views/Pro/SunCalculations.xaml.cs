using Location.Photography.Business.DataAccess;
using Location.Photography.Business.SunCalculator.Interface;
using lpsv = Location.Photography.Shared.ViewModels;
using lcbd = Locations.Core.Business.DataAccess;
using Locations.Core.Shared.ViewModels;
using Location.Photography.Shared.ViewModels;
using Locations.Core.Shared.DTO;
using Locations.Core.Shared;
using Location.Core.Helpers;
namespace Location.Core.Views.Pro;

public partial class SunCalculations : ContentPage
{
    lcbd.SettingsService settingsService = new lcbd.SettingsService();
    LocationService ls = new LocationService();
    public SunCalculations()
	{
		InitializeComponent();
        DoTheNeedful();
        LocationsPicker.SelectedIndex = 0;
        
        var page = settingsService.GetSettingByName(MagicStrings.SunCalculatorViewed);

    }
    private void DoTheNeedful()
    {
        DateTime no = DateTime.Now;
        var q = no.ToString("hh:mm tt");

        var x = new lpsv.SunCalculations();
        x.DateFormat = settingsService.GetSettingByName(Locations.Core.Shared.MagicStrings.DateFormat).Value;
        x.TimeFormat = settingsService.GetSettingByName(Locations.Core.Shared.MagicStrings.TimeFormat).Value;
        x.Locations = ls.GetLocations();
        x.Date = date.Date = DateTime.Now;
        date.Format = x.DateFormat;

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
        base.OnNavigatedTo(args);
        PageHelpers.CheckVisit(MagicStrings.SunCalculatorViewed, PageEnums.SunCalculations, settingsService, Navigation);
        PageHelpers.ShowAD(settingsService.GetSettingByName(MagicStrings.FreePremiumAdSupported).ToBoolean(), Navigation);
    }
}