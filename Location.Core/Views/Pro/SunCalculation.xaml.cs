using Location.Photography.Business.DataAccess;
using Location.Photography.Business.SunCalculator.Interface;
using lpsv = Location.Photography.Shared.ViewModels;
using Locations.Core.Shared.ViewModels;

namespace Location.Core.Views.Pro;

public partial class SunCalculation : ContentPage
{
    SettingsService settingsService = new SettingsService();
    LocationService ls = new LocationService();
    public SunCalculation()
	{
		InitializeComponent();
        DoTheNeedful();
    }
    private void DoTheNeedful()
    {
        lpsv.SunCalculations x = ((lpsv.SunCalculations)BindingContext);
        x.DateFormat = settingsService.GetSettingByName(Locations.Core.Shared.MagicStrings.DateFormat).Value;
        x.Locations = ls.GetLocations();

        LocationsPicker.SelectedIndex = 0;
    }
    public SunCalculation(ISunCalculator sunCalc)
    {
        DoTheNeedful();
    }

    private void LocationsPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        var x = (LocationViewModel)sender;
        var y = (lpsv.SunCalculations)BindingContext;
        y.Latitude = x.Lattitude;
        y.Longitude = x.Longitude;
        y.CalculateSun();
    }

    private void date_DateSelected(object sender, DateChangedEventArgs e)
    {
        var date = e.NewDate;
        var y = (lpsv.SunCalculations)BindingContext;
        y.Date = date;
        y.CalculateSun();
    }
}