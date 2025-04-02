using vm = Location.Photography.Shared.ViewModels;
using Locations.Core.Shared.ViewModels;
using Locations.Core.Business.DataAccess;
using Locations.Core.Shared;
using Locations.Core.Shared.Enums;

namespace Location.Core.Views.Premium;

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
        var z = new LocationsService();
        var hemi = y.GetSettingByName(MagicStrings.Hemisphere).Value;
        Item.Hemisphere = hemi;
        arrow.Source = hemi == Hemisphere.HemisphereChoices.North.Name() ? "arrow_clipart_north.png" : "arrow_clipart_south.png";
        date.Format = y.GetSettingByName(MagicStrings.DateFormat).Value;
        time.Format = y.GetSettingByName(MagicStrings.TimeFormat).Value;
        Item.List_Locations = z.GetLocations();
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