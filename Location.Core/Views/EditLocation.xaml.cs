using Locations.Core.Business.DataAccess;
using Locations.Core.Shared.ViewModels;

namespace Location.Core.Views;

public partial class EditLocation : ContentPage
{
    private int id;
    LocationsService locationService = new LocationsService();

    public EditLocation()
	{
		InitializeComponent();
	}

    public EditLocation(int id)
    {
        InitializeComponent();
        this.id = id;
        var x = locationService.Get(id);
        BindingContext = x;
    }
    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        var x = locationService.Get(id);
        BindingContext = x;
    }

    private void WeatherButton_Pressed(object sender, EventArgs e)
    {
        var y = (LocationViewModel)BindingContext;
        Navigation.PushModalAsync(new DetailViews.WeatherDisplay(y));
    }

    private void SunEvents_Pressed(object sender, EventArgs e)
    {
        var y = (LocationViewModel)BindingContext;
        Navigation.PushModalAsync(new Pro.SunCalculations(y));
    }
}