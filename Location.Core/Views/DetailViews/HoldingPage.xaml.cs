using Locations.Core.Business.DataAccess;
using Locations.Core.Shared.ViewModels;
using Locations.Core.Shared.ViewModels.Interface;


namespace Location.Core.Views.DetailViews;

public partial class HoldingPage : TabbedPage
{
	LocationsService ls = new LocationsService();
	public HoldingPage()
	{
		InitializeComponent();
		BindingContext = new DetailsViewModel();
	}
	public HoldingPage(IDetailsView locationViewModel)
	{
		BindingContext = (DetailsViewModel)locationViewModel;
		GetDetails();
	}

    private void GetDetails()
    {
        var x = (LocationViewModel)BindingContext;
		//Children.Add(new AddLocation(x.Id, true));
		//WeatherService ws = new WeatherService();
		//var y = ws.GetWeather(x.Lattitude, x.Longitude);
        //Children.Add(new WeatherDisplay(y));
		
    }

    public HoldingPage(int id)
	{
		BindingContext = ls.Get(id);
        GetDetails();
    }
}