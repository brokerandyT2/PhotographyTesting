using Location.Photography.Business;
using Location.Photography.Business.DataAccess;
using Location.Photography.Business.SunCalculator.Interface;
using Location.Photography.Shared;
using Location.Photography.Shared.ViewModels;
using Locations.Core.Shared.ViewModels;


namespace Location.Photography.UI.Views.Pro;

public partial class SunCalculation : ContentPage
{

    SettingsService settingsService = new SettingsService();
    LocationService ls = new LocationService();
    public SunCalculation()
    {
        InitializeComponent();
    }
    public SunCalculation(ISunCalculator sunCalc)
    {
        SunCalculations x = ((SunCalculations)sunCalc);

        x.DateFormat = settingsService.GetSettingByName(Locations.Core.Shared.MagicStrings.DateFormat).Value;
        x.Locations = ls.GetLocations();

        LocationsPicker.SelectedIndex = 0;
    }

    private void LocationsPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        var x = (LocationViewModel)sender;
        var y = (SunCalculations)BindingContext;
        y.Latitude = x.Lattitude;
        y.Longitude = x.Longitude;
        y.CalculateSun();
    }

    private void date_DateSelected(object sender, DateChangedEventArgs e)
    {
        var date = e.NewDate;
        var y = (SunCalculations)BindingContext;
        y.Date = date;
        y.CalculateSun();
    }
}