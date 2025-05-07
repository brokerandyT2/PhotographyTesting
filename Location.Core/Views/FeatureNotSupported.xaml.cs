using Location.Core.Resources;
using Location.Photography.Business.DataAccess;
using Locations.Core.Shared;
using Locations.Core.Shared.Customizations.Alerts.Interfraces;


namespace Location.Core.Views;

public partial class FeatureNotSupported : ContentPage
{
    SettingsService ss = new();
    private IAlertService alertServ;

    public FeatureNotSupported()
    {
        InitializeComponent();

    }
    public FeatureNotSupported(IAlertService alertServ)
    {
        this.alertServ = alertServ;

        InitializeComponent();
    }
    public FeatureNotSupported(string page, IAlertService alertServ) : this(page)
    {
        this.alertServ = alertServ;



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