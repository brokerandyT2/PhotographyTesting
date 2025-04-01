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
	}
	public HoldingPage(IDetailsView locationViewModel)
	{
		BindingContext = (DetailsViewModel)locationViewModel;
		GetDetails();
	}

    private void GetDetails()
    {
       var x = (LocationViewModel)BindingContext;
		this.Children.Add(new AddLocation(x.Id, true));
		this.Children.Add(new WeatherDisplay(x));
		
    }

    public HoldingPage(int id)
	{
		BindingContext = ls.Get(id);
        GetDetails();
    }
}