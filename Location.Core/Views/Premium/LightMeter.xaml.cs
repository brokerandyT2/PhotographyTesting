using vm = Location.Photography.Shared.ViewModels;
using SkiaSharp;
using Locations.Core.Business.DataAccess;
using Locations.Core.Shared.Customizations.Alerts.Interfraces;
using Locations.Core.Shared.Customizations.Logging.Interfaces;
using Location.Photography.Shared.ViewModels;
using Location.Photography.Business.DataAccess;

namespace Location.Core.Views.Premium
{
    public partial class LightMeter : ContentPage
    {
        private readonly vm.LightMeterViewModel _viewModel;
        private System.Timers.Timer _captureTimer;
        private bool _isCapturing = false;

        public LightMeter()
        {
            InitializeComponent();
            _viewModel = BindingContext as vm.LightMeterViewModel;
            LightMeterScaleDrawable.Instance.BindViewModel(_viewModel);



        }
        protected override void OnNavigatedTo(NavigatedToEventArgs args)
        {
            var settingsService = new Locations.Core.Business.DataAccess.SettingsService();
            base.OnNavigatedTo(args);
            int interval;
            try
            {
                var value = settingsService.GetSetting("CameraRefresh").Value;
                interval = Convert.ToInt32(value);
            }
            catch { interval = 2000; }
            _captureTimer = new System.Timers.Timer(interval);
            _captureTimer.Elapsed += async (s, e) => await CaptureAndProcessFrameAsync();
            _captureTimer.Start();
        }
        private async Task CaptureAndProcessFrameAsync()
        {
            if (_isCapturing || cameraView == null || !cameraView.IsEnabled) return;
            _isCapturing = true;
            try
            {
                var stream = cameraView.TakePhotoAsync().Result;
                if (stream != null)
                {
                    using var memoryStream = new MemoryStream();
                    await stream.CopyToAsync(memoryStream);
                    memoryStream.Seek(0, SeekOrigin.Begin);
                    var bitmap = SKBitmap.Decode(memoryStream);
                    _viewModel?.ProcessFrame(bitmap);
                    MainThread.BeginInvokeOnMainThread(() => graphicsView.Invalidate());
                }
            }
            catch (Exception) { }
            finally { _isCapturing = false; }
        }

        private void Button_Pressed(object sender, EventArgs e)
        {
            var x = cameraView.TakePhotoAsync().Result;
            var z = string.Empty;
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