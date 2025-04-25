using Location.Photography.Business.DataAccess;
using lcbd = Locations.Core.Business.DataAccess;
using Locations.Core.Shared;
using Locations.Core.Shared.ViewModels;
using Locations.Core.Shared.ViewModels.Interface;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Location.Core.Helpers;
using Locations.Core.Business.DataAccess;

namespace Location.Core.Views;

public partial class ListLocations : ContentPage
{
    lcbd.LocationsService ls = new lcbd.LocationsService();
    lcbd.SettingsService ss = new lcbd.SettingsService();
    private ObservableCollection<LocationViewModel> _items = [];
    public ObservableCollection<LocationViewModel> Items { get { return _items; } set { _items = value; } }
    public ListLocations()
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
        var loc = ls.GetLocations();
        Items.Clear();
        foreach (var z in loc)
        {
            _items.Add(z);
        }
        BindingContext = Items;
        cv.ItemsSource = Items;
    }
    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        PopulateData();
        var x = ss.GetSettingByName(MagicStrings.LocationListViewed);
        if(x.ToBoolean() == false)
        {
            Navigation.PushModalAsync(new Views.DetailViews.HoldingPage(0));
            x.Value = MagicStrings.True_string;
            ss.Save(x);
        }
        CheckVisit();

    }
    private void ImageButton_Pressed(object sender, EventArgs e)
    {
        var z = (Microsoft.Maui.Controls.ImageButton)sender;
        var id = (int)z.CommandParameter;

        LocationViewModel yt = ls.Get(id);

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