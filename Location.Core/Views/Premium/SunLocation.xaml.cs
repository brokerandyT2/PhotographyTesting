using vm = Location.Photography.Shared.ViewModels;
using Locations.Core.Shared.ViewModels;
using Locations.Core.Business.DataAccess;
using Locations.Core.Shared;
using Locations.Core.Shared.Enums;
using Location.Photography.Shared.ViewModels;
using Location.Photography.Business.DataAccess;
using Location.Core.Helpers;

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
    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        Locations.Core.Business.DataAccess.SettingsService ss = new Locations.Core.Business.DataAccess.SettingsService();
        base.OnNavigatedTo(args);
        var x = ss.GetSettingByName(MagicStrings.SunLocationViewed);
        var z = ss.GetSettingByName(MagicStrings.FreePremiumAdSupported);

        PageHelpers.CheckVisit(MagicStrings.SunLocationViewed, PageEnums.SunLocation, ss, Navigation);
        PageHelpers.ShowAD(ss.GetSettingByName(MagicStrings.FreePremiumAdSupported).ToBoolean(), Navigation);

    }
    public SunLocation()
    {
        InitializeComponent();

        var y = new Locations.Core.Business.DataAccess.SettingsService();
        var z = new LocationsService();
        var q = new vm.SunLocation();
        var hemi = y.GetSettingByName(MagicStrings.Hemisphere).Value;
        q.Hemisphere = hemi;
        arrow.Source = hemi == Hemisphere.HemisphereChoices.North.Name() ? "arrow_clipart_north.png" : "arrow_clipart_south.png";
        date.Format = y.GetSettingByName(MagicStrings.DateFormat).Value;
        date.Date = DateTime.Now;
        time.Format = y.GetSettingByName(MagicStrings.TimeFormat).Value;
        time.Time = DateTime.Now.TimeOfDay;
        q.List_Locations = z.GetLocations();
        BindingContext = q;
        locationPicker.SelectedIndex = 0;
    }

    private void locationPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        var y = ((vm.SunLocation)BindingContext);
        var x = (LocationViewModel)((Picker)sender).SelectedItem;
        y.Latitude = x.Lattitude;
        y.Longitude = x.Longitude;
        y.Calculate();
    }

    private void date_DateSelected(object sender, DateChangedEventArgs e)
    {
        var y = ((vm.SunLocation)BindingContext);
        y.SelectedDate = DateOnly.FromDateTime(e.NewDate);
        y.Calculate();
    }

    private void time_TimeSelected(object sender, TimeChangedEventArgs e)
    {
        var y = ((vm.SunLocation)BindingContext);
        y.SelectedTime = new TimeOnly(e.NewTime.Hours, e.NewTime.Minutes, 0);
        y.Calculate();
    }
}