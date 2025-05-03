using Location.Core.Helpers;
using Location.Core.Resources;
using Location.Photography.Business.DataAccess;
using Locations.Core.Shared.Customizations.Alerts.Interfraces;
using Locations.Core.Shared.Customizations.Logging.Interfaces;
using Locations.Core.Shared;


namespace Location.Core.Views;

public partial class FeatureNotSupported : ContentPage
{
    SettingsService ss = new();
    private IAlertService alertServ;
    private ILoggerService loggerService;
    public FeatureNotSupported()
    {
        InitializeComponent();

    }
    public FeatureNotSupported(IAlertService alertServ, ILoggerService loggerService)
    {
        this.alertServ = alertServ;
        this.loggerService = loggerService;
        InitializeComponent();
    }
    public FeatureNotSupported(string page, IAlertService alertServ, ILoggerService loggerService) : this(page)
    {
        this.alertServ = alertServ;
        this.loggerService = loggerService;


    }
    public FeatureNotSupported(string page)
    {
        var subscription = ss.GetSettingByName(MagicStrings.SubscriptionType).Value;


        string toDisplay = string.Empty;
        switch (page)
        {
            case MagicStrings.ExposureCalculator:
                toDisplay = string.Format(AppResources.ExposurePaid, subscription, "Premium");
                break;
            default:
                toDisplay = "Page Not Found";
                break;
        }
        content.Text = toDisplay;
    }
}