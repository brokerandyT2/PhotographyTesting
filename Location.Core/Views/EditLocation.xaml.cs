using Location.Core.Resources;
using Locations.Core.Shared.ViewModels;

namespace Location.Core.Views;

public partial class EditLocation : ContentPage
{
    private int id;
   

    public EditLocation()
	{
		InitializeComponent();
	}
    public EditLocation(int id)
    {
       
    }
    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
       
    }

    private void WeatherButton_Pressed(object sender, EventArgs e)
    {
        var y = (LocationViewModel)BindingContext;
    }

    private void SunEvents_Pressed(object sender, EventArgs e)
    {
        var y = (LocationViewModel)BindingContext;
    }

    private void ImageButton_Pressed(object sender, EventArgs e)
    {
        Navigation.PushAsync(new MainPage());
    }
}