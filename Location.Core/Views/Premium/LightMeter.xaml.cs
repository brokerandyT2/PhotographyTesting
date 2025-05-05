using vm = Location.Photography.Shared.ViewModels;
using SkiaSharp;
using Locations.Core.Business.DataAccess;
using Locations.Core.Shared.Customizations.Alerts.Interfraces;
using Locations.Core.Shared.Customizations.Logging.Interfaces;
using Location.Photography.Shared.ViewModels;
using Location.Photography.Business.DataAccess;
using Location.Core.Resources;
using Android.Hardware;
using Location.Core.Platforms.Android.Interface;
using Microsoft.Maui;
using Java.Interop;

namespace Location.Core.Views.Premium
{
    public partial class LightMeter : ContentPage, ISensorEventListener
    {
        private readonly IAmbientLightSensorService _lightSensorService;
        private double _evValue;
        private double _needleRotation;
        private double _peakNeedleRotation;
        private bool disposedValue;

        public List<int> ISOs { get; set; }
        public List<double> Apertures { get; set; }
        public List<double> ShutterSpeeds { get; set; }

        public nint Handle => throw new NotImplementedException();

        public int JniIdentityHashCode => throw new NotImplementedException();

        public JniObjectReference PeerReference => throw new NotImplementedException();

        public JniPeerMembers JniPeerMembers => throw new NotImplementedException();

        public JniManagedPeerStates JniManagedPeerState => throw new NotImplementedException();

        public LightMeter()
        {
            InitializeComponent();

            // Initialize the light sensor service
            _lightSensorService = DependencyService.Get<IAmbientLightSensorService>();

            // Set up ISO, Aperture, and Shutter Speed options
            ISOs = new List<int> { 100, 200, 400, 800, 1600 };
            Apertures = new List<double> { 1.4, 2.0, 2.8, 4.0, 5.6, 8.0, 11.0, 16.0 };
            ShutterSpeeds = new List<double> { 1 / 1000.0, 1 / 500.0, 1 / 250.0, 1 / 125.0, 1 / 60.0, 1 / 30.0, 1 / 15.0, 1 / 8.0, 1 / 4.0, 1 / 2.0, 1 };

            // Bind the collections to the pickers
            isoPicker.ItemsSource = ISOs;
            aperturePicker.ItemsSource = Apertures;
            shutterPicker.ItemsSource = ShutterSpeeds;

            // Set default selections
            isoPicker.SelectedItem = ISOs[0];
            aperturePicker.SelectedItem = Apertures[2]; // f/2.8
            shutterPicker.SelectedItem = ShutterSpeeds[3]; // 1/125s

            // Initialize the light sensor
            _lightSensorService.LightLevelChanged += OnLightLevelChanged;
        }

        // Handle light sensor changes
        private void OnLightLevelChanged(object sender, float lux)
        {
            // Update EV and Needle Rotation based on the light level (lux)
            _evValue = CalculateExposureValue(lux);
            _needleRotation = MapEVToNeedleRotation(_evValue);

            // Update peak needle if necessary
            if (_needleRotation > _peakNeedleRotation)
            {
                _peakNeedleRotation = _needleRotation;
            }

            // Update the UI (needle rotation in graphics view)
            graphicsView.Invalidate(); // Trigger a redraw of the meter
        }

        // Button click event to start light sensor monitoring
        private void Button_Pressed_1(object sender, EventArgs e)
        {
            _lightSensorService.StartListening();
        }

        // Calculate Exposure Value (EV) based on lux, ISO, Aperture, and Shutter Speed
        private double CalculateExposureValue(float lux)
        {
            double isoFactor = (int)isoPicker.SelectedItem / 100.0; // ISO divided by 100
            double apertureFactor = (double)aperturePicker.SelectedItem * (double)aperturePicker.SelectedItem; // f-stop squared
            double shutterSpeedFactor = 1 / (double)shutterPicker.SelectedItem; // Shutter speed (in seconds)

            // EV = log2(lux / (ISO * Aperture * Shutter Speed))
            double ev = Math.Log2(lux / (isoFactor * apertureFactor * shutterSpeedFactor));
            return ev;
        }

        // Map EV to needle rotation (e.g., -5 EV to +20 EV mapped to 135–270 degrees)
        private double MapEVToNeedleRotation(double ev)
        {
            return 135 + (ev + 5) * (270 / 25); // Maps EV -5 to +20 onto 135–270 degrees
        }

        // Sensor Event Listener methods (not used here but needed by interface)
        public void OnAccuracyChanged(Sensor sensor, SensorStatus accuracy) { }
        public void OnSensorChanged(SensorEvent e)
        {
            if (e.Sensor.Type == SensorType.Light)
            {
                float lux = e.Values[0];
                OnLightLevelChanged(this, lux);
            }
        }

        public void SetJniIdentityHashCode(int value)
        {
            throw new NotImplementedException();
        }

        public void SetPeerReference(JniObjectReference reference)
        {
            throw new NotImplementedException();
        }

        public void SetJniManagedPeerState(JniManagedPeerStates value)
        {
            throw new NotImplementedException();
        }

        public void UnregisterFromRuntime()
        {
            throw new NotImplementedException();
        }

        public void DisposeUnlessReferenced()
        {
            throw new NotImplementedException();
        }

        public void Disposed()
        {
            throw new NotImplementedException();
        }

        public void Finalized()
        {
            throw new NotImplementedException();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~LightMeter()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }





    public class LightMeterScaleDrawable : IDrawable
    {
        private static readonly Lazy<LightMeterScaleDrawable> _instance = new(() => new LightMeterScaleDrawable());
        public static LightMeterScaleDrawable Instance => _instance.Value;

        private vm.LightMeterViewModel? _viewModel;
        private float _animatedRotation = 135f;

        public void BindViewModel(LightMeterViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            if (_viewModel == null) return;

            var centerX = dirtyRect.Width / 2;
            var centerY = dirtyRect.Height / 2;
            var radius = Math.Min(centerX, centerY) - 20;

            // Retro cream dial background
            canvas.FillColor = Color.FromArgb("#F5F5DC"); // beige/cream
            canvas.FillCircle(centerX, centerY, radius);

            // Outer circle stroke
            canvas.StrokeColor = Colors.DarkGray;
            canvas.StrokeSize = 4;
            canvas.DrawCircle(centerX, centerY, radius);

            // Inner ring for retro styling
            canvas.StrokeColor = Colors.LightGray;
            canvas.StrokeSize = 2;
            canvas.DrawCircle(centerX, centerY, radius - 20);

            // Tick marks and EV labels
            for (int ev = -5; ev <= 5; ev++)
            {
                float angle = 135 + (ev + 5) * (270f / 10f);
                float rad = angle * (float)Math.PI / 180f;

                float tickStart = radius - 10;
                float tickEnd = radius;
                float x1 = centerX + (float)Math.Cos(rad) * tickStart;
                float y1 = centerY + (float)Math.Sin(rad) * tickStart;
                float x2 = centerX + (float)Math.Cos(rad) * tickEnd;
                float y2 = centerY + (float)Math.Sin(rad) * tickEnd;

                canvas.StrokeColor = ev == 0 ? Colors.OrangeRed : Colors.Black;
                canvas.StrokeSize = 2;
                canvas.DrawLine(x1, y1, x2, y2);

                var labelX = centerX + (float)Math.Cos(rad) * (radius - 25);
                var labelY = centerY + (float)Math.Sin(rad) * (radius - 25);
                canvas.FontColor = Colors.Black;
                canvas.FontSize = ev == 0 ? 18 : 14;
                canvas.DrawString(ev.ToString(), labelX, labelY, HorizontalAlignment.Center);
            }

            // Smooth needle animation with subtle jitter
            float baseTargetAngle = 135 + (float)(_viewModel.EVValue + 5) * (270f / 10f);

            // Subtle random jitter (±0.5°)
            float jitter = (float)(Random.Shared.NextDouble() - 0.5) * 1f;

            // Slowly blend toward target + jitter
            float jitteredTarget = baseTargetAngle + jitter;
            _animatedRotation += (jitteredTarget - _animatedRotation) * 0.05f;

            var needleRad = _animatedRotation * (float)Math.PI / 180f;
            float nx = centerX + (float)Math.Cos(needleRad) * (radius - 30);
            float ny = centerY + (float)Math.Sin(needleRad) * (radius - 30);

            // Needle
            canvas.StrokeColor = Colors.Red;
            canvas.StrokeSize = 4;
            canvas.DrawLine(centerX, centerY, nx, ny);

            // Needle base hub
            canvas.FillColor = Colors.Black;
            canvas.FillCircle(centerX, centerY, 6);
        }
    }
}