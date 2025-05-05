using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Location.Photography.Shared.ExposureCalculator;
using System;
using System.Linq;
using Microsoft.Maui;
using Syncfusion.Maui.Toolkit.Graphics.Internals;
using System.Threading.Tasks;
using Location.Core.Platforms.Android.Interface;
using Location.Core.Resources;
using Location.Core.Platforms.Android.Implementation;
using Location.Photography.Business.DataAccess;
using Xamarin.Google.Crypto.Tink.Subtle;
using Locations.Core.Shared;
using Android.Telephony;
using static Locations.Core.Shared.Enums.SubscriptionType;

namespace Location.Core.Views.Premium
{
    public partial class LightMeter : ContentPage, IVolumeKeyHandler
    {
        private const double NeedleAnimationDuration = 0.5; // Duration of the needle animation in seconds
        private float apertureValue;
        private float shutterSpeedValue;
        private float isoValue;
        private string selectedStepSize = "Full"; // Default step size
        private bool isNeedleAtPeak = false; // Track whether the needle is at the peak

        private IAmbientLightSensorService _lightSensorService;
        private float _peakLux;

        public LightMeter()
        {
            InitializeComponent();
            this.BindingContext = this;
            graphicsView.Drawable = new LightMeterScaleDrawable(this);

            stepPicker.SelectedIndexChanged += StepPicker_SelectedIndexChanged;

            // Initialize the light sensor service
            _lightSensorService = DependencyService.Get<IAmbientLightSensorService>();
            _lightSensorService.LightLevelChanged += OnLightLevelChanged;


            // Set initial values for the exposure dials
            apertureValue = 5.6f;
            shutterSpeedValue = 1 / 60f;
            isoValue = 100f;

            // Initialize the dials (this can be updated based on saved values)
            UpdateNeedlePosition();

            // Gesture recognizers for dragging the dials
            AddGestureRecognizers();
            SettingsService ss = new SettingsService();

            SubscriptionTypeEnum _subType;
            Enum.TryParse(ss.GetSettingByName(MagicStrings.SubscriptionType).Value, out _subType);
             expButton.IsVisible = _subType == SubscriptionTypeEnum.Premium ? true : false;

        }
        protected override void OnNavigatedTo(NavigatedToEventArgs args)
        {
            base.OnNavigatedTo(args);
            _lightSensorService.StartListening();
            DisplayAlert(AppResources.Information, AppResources.VolumeUpDown, AppResources.OK);
        }
        public string[] StepOptions => new[] { "Full", "Half", "Thirds" };

        private void StepPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (stepPicker.SelectedIndex != -1)
            {
                selectedStepSize = stepPicker.ItemsSource[stepPicker.SelectedIndex].ToString();
                UpdateNeedlePosition();
            }
        }

        // Add gesture recognizers for dragging
        private void AddGestureRecognizers()
        {
            var tapGesture = new TapGestureRecognizer();
            tapGesture.Tapped += (s, e) =>
            {
                // Logic for handling tapping gestures on the LightMeter scales if necessary
            };

            graphicsView.GestureRecognizers.Add(tapGesture);

            // Example for dragging interaction (simplified for demonstration)
            var panGesture = new PanGestureRecognizer();
            panGesture.PanUpdated += (sender, args) =>
            {
                if (args.StatusType == GestureStatus.Running)
                {
                    var deltaX = args.TotalX;

                    // Update the value based on the drag direction (this is just an example, you can tweak it)
                    if (Math.Abs(deltaX) > 0.5) // Threshold for significant drag
                    {
                        var valueChange = Math.Sign(deltaX); // Drag direction
                        UpdateDialValues(valueChange);
                    }
                }
            };

            graphicsView.GestureRecognizers.Add(panGesture);
        }

        // Update the dial values based on the drag direction
        private void UpdateDialValues(float valueChange)
        {
            var apertureValues = Apetures.GetScale(selectedStepSize);
            var shutterSpeedValues = ShutterSpeeds.GetScale(selectedStepSize);
            var isoValues = ISOs.GetScale(selectedStepSize);

            // Simulate value change for aperture, shutter speed, or ISO based on the direction of the drag
            apertureValue = SnapToClosestValue(apertureValues, apertureValue + valueChange);
            shutterSpeedValue = SnapToClosestValue(shutterSpeedValues, shutterSpeedValue + valueChange);
            isoValue = SnapToClosestValue(isoValues, isoValue + valueChange);

            // Update the display with the new dial values
            UpdateNeedlePosition();
        }

        // Snap a value to the closest value from the provided scale
        private float SnapToClosestValue(string[] scaleValues, float value)
        {
            var parsedValues = scaleValues.Select(v => ParseValue(v)).ToList();
            var closestValue = parsedValues.OrderBy(v => Math.Abs(v - value)).First();
            return closestValue;
        }

        private float ParseValue(string value)
        {
            // Parse different formats for aperture (f/1.8), shutter speed (1/60), and ISO
            if (value.StartsWith("f/"))
            {
                return float.Parse(value.Substring(2)); // Remove "f/" and parse
            }
            else if (value.Contains("/"))
            {
                // This is a shutter speed, e.g., "1/60"
                var parts = value.Split('/');
                return 1 / float.Parse(parts[1]);
            }
            else
            {
                // Assume it's ISO
                return float.Parse(value);
            }
        }

        // Method to handle dynamic dragging for ISO, Aperture, and Shutter Speed dials
        private void UpdateNeedlePosition()
        {
            // Get the dial values based on the selected step size
            var apertureValues = Apetures.GetScale(selectedStepSize);
            var shutterSpeedValues = ShutterSpeeds.GetScale(selectedStepSize);
            var isoValues = ISOs.GetScale(selectedStepSize);

            // Convert the values to numbers
            var apertureIndex = Array.IndexOf(apertureValues, apertureValue.ToString());
            var shutterSpeedIndex = Array.IndexOf(shutterSpeedValues, shutterSpeedValue.ToString());
            var isoIndex = Array.IndexOf(isoValues, isoValue.ToString());

            // Update the drawable with the calculated positions
            if (graphicsView.Drawable is LightMeterScaleDrawable drawable)
            {
                // Corrected SetNeedlePosition with only one argument (index or angle)
                drawable.SetNeedlePosition((float)apertureIndex); // Assuming angle here, convert as necessary

                // Add a needle animation (twitch effect when reaching peak)
                AnimateNeedle(drawable);

                // Invalidate only the graphics view
                graphicsView.Invalidate(); // Redraw the light meter
            }
        }

        private void AnimateNeedle(LightMeterScaleDrawable drawable)
        {
            // Trigger a twitch effect when the needle reaches the peak (just a simple example)
            if (isNeedleAtPeak)
            {
                // Perform a "twitch" by briefly changing the needle's position and then reverting it
                drawable.SetNeedlePosition(drawable.NeedlePosition + 2); // Move needle slightly
                graphicsView.Invalidate();

                // Revert the needle after the twitch duration
                Task.Delay(100).ContinueWith(_ =>
                {
                    drawable.SetNeedlePosition(drawable.NeedlePosition - 2); // Move needle back to peak
                    graphicsView.Invalidate();
                });
            }

            // Set the needle position with animation
            drawable.AnimateNeedleTo(graphicsView, drawable.NeedlePosition, NeedleAnimationDuration);
        }

        private string[] GetScaleValues(dynamic scale)
        {
            return selectedStepSize switch
            {
                "Full" => scale.Full,
                "Half" => scale.Halves,
                "Thirds" => scale.Thirds,
                _ => scale.Full
            };
        }

        private void OnLightLevelChanged(object sender, float lux)
        {
            // Calculate EV based on ISO, Aperture, and Shutter Speed
            var ev = ExposureValueCalculator.CalculateEV(lux, isoValue, apertureValue, shutterSpeedValue);

            // Update the peak needle
            if (lux > _peakLux)
            {
                _peakLux = lux;
                isNeedleAtPeak = true;
            }
            else
            {
                isNeedleAtPeak = false;
            }

            // Update the needle positions
            UpdateNeedlePosition();
        }

        public void OnVolumeKeyPressed()
        {
            if (((AmbientLightSensorService)_lightSensorService).IsRunning)
            {
                _lightSensorService.StopListening();
                DisplayAlert(AppResources.Information, AppResources.MeasuringStopped, AppResources.OK);
            }
            else
            {
                _lightSensorService.StartListening();
            }
        }


        public enum ExposureStep
        {
            Full,
            Half,
            Third
        }

        private void Button_Pressed(object sender, EventArgs e)
        {
            var steps = stepPicker.SelectedItem.ToString();
            var iso = isoValue;
            var shutter = shutterSpeedValue;
            var fstop = apertureValue;
            Navigation.PushAsync(new ExposureCalculator(steps, iso, shutter, fstop));

        }
    }

    public static class ExposureValueCalculator
    {
        // Placeholder for actual EV calculation logic
        public static double CalculateEV(float lux, float iso, float aperture, float shutterSpeed)
        {
            // Implement EV calculation here
            return lux * iso * aperture / shutterSpeed; // Example calculation
        }
    }

    public class LightMeterScaleDrawable : IDrawable
    {
        private float _needleAngle;
        private bool _isAnimatingNeedle = false; // Flag to track if the needle is animating
        private float _currentNeedleAngle; // The current angle during animation
        private float _targetNeedleAngle; // The target angle to animate towards

        private readonly LightMeter _lightMeter;

        public LightMeterScaleDrawable(LightMeter lightMeter)
        {
            _lightMeter = lightMeter;
        }

        public float NeedlePosition
        {
            get => _needleAngle;
            set
            {
                _needleAngle = value;
                _currentNeedleAngle = value; // Ensure current angle reflects change
                _isAnimatingNeedle = false; // Stop animation if set manually
            }
        }

        public void SetNeedlePosition(float angle)
        {
            NeedlePosition = angle;
        }

        public void AnimateNeedleTo(IAnimatable animatable, float targetAngle, double duration)
        {
            // Start animation towards the target needle angle
            _targetNeedleAngle = targetAngle;
            _isAnimatingNeedle = true;

            // Start a simple animation loop to update the needle's position
            var animation = new Animation(v =>
            {
                _currentNeedleAngle = (float)v;
                (animatable as GraphicsView)?.Invalidate(); // Redraw the drawable while animating
            }, _currentNeedleAngle, _targetNeedleAngle);

            animation.Commit(animatable, "NeedleAnimation", 16, Convert.ToUInt32(duration * 1000), Easing.Linear);
        }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            // Drawing code for your light meter scale
            canvas.DrawLine(0, 0, 100, 100); // Example drawing (replace with actual scale drawing code)
        }
    }
}
