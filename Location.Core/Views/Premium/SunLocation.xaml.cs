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
using Microsoft.Maui.Controls;

namespace Location.Core.Views.Premium;

public partial class SunLocation : ContentPage
{
   Locations.Core.Business.DataAccess.LocationsService ss = new Locations.Core.Business.DataAccess.LocationsService();
    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        PageHelpers.CheckVisit(MagicStrings.SunLocationViewed, PageEnums.SunLocation, new Locations.Core.Business.DataAccess.SettingsService(), Navigation);
  
    }

    private void DoTheNeedful()
    {
        
        var x = new vm.SunLocation();

        x.SelectedDateTime = GetDate();

        date.Date = DateTime.Now;
        time.Time = TimeOnly.FromDateTime(DateTime.Now).ToTimeSpan();
        BindingContext = x;
        locationPicker.SelectedIndex = 0;
    }

    public SunLocation()
    {
        InitializeComponent();
        BindingContext = new vm.SunLocation();
        var loc = ss.GetLocations();
        locationPicker.ItemsSource = loc;
        date.Date = DateTime.Now;
        time.Time = TimeOnly.FromDateTime(DateTime.Now).ToTimeSpan();
        locationPicker.SelectedIndex = 0;

    }

    private void locationPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        var y = ((vm.SunLocation)BindingContext);
        var x = (LocationViewModel)((Picker)sender).SelectedItem;
        y.Latitude = x.Lattitude;
        y.Longitude = x.Longitude;
        y.SelectedDateTime = GetDate();
        //y.Calculate();
    }

    private DateTime GetDate()
    {
        return new DateTime(date.Date.Year, date.Date.Month, date.Date.Day, time.Time.Hours, time.Time.Minutes, 0);
    }

    private void date_DateSelected(object sender, DateChangedEventArgs e)
    {
        var y = ((vm.SunLocation)BindingContext);
        y.SelectedDate = date.Date;
        y.SelectedDateTime = GetDate();
        //y.Calculate();
    }

    private void time_TimeSelected(object sender, TimeChangedEventArgs e)
    {
        var y = ((vm.SunLocation)BindingContext);
        y.SelectedDateTime = GetDate();
        //y.Calculate();
    }

    private void ImageButton_Pressed(object sender, EventArgs e)
    {
        Navigation.PopModalAsync();
    }
}