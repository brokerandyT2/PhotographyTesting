
using Location.Core.Helpers;
using Location.Photography.Business.LightMeter;
using Locations.Core.Shared;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using System;
using Location.Photography.Business.LightMeter;

namespace Location.Core.Views.Premium;

public partial class LightMeter : ContentPage
{
    public LunaProDrawable LunaDrawable { get; set; }
    private LunaProDrawable lightMeter;
    private Point initialTouchPoint;

    public LightMeter()
    {
        InitializeComponent();
        // Create the drawable for the GraphicsView
        lightMeter = new LunaProDrawable(LightMeterView);
        LightMeterView.Drawable = lightMeter;

        // Add tap and pan gesture recognizers
        var tapGesture = new TapGestureRecognizer();
       // tapGesture.Tapped += OnLightMeterTapped;

        var panGesture = new PanGestureRecognizer();
        panGesture.PanUpdated += OnLightMeterPanned;

       // LightMeterContainer.GestureRecognizers.Add(tapGesture);
        LightMeterContainer.GestureRecognizers.Add(panGesture);
    }

    private void OnLightMeterTapped(object sender, TappedEventArgs e)
    {
        // Get the position of the tap relative to the graphics view
        Point tapLocation = (Point)e.GetPosition(LightMeterView);
        if (tapLocation != null)
        {
            // Store as initial touch point
            initialTouchPoint = tapLocation;

            // Forward tap to the drawable with touch point info
            if (lightMeter is LunaProDrawable drawable)
            {
              //  drawable.HandleTapped(tapLocation);
                LightMeterView.Invalidate();
            }
            
        }
    }

    private void OnLightMeterPanned(object sender, PanUpdatedEventArgs e)
    {
        if (lightMeter is LunaProDrawable drawable)
        {
            // Forward pan event to the drawable
            //drawable.HandlePan(e, initialTouchPoint);
            LightMeterView.Invalidate();

            // If the pan has completed, reset the initial point
            if (e.StatusType == GestureStatus.Completed || e.StatusType == GestureStatus.Canceled)
            {
                initialTouchPoint = Point.Zero;
            }
        }
    }
    
    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        PageHelpers.CheckVisit(MagicStrings.LightMeterViewed, PageEnums.LightMeter, new Locations.Core.Business.DataAccess.SettingsService(), Navigation);
    }

    private void FullRadioButton_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {

    }

    private void SendToExpCalc_Pressed(object sender, EventArgs e)
    {

    }
}
