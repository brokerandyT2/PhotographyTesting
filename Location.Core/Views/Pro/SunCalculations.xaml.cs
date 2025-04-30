using Location.Photography.Business.DataAccess;
using Location.Photography.Business.SunCalculator.Interface;
using lpsv = Location.Photography.Shared.ViewModels;
using lcbd = Locations.Core.Business.DataAccess;
using Locations.Core.Shared.ViewModels;
using Location.Photography.Shared.ViewModels;
using Locations.Core.Shared.DTO;
using Locations.Core.Shared;
using Location.Core.Helpers;
using Microsoft.Maui.Controls;
namespace Location.Core.Views.Pro;

public partial class SunCalculations : ContentPage
{
    lcbd.SettingsService settingsService = new lcbd.SettingsService();
    LocationService ls = new LocationService();
    public SunCalculations()
	{
		InitializeComponent();
        DoTheNeedful();

    }
    public SunCalculations(LocationViewModel location)
    {
        InitializeComponent();
        DoTheNeedful(location);
    }
    private void DoTheNeedful(LocationViewModel location)
    {
        var loc = ls.GetLocations();
        Location.Photography.Shared.ViewModels.SunCalculations vm = new lpsv.SunCalculations();
        vm.Locations = loc;
        vm.DateFormat = settingsService.GetSettingByName(MagicStrings.DateFormat).Value;
        vm.TimeFormat = settingsService.GetSettingByName(MagicStrings.TimeFormat).Value;
        vm.Latitude = location.Lattitude;
        vm.Longitude = location.Longitude;
        locationPhoto.Source = location.Photo == string.Empty ? "imageoffoutlinecustom.png" : location.Photo;
        vm.Date = DateTime.Now;
        vm.CalculateSun();

        BindingContext = vm;
        locationPicker.SelectedIndex = 0;
    }
    private void DoTheNeedful()
    {
        var loc = ls.GetLocations();
        Location.Photography.Shared.ViewModels.SunCalculations vm = new lpsv.SunCalculations();
        vm.Locations = loc;
        vm.DateFormat = settingsService.GetSettingByName(MagicStrings.DateFormat).Value;
        vm.TimeFormat = settingsService.GetSettingByName(MagicStrings.TimeFormat).Value;
        vm.Latitude = loc[0].Lattitude;
        vm.Longitude = loc[0].Longitude;
        locationPhoto.Source = loc[0].Photo == string.Empty ? "imageoffoutlinecustom.png" : loc[0].Photo;
        vm.Date = DateTime.Now;
        vm.CalculateSun();

        BindingContext = vm;
        locationPicker.SelectedIndex = 0;
    }
    private void LocationsPicker_SelectedIndexChanged(object sender, EventArgs e)
    {

        lpsv.SunCalculations x = ((lpsv.SunCalculations)BindingContext);
        var q = string.Empty;
        var z = ((LocationViewModel)((Picker)sender).SelectedItem);//.Locations[LocationsPicker.SelectedIndex];
        locationPhoto.Source = z.Photo == string.Empty ? "imageoffoutlinecustom.png" : z.Photo ;
        x.Latitude = z.Lattitude;
        x.Longitude = z.Longitude;
        x.Date = datePicker.Date;
        x.CalculateSun();
//        BindingContext = x;

    }
    
    private void datePicker_DateSelected(object sender, DateChangedEventArgs e)
    {
        var date = e.NewDate;
        var y = (lpsv.SunCalculations)BindingContext;
        y.Date = date;
        y.CalculateSun();
        //BindingContext = y;

    }
    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        PageHelpers.CheckVisit(MagicStrings.SunCalculatorViewed, PageEnums.SunCalculations, settingsService, Navigation);
    }
}