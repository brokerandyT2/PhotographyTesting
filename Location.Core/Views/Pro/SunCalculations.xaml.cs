using Location.Photography.Business.DataAccess;
using Location.Photography.Business.SunCalculator.Interface;
using lpsv = Location.Photography.Shared.ViewModels;
using lcbd = Locations.Core.Business.DataAccess;
using Locations.Core.Shared.ViewModels;
using Location.Photography.Shared.ViewModels;
using Locations.Core.Shared.DTO;
using Locations.Core.Shared;
using Location.Core.Helpers;
using Microsoft.Maui.Controls;

using Locations.Core.Shared.ViewModels.Interface;
using Locations.Core.Shared.Customizations.Alerts.Interfraces;
using Locations.Core.Shared.Customizations.Logging.Interfaces;
using Locations.Core.Shared.Customizations.Alerts.Implementation;
using Locations.Core.Shared.Customizations.Logging.Implementation;
using Microsoft.Extensions.Logging;
using Location.Core.Resources;
namespace Location.Core.Views.Pro;

public partial class SunCalculations : ContentPage
{
    lcbd.SettingsService settingsService = new lcbd.SettingsService();
    LocationService ls = new LocationService(new LoggerService(new ServiceCollection().AddLogging().BuildServiceProvider().GetRequiredService<ILogger<LoggerService>>()), new AlertService());
    private IAlertService alertServ;
    private ILoggerService loggerService;
    public SunCalculations(IAlertService alertServ, ILoggerService logger) : this()
    {
        this.alertServ = alertServ;
        this.loggerService = logger;

    }
    public SunCalculations(IAlertService alertServ, ILoggerService logger, LocationViewModel id) : this(id)
    {
        this.alertServ = alertServ;
        this.loggerService = logger;
    }
    public SunCalculations(IAlertService alertServ, ILoggerService logger, ILocationViewModel id) : this(id)
    {
        this.alertServ = alertServ;
        this.loggerService = logger;
    }
    public SunCalculations()
    {
        InitializeComponent();
        DoTheNeedful();

    }
    public SunCalculations(ILocationViewModel id) : this(id as LocationViewModel)
    {
        DoTheNeedful(id as LocationViewModel);
    }
    public SunCalculations(LocationViewModel location)
    {
        InitializeComponent();
        DoTheNeedful(location);
    }
    private void DoTheNeedful(LocationViewModel location)
    {
        Location.Photography.Shared.ViewModels.SunCalculations vm = new lpsv.SunCalculations();
        try
        {
            var loc = ls.GetLocations();

            vm.Locations = loc;
            vm.DateFormat = settingsService.GetSettingByName(MagicStrings.DateFormat).Value;
            vm.TimeFormat = settingsService.GetSettingByName(MagicStrings.TimeFormat).Value;
            vm.Latitude = location.Lattitude;
            vm.Longitude = location.Longitude;
            locationPhoto.Source = location.Photo == string.Empty ? "imageoffoutlinecustom.png" : location.Photo;
            vm.Date = DateTime.Now;
            vm.CalculateSun();

            BindingContext = vm;
            locationPicker.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            DisplayAlert(AppResources.Error, ex.Message, AppResources.OK);
        }
        if (vm.IsError)
        {
            DisplayAlert(AppResources.Error, vm.alertEventArgs.Message, AppResources.OK);
        }
    }
    private void DoTheNeedful()
    {
        Location.Photography.Shared.ViewModels.SunCalculations vm = new lpsv.SunCalculations();
        try
        {
            var loc = ls.GetLocations();

            vm.Locations = loc;
            vm.DateFormat = settingsService.GetSettingByName(MagicStrings.DateFormat).Value;
            vm.TimeFormat = settingsService.GetSettingByName(MagicStrings.TimeFormat).Value;
            vm.Latitude = loc[0].Lattitude;
            vm.Longitude = loc[0].Longitude;
            locationPhoto.Source = loc[0].Photo == string.Empty ? "imageoffoutlinecustom.png" : loc[0].Photo;
            vm.Date = DateTime.Now;
            vm.CalculateSun();

            BindingContext = vm;
            locationPicker.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            DisplayAlert(AppResources.Error, ex.Message, AppResources.OK);
        }
        if (vm.IsError)
        {
            DisplayAlert(AppResources.Error, vm.alertEventArgs.Message, AppResources.OK);
        }
    }
    private void LocationsPicker_SelectedIndexChanged(object sender, EventArgs e)
    {lpsv.SunCalculations x = ((lpsv.SunCalculations)BindingContext);
        try { 
        
        var q = string.Empty;
        var z = ((LocationViewModel)((Picker)sender).SelectedItem);//.Locations[LocationsPicker.SelectedIndex];
        locationPhoto.Source = z.Photo == string.Empty ? "imageoffoutlinecustom.png" : z.Photo;
        x.Latitude = z.Lattitude;
        x.Longitude = z.Longitude;
        x.Date = datePicker.Date;
        x.CalculateSun();
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

    private void datePicker_DateSelected(object sender, DateChangedEventArgs e)
    {
        var date = e.NewDate;
        var y = (lpsv.SunCalculations)BindingContext;
        y.Date = date;
        try
        {
            y.CalculateSun();
        }catch(Exception ex)
        {
            DisplayAlert(AppResources.Error, ex.Message, AppResources.OK);
        }
        if (y.IsError)
        {
            DisplayAlert(AppResources.Error, y.alertEventArgs.Message, AppResources.OK);
        }

    }
    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        PageHelpers.CheckVisit(MagicStrings.SunCalculatorViewed, PageEnums.SunCalculations, settingsService, Navigation);
    }
}