using vm = Location.Photography.Shared.ViewModels;
using Locations.Core.Shared.ViewModels;
using Location.Photography.Data.Queries;
using Location.Photography.Business.DataAccess;
using Locations.Core.Shared;
namespace Location.Photography.UI.Views.Premium;

public partial class SunLocation : ContentPage
{
    public vm.SunLocation Item
    {
        get => BindingContext as vm.SunLocation;
        set
        {
            BindingContext = value;

        }

    }
    public SunLocation()
    {
        InitializeComponent();

        var y = new SettingsService();
        var z = new LocationService();
        var hemi = y.GetSettingByName(MagicStrings.Hemisphere).Value;
        Item.Hemisphere = hemi;
        date.Format = y.GetSettingByName(MagicStrings.DateFormat).Value;
        time.Format = y.GetSettingByName(MagicStrings.TimeFormat).Value;
        Item.List_Locations = z.GetAll();
    }

    private void locationPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        var x = (LocationViewModel)sender;
        Item.Latitude = x.Lattitude;
        Item.Longitude = x.Longitude;
        Item.Calculate();
    }

    private void date_DateSelected(object sender, DateChangedEventArgs e)
    {
        Item.SelectedDate = DateOnly.FromDateTime(e.NewDate);
        Item.Calculate();
    }

    private void time_TimeSelected(object sender, TimeChangedEventArgs e)
    {
        Item.SelectedTime = new TimeOnly(e.NewTime.Hours, e.NewTime.Minutes, 0);
        Item.Calculate();
    }
}