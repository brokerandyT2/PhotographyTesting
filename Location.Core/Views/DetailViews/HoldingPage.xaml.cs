using Locations.Core.Business.DataAccess;
using Locations.Core.Shared.ViewModels;
using Locations.Core.Shared.ViewModels.Interface;


namespace Location.Core.Views.DetailViews;

public partial class HoldingPage : TabbedPage
{
	
	public HoldingPage()
	{
		InitializeComponent();
		BindingContext = new DetailsViewModel();
	}
	
}