using Location.Core.Helpers;

namespace Location.Core.Views;

public partial class SubscriptionOrAdFeature : ContentPage
{
    private PageEnums pageEnums;

    public SubscriptionOrAdFeature()
	{
		InitializeComponent();
	}

    public SubscriptionOrAdFeature(PageEnums pageEnums)
    {
        this.pageEnums = pageEnums;
    }
}