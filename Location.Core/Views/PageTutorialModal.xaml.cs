using Location.Core.Helpers;
using Location.Core.Resources;
using Locations.Core.Shared.Customizations.Alerts.Interfraces;
using Locations.Core.Shared.Customizations.Logging.Interfaces;
namespace Location.Core.Views;

public partial class PageTutorialModal : ContentPage
{
    private IAlertService alertServ;
    private ILoggerService loggerService;
    public PageTutorialModal()
	{
		InitializeComponent();
	}
    public PageTutorialModal(ILoggerService loggerService, IAlertService alertServ)
    {
        this.alertServ = alertServ;
        this.loggerService = loggerService;
        InitializeComponent();
    }
    public PageTutorialModal(PageEnums page)
    {
        InitializeComponent();
        switch (page)
        {
            case PageEnums.WeatherDisplay:
                content.Text = AppResources.WeatherDisplayTutorial;
                break;
            case PageEnums.ExposureCalculator:
                content.Text = AppResources.ExposureCalculatorTutorial;
                break;
            case PageEnums.LightMeter:
                content.Text = AppResources.LightMeterTutorial;
                break;
            case PageEnums.SunLocation:
                content.Text = AppResources.SunLocationTutorial;
                break;
            case PageEnums.SceneEvaluation:
                content.Text = AppResources.SceneEvaluationTutorial;
                break;
            case PageEnums.SunCalculations:
                content.Text = AppResources.SunCalculationsTutorial;
                break;
            case PageEnums.AddLocation:
                content.Text = AppResources.AddLocationTutorial;
                break;
            case PageEnums.ListLocations:
                content.Text = AppResources.ListLocationsTutorial;
                break;
            case PageEnums.Settings:
                content.Text = AppResources.SettingsTutorial;
                break;
            case PageEnums.Tips:
                content.Text = AppResources.TipsTutorial;
                break;
            default:
                content.Text = "Page Not Found";
                break;
        }
    }
}