using Locations.Core.Business.DataAccess;
using Locations.Core.Shared;
using Locations.Core.Shared.ViewModels;
using Locations.Core.Shared.ViewModels.Interface;

namespace Location.Core.Views;

public partial class AddLocation : ContentPage
{
    LocationsService ls = new LocationsService();
    SettingsService ss = new SettingsService();
    public AddLocation()
    {
        InitializeComponent();
        CloseModal.IsVisible = CloseModal.IsEnabled = false;
    }
    public AddLocation(ILocationViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = (LocationViewModel)viewModel;
        CloseModal.IsVisible = CloseModal.IsEnabled = false;
    }
    public AddLocation(int id)
    {
        InitializeComponent();
        BindingContext = ls.Get(id);
        CloseModal.IsVisible = CloseModal.IsEnabled = true;
    }
    public AddLocation(int id, bool isEditMode)
    {
        InitializeComponent();
        CloseModal.IsVisible = isEditMode;
        BindingContext = ls.Get(id);

    }

    private void Save_Pressed(object sender, EventArgs e)
    {
        BindingContext = ls.Save((LocationViewModel)BindingContext, true);

    }
    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        var x = ss.GetSettingByName(MagicStrings.AddLocationViewed);
        if (x.ToBoolean() == false)
        {
            Navigation.PushModalAsync(new Views.DetailViews.HoldingPage(0));
            x.Value = MagicStrings.True_string;
            ss.UpdateSetting(x);
        }

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
            }
        }
    }

    private void CloseModal_Pressed(object sender, EventArgs e)
    {
        Navigation.PopModalAsync();
    }


}