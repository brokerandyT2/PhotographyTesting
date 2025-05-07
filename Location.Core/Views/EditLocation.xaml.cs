using Locations.Core.Shared.Customizations.Alerts.Interfraces;
using Locations.Core.Shared.Customizations.Logging.Interfaces;
using Locations.Core.Business.DataAccess;
using Locations.Core.Shared.ViewModels;
using Location.Core.Resources;

namespace Location.Core.Views;

public partial class EditLocation : ContentPage
{
    private int id;
    LocationsService locationService = new LocationsService();
    private IAlertService alertServ;
    private ILoggerService loggerService;
    public EditLocation()
	{
		InitializeComponent();
	}
    public EditLocation(IAlertService alertServ, ILoggerService logger)
    {
        this.alertServ = alertServ;
        this.loggerService = logger;
        InitializeComponent();
    }
    public EditLocation(IAlertService alertSer, ILoggerService logger, int id) : this(id)
    {
        alertServ = alertSer;
        this.loggerService = logger;
    }
    public EditLocation(int id)
    {
        LocationViewModel x = new LocationViewModel();
        InitializeComponent();
        this.id = id;
        try
        {
            x = locationService.Get(id);
            BindingContext = x;
        }
        catch (Exception ex)
        {
            DisplayAlert(AppResources.Error, ex.Message, AppResources.OK);
        }
        if (x.IsError)
        {
            DisplayAlert(AppResources.Error, x.alertEventArgs.Message, AppResources.OK);
        }
    }
    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        LocationViewModel x = new LocationViewModel();
        try
        {
            x = locationService.Get(id);
            BindingContext = x;
        }
        catch (Exception ex)
        {
            DisplayAlert(AppResources.Error, ex.Message, AppResources.OK);
        }
        if (x.IsError)
        {
            DisplayAlert(AppResources.Error, x.alertEventArgs.Message, AppResources.OK);
        }
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

    private void ImageButton_Pressed(object sender, EventArgs e)
    {
        Navigation.PushAsync(new MainPage());
    }
}