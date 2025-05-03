using CommunityToolkit.Mvvm.ComponentModel;
using Location.Core.Helpers;
using Locations.Core.Business.DataAccess;
using Locations.Core.Shared;
using Locations.Core.Shared.Customizations.Alerts.Implementation;
using Locations.Core.Shared.Customizations.Alerts.Interfraces;
using Locations.Core.Shared.Customizations.Logging.Interfaces;
using Locations.Core.Shared.ViewModels;
using Locations.Core.Shared.ViewModels.Interface;

namespace Location.Core.Views;

public partial class AddLocation : ContentPage
{
    LocationsService ls = new LocationsService();
    Locations.Core.Business.DataAccess.SettingsService ss = new Locations.Core.Business.DataAccess.SettingsService();
    private IAlertService alertServ;
    private ILoggerService loggerService;
    public AddLocation(ILoggerService loggerService, IAlertService alertServ): this()
    {

        this.alertServ = alertServ;
        this.loggerService = loggerService;

    }
    public AddLocation(IAlertService alertServ, ILoggerService loggerService, ILocationViewModel viewModel) : this(viewModel)
    {
        this.alertServ = alertServ;
        this.loggerService = loggerService;
    }
    public AddLocation(IAlertService alertServ, ILoggerService loggerService, ILocationViewModel viewModel, int id) : this(id)
    {
        this.alertServ = alertServ;
        this.loggerService = loggerService;
    }
    public AddLocation(IAlertService alertServ, ILoggerService loggerService, ILocationViewModel viewModel, int id, bool isEditMode) : this(id, isEditMode)
    {
        this.alertServ = alertServ;
        this.loggerService = loggerService;
    }
    public AddLocation()
    {
        InitializeComponent();
        CloseModal.IsVisible = CloseModal.IsEnabled = false;
       var x = (LocationViewModel)BindingContext;
        x.RaiseAlert += OnRaiseAlert;
    }

    private void OnRaiseAlert(object? sender, Locations.Core.Shared.Customizations.Alerts.Implementation.AlertEventArgs e)
    {
        ServiceBase<LocationViewModel>.RaiseError(new Exception(e.Message));
    }

    public AddLocation(ILocationViewModel viewModel) : this()
    {
        InitializeComponent();
        BindingContext = (LocationViewModel)viewModel;
       CloseModal.IsVisible = CloseModal.IsEnabled = false;
    }
    public AddLocation(int id) : this()
    {
        InitializeComponent();
        BindingContext = ls.Get(id);
        CloseModal.IsVisible = CloseModal.IsEnabled = true;
    }
    public AddLocation(int id, bool isEditMode): this(id)
    {
        InitializeComponent();
        CloseModal.IsVisible = isEditMode;
        BindingContext = ls.Get(id);

    }

    private void Save_Pressed(object sender, EventArgs e)
    {
        
        var x = (LocationViewModel)BindingContext;
        var y = ls.Save(x, true);
        BindingContext = new LocationViewModel();

    }
    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        PageHelpers.CheckVisit(MagicStrings.SunCalculatorViewed, PageEnums.SunCalculations, ss, Navigation);


    }
    private async void AddPhoto_Pressed(object sender, EventArgs e)
    {
        var x = (LocationViewModel)BindingContext;
        if (MediaPicker.Default.IsCaptureSupported)
        {
            FileResult photo = await MediaPicker.Default.CapturePhotoAsync();

            if (photo != null)
            {
                // save the file into local storage
                string localFilePath = Path.Combine(Microsoft.Maui.Storage.FileSystem.AppDataDirectory, photo.FileName);

                using Stream sourceStream = await photo.OpenReadAsync();
                using FileStream localFileStream = File.OpenWrite(localFilePath);
                await sourceStream.CopyToAsync(localFileStream);
                x.Photo = localFilePath;
                AddPhoto.ImageSource = localFilePath;
            }
        }
    }

    private void CloseModal_Pressed(object sender, EventArgs e)
    {
        Navigation.PopModalAsync();
    }


}