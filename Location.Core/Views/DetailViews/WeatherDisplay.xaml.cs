namespace Location.Core.Views.DetailViews;

public partial class WeatherDisplay : ContentPage
{
    

    public WeatherDisplay()
    {
        InitializeComponent();
    }
    

    private void ImageButton_Pressed(object sender, EventArgs e)
    {
        Navigation.PopModalAsync();
    }
}