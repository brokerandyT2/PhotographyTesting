using Location.Core.Views.Premium;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using System;

namespace Location.Core.Views.Premium
{
    public partial class LightMeter : ContentPage
    {
        public LunaProDrawable LunaDrawable { get; set; }
        private int _activeDialIndex = -1;
        private Point _lastPanPoint;

        public LightMeter()
        {
            InitializeComponent();
            BindingContext = this;

            LunaDrawable = new LunaProDrawable();
            // Create instance of LunaProDrawable
            LunaMeterView.Drawable = LunaDrawable; // Set the drawable for the view
            LunaMeterView.Invalidate();           // Force the redraw

            // Set initial state for step size radio buttons
            FullRadioButton.IsChecked = true;
            HalvesRadioButton.IsChecked = ThirdsRadioButton.IsChecked = false;

            // Gesture handling for pan gestures
            var panGesture = new PanGestureRecognizer();
            panGesture.PanUpdated += OnPanUpdated;
            LunaMeterView.GestureRecognizers.Add(panGesture);

            LunaMeterView.Invalidate();  // Redraw the view after initializing
        }
        private void OnStepSizeChanged(object sender, CheckedChangedEventArgs e)
        {
            // Logic to handle step size change
            if (e.Value)
            {
                var radioButton = sender as Microsoft.Maui.Controls.RadioButton;
                if (radioButton != null)
                {
                    // Perform actions based on which radio button is checked
                    if (radioButton == FullRadioButton)
                    {
                        LunaDrawable._isoValues = Location.Photography.Shared.ExposureCalculator.ISOs.Full.ToList();
                        LunaDrawable._fStopValues = Location.Photography.Shared.ExposureCalculator.Apetures.Full.ToList();
                        LunaDrawable._shutterSpeedValues = Location.Photography.Shared.ExposureCalculator.ShutterSpeeds.Full.ToList();

                    }
                    else if (radioButton == HalvesRadioButton)
                    {
                        LunaDrawable._isoValues = Location.Photography.Shared.ExposureCalculator.ISOs.Halves.ToList();
                        LunaDrawable._fStopValues = Location.Photography.Shared.ExposureCalculator.Apetures.Halves.ToList();
                        LunaDrawable._shutterSpeedValues = Location.Photography.Shared.ExposureCalculator.ShutterSpeeds.Halves.ToList();
                    }
                    else if (radioButton == ThirdsRadioButton)
                    {
                        LunaDrawable._isoValues = Location.Photography.Shared.ExposureCalculator.ISOs.Thirds.ToList();
                        LunaDrawable._fStopValues = Location.Photography.Shared.ExposureCalculator.Apetures.Thirds.ToList();
                        LunaDrawable._shutterSpeedValues = Location.Photography.Shared.ExposureCalculator.ShutterSpeeds.Thirds.ToList();
                    }
                }
            }
        }
        private void OnPanUpdated(object sender, PanUpdatedEventArgs e)
        {
            switch (e.StatusType)
            {
                case GestureStatus.Started:
                    // Start gesture - determine the dial being touched
                    var startPoint = new Point(e.TotalX, e.TotalY);
                    _activeDialIndex = LunaDrawable.HitTestDial(startPoint);
                    _lastPanPoint = startPoint;
                    break;

                case GestureStatus.Running:
                    if (_activeDialIndex != -1)
                    {
                        var currentPoint = new Point(e.TotalX, e.TotalY);

                        // Calculate angle difference for dial rotation
                        double angleStart = Math.Atan2(_lastPanPoint.Y - LunaDrawable.CenterY, _lastPanPoint.X - LunaDrawable.CenterX);
                        double angleCurrent = Math.Atan2(currentPoint.Y - LunaDrawable.CenterY, currentPoint.X - LunaDrawable.CenterX);
                        double angleDelta = angleCurrent - angleStart;

                        LunaDrawable.RotateDial(_activeDialIndex, angleDelta);
                        _lastPanPoint = currentPoint;
                    }
                    break;

                case GestureStatus.Completed:
                case GestureStatus.Canceled:
                    _activeDialIndex = -1;
                    break;
            }
        }
    }
}
