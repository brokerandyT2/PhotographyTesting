using Location.Core.Helpers;
using Locations.Core.Shared;

namespace Location.Core.Views;

public partial class Settings : ContentPage
{
	public Settings()
	{
		InitializeComponent();
	}
    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        PageHelpers.CheckVisit(MagicStrings.SettingsViewed, PageEnums.Settings, new Locations.Core.Business.DataAccess.SettingsService(), Navigation);

    }
}