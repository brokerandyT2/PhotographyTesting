using Locations.Core.Business.DataAccess;
using Locations.Core.Shared;
using Locations.Core.Shared.ViewModels;
using Locations.Core.Shared.ViewModels.Interface;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Location.Core.Views;

public partial class ListLocations : ContentPage
{
    LocationsService ls = new LocationsService();
    SettingsService ss = new SettingsService();
    private ObservableCollection<LocationViewModel> _items = [];
    public ObservableCollection<LocationViewModel> Items { get { return _items; } set { _items = value; } }
    public ListLocations()
    {
        InitializeComponent();
        BindingContext = this;
    }
    public ListLocations(ILocationList model)
    {
        InitializeComponent();
        var x = (LocationsListViewModel)model;
        if (Items.Count == 0)
        {
            PopulateData();
        }
    }
    private void PopulateData()
    {
        var loc = ls.GetLocations();
        _items.Clear();
        foreach (var z in loc)
        {
            _items.Add(z);
        }
        BindingContext = this;
    }
    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        PopulateData();
        var x = ss.GetSettingByName(MagicStrings.LocationListViewed);
        if(x.ToBoolean() == false)
        {
#if RELEASE
            Navigation.PushModalAsync(new Views.DetailViews.HoldingPage(0));      
#endif
            x.Value = MagicStrings.True_string;
#if RELEASE
            ss.UpdateSetting(xx);
#endif
        }

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

        Navigation.PushModalAsync(new Views.DetailViews.HoldingPage(id));
    }


}