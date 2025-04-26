using vm = Location.Photography.Shared.ViewModels;
using Locations.Core.Shared.ViewModels;
using Locations.Core.Business.DataAccess;
using Locations.Core.Shared;
using Locations.Core.Shared.Enums;
using Location.Photography.Shared.ViewModels;
using Location.Photography.Business.DataAccess;
using Location.Core.Helpers;
using System.Collections.ObjectModel;
using CommunityToolkit.Maui.Core.Extensions;

namespace Location.Core.Views.Premium;

public partial class SunLocation : ContentPage
{
   
    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        PageHelpers.CheckVisit(MagicStrings.SunLocationViewed, PageEnums.SunLocation, new Locations.Core.Business.DataAccess.SettingsService(), Navigation);
  
    }

    private void DoTheNeedful()
    {
        Locations.Core.Business.DataAccess.LocationsService ss = new Locations.Core.Business.DataAccess.LocationsService();
        var x = new vm.SunLocation();
        x.List_Locations = ss.GetLocations().ToObservableCollection() ;
        x.SelectedDate = DateOnly.FromDateTime(DateTime.Now);
        x.SelectedTime = TimeOnly.FromDateTime(DateTime.Now);
        date.Date = DateTime.Now;
        time.Time = TimeOnly.FromDateTime(DateTime.Now).ToTimeSpan();
        BindingContext = x;
        locationPicker.SelectedIndex = 0;
    }

    public SunLocation()
    {
        InitializeComponent();

        DoTheNeedful();
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