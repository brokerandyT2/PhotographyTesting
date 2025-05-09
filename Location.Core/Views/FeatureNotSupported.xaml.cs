using Location.Core.Resources;
using Locations.Core.Shared;


namespace Location.Core.Views;

public partial class FeatureNotSupported : ContentPage
{


    public FeatureNotSupported()
    {
        InitializeComponent();

    }
   
    public FeatureNotSupported(string page)
    {
        var subscription = string.Empty;// ss.GetSettingByName(MagicStrings.SubscriptionType).Value;


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