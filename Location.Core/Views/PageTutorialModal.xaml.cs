using Location.Core.Helpers;
namespace Location.Core.Views;

public partial class PageTutorialModal : ContentPage
{
  

    public PageTutorialModal()
	{
		InitializeComponent();
	}
   
    public PageTutorialModal(PageEnums page)
    {
        InitializeComponent();
        var file = string.Empty;
        switch (page)
        {
            case PageEnums.WeatherDisplay:
                file = "WeatherDisplay.html";
                break;
            case PageEnums.ExposureCalculator:
                file = "ExposureCalculator.html";
                break;
            case PageEnums.LightMeter:
                file = "LightMeter.html";
                break;
            case PageEnums.SunLocation:
                file = "SunLocation.html";
                break;
            case PageEnums.SceneEvaluation:
                file = "SceneEvaluation.html";
                break;
            case PageEnums.SunCalculations:
                file = "SunCalculations.html";
                break;
            case PageEnums.AddLocation:
                file = "AddLocation.html";
                break;
            case PageEnums.ListLocations:
                file = "ListLocations.html";
                break;
            case PageEnums.Settings:
                file = "Settings.html";
                break;
            case PageEnums.Tips:
                file = "Tips.html";
                break;
            default:
                content.Text = "Page Not Found";
                break;
        }
        using var stream = FileSystem.OpenAppPackageFileAsync(file).Result;
        using var reader = new StreamReader(stream);
        var html = reader.ReadToEndAsync().Result;
        tutorial.Source = new HtmlWebViewSource { Html = html };
    }
}