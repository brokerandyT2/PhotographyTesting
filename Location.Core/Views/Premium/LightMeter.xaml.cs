using Location.Core.Helpers;
using Location.Photography.Business.LightMeter;
using Locations.Core.Shared;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.PlatformConfiguration.AndroidSpecific;
using Microsoft.Maui.Graphics;
using System;
using TabbedPage = Microsoft.Maui.Controls.TabbedPage;

namespace Location.Core.Views.Premium;

public partial class LightMeter : ContentPage
{
    public LunaProDrawable LunaDrawable { get; set; }
    private LunaProDrawable lightMeter;
    private TabbedPage parentTabbedPage;
    private bool inDialInteraction = false;

    public LightMeter()
    {
        InitializeComponent();

        // Find parent TabbedPage when page loads
        this.Loaded += OnLoaded;
    }

    private void OnLoaded(object sender, EventArgs e)
    {
        // Find the parent TabbedPage
        var page = this.Parent;
        while (page != null && !(page is TabbedPage))
        {
            page = page.Parent as Page;
        }
        parentTabbedPage = page as TabbedPage;

        // Create the drawable for the GraphicsView
        lightMeter = new LunaProDrawable(LightMeterView);
        lightMeter.InteractionStarted += OnDialInteractionStarted;
        lightMeter.InteractionEnded += OnDialInteractionEnded;
        LightMeterView.Drawable = lightMeter;
    }

    private void OnDialInteractionStarted(object sender, EventArgs e)
    {
        inDialInteraction = true;
        DisableTabbedPageSwiping();
    }

    private void OnDialInteractionEnded(object sender, EventArgs e)
    {
        inDialInteraction = false;
        EnableTabbedPageSwiping();
    }

    private void DisableTabbedPageSwiping()
    {
        if (parentTabbedPage != null)
        {
            // Use the platform-specific API to disable swiping
            parentTabbedPage.On<Microsoft.Maui.Controls.PlatformConfiguration.Android>()
                .SetIsSwipePagingEnabled(false);
        }
    }

    private void EnableTabbedPageSwiping()
    {
        if (parentTabbedPage != null)
        {
            // Use the platform-specific API to re-enable swiping
            parentTabbedPage.On<Microsoft.Maui.Controls.PlatformConfiguration.Android>()
                .SetIsSwipePagingEnabled(true);
        }
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        PageHelpers.CheckVisit(MagicStrings.LightMeterViewed, PageEnums.LightMeter, new Locations.Core.Business.DataAccess.SettingsService(), Navigation);
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();

        // Make sure swiping is re-enabled when page disappears
        EnableTabbedPageSwiping();

        // Clean up event handlers
        if (lightMeter != null)
        {
            lightMeter.InteractionStarted -= OnDialInteractionStarted;
            lightMeter.InteractionEnded -= OnDialInteractionEnded;
        }

        this.Loaded -= OnLoaded;
    }

    private void FullRadioButton_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        // Implement step size changes here
    }

    private void SendToExpCalc_Pressed(object sender, EventArgs e)
    {
        // Implement send to exposure calculator functionality here
    }
}