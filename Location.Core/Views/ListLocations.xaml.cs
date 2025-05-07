using Location.Core.Helpers;
using Location.Core.Resources;
using Locations.Core.Shared;
using Locations.Core.Shared.Customizations.Alerts.Interfraces;
using Locations.Core.Shared.ViewModels;
using Locations.Core.Shared.ViewModels.Interface;
using System.Collections.ObjectModel;
using lcbd = Locations.Core.Business.DataAccess;

namespace Location.Core.Views;

public partial class ListLocations : ContentPage
{
    lcbd.LocationsService ls = new lcbd.LocationsService();
    lcbd.SettingsService ss = new lcbd.SettingsService();
    private IAlertService alertServ;

    private ObservableCollection<LocationViewModel> _items = [];
    public ObservableCollection<LocationViewModel> Items { get { return _items; } set { _items = value; } }
    public ListLocations()
    {
        InitializeComponent();
        BindingContext = this;
        CheckVisit();
    }
    public ListLocations(IAlertService alertServ)
    {
        InitializeComponent();

        BindingContext = this;
        CheckVisit();
    }
    private void CheckVisit()
    {
        PageHelpers.CheckVisit(MagicStrings.LocationListViewed, PageEnums.ListLocations, ss, Navigation);
    }

    public ListLocations(ILocationList model)
    {
        InitializeComponent();
        var x = (LocationsListViewModel)model;
        if (Items.Count == 0)
        {
            PopulateData();
        }
        CheckVisit();
    }
    private void PopulateData()
    {
        try
        {
            var loc = ls.GetLocations();
            Items.Clear();
            foreach (var z in loc)
            {
                _items.Add(z);

                if (z.IsError)
                {
                    DisplayAlert(AppResources.Error, AppResources.ErrorUpdatingSetting, AppResources.OK);
                }
            }
            BindingContext = Items;
            cv.ItemsSource = Items;
        }
        catch (Exception ex)
        {
            DisplayAlert(AppResources.Error, AppResources.ErrorUpdatingSetting, AppResources.OK);
        }
    }
       
    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        PopulateData();
      PageHelpers.CheckVisit(MagicStrings.LocationListViewed, PageEnums.ListLocations, ss, Navigation);     

    }
    private void ImageButton_Pressed(object sender, EventArgs e)
    {
        var z = (Microsoft.Maui.Controls.ImageButton)sender;
        var id = (int)z.CommandParameter;
        LocationViewModel yt = new LocationViewModel();

        try
        {
            yt =   ls.Get(id);
            if (yt.IsError)
            {
                DisplayAlert(AppResources.Error, yt.alertEventArgs.Message, AppResources.OK);
            }
        }
        catch (Exception ex)
        {
            DisplayAlert(AppResources.Error, AppResources.ErrorUpdatingSetting, AppResources.OK);
        }
     

        var location = new Microsoft.Maui.Devices.Sensors.Location(yt.Lattitude, yt.Longitude);
        var options = new MapLaunchOptions { Name = yt.Title };
        try
        {
            Map.Default.OpenAsync(location, options).RunSynchronously();
        }
        catch (Exception ex)
        {
            throw new Exception("No Map Application");
        }
    }

    private void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is not LocationViewModel item)
            return;
        var id = ((LocationViewModel)e.CurrentSelection.FirstOrDefault()).Id;

        Navigation.PushModalAsync(new NavigationPage(new Views.EditLocation(id)));
      
    }


}