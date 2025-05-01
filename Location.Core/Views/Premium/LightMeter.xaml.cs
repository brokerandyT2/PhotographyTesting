using vm = Location.Photography.Shared.ViewModels;
using SkiaSharp;
using SkiaSharp.Views.Maui;
using Locations.Core.Business.DataAccess;
using System.Timers;
using Camera.MAUI;
using Location.Photography.Business.DataAccess;
using Locations.Core.Shared;

namespace Location.Core.Views.Premium;

public partial class LightMeter : ContentPage
{
    private readonly vm.LightMeter _viewModel;
    private System.Timers.Timer _captureTimer;
    private bool _isCapturing = false;
    Locations.Core.Business.DataAccess.SettingsService _settingsService = new Locations.Core.Business.DataAccess.SettingsService();
    public LightMeter()
    {
        InitializeComponent();
        _viewModel = BindingContext as vm.LightMeter;
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        // Start camera timer loop 
       var timer = _settingsService.GetSetting(MagicStrings.CameraRefresh).Value;
        _captureTimer = new System.Timers.Timer(Convert.ToInt16(timer)); // 1000 ms = 1 second
        _captureTimer.Elapsed += async (s, e) => await CaptureAndProcessFrameAsync();
        _captureTimer.Start();
    }

    protected override void OnNavigatedFrom(NavigatedFromEventArgs args)
    {
        base.OnNavigatedFrom(args);
        _captureTimer?.Stop();
        _captureTimer?.Dispose();
    }

    private async Task CaptureAndProcessFrameAsync()
    {
        if (_isCapturing || cameraView == null || !cameraView.IsEnabled)
            return;

        _isCapturing = true;

        try
        {
            var stream = await cameraView.TakePhotoAsync();
            if (stream != null)
            {
                using var memoryStream = new MemoryStream();
                await stream.CopyToAsync(memoryStream);
                memoryStream.Seek(0, SeekOrigin.Begin);

                var bitmap = SKBitmap.Decode(memoryStream);
                if (bitmap != null)
                {
                    _viewModel?.ProcessFrame(bitmap);

                    // Request redraw
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        canvasView.InvalidateSurface();
                    });
                }
            }
        }
        catch (Exception ex)
        {
            // Optional: Log error
        }
        finally
        {
            _isCapturing = false;
        }
    }

    private void Button_Pressed(object sender, EventArgs e)
    {
        // You could optionally trigger a single snapshot manually here
        _ = CaptureAndProcessFrameAsync();
    }

    private void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs e)
    {
        var canvas = e.Surface.Canvas;
        var info = e.Info;
        canvas.Clear();

        float centerX = info.Width / 2f;
        float centerY = info.Height / 2f;
        float radius = Math.Min(centerX, centerY) * 0.8f;

        // Draw background
        using var bgPaint = new SKPaint
        {
            Style = SKPaintStyle.Stroke,
            Color = SKColors.Gray,
            StrokeWidth = 5
        };
        canvas.DrawCircle(centerX, centerY, radius, bgPaint);

        // Draw needle
        using var needlePaint = new SKPaint
        {
            Style = SKPaintStyle.Stroke,
            Color = SKColors.Red,
            StrokeWidth = 4,
            IsAntialias = true
        };

        float angle = (float)(_viewModel?.NeedleRotation ?? 0);
        var radMatrix = SKMatrix.CreateRotationDegrees(angle, centerX, centerY);

        SKPoint start = new(centerX, centerY);
        SKPoint end = new(centerX, centerY - radius);
        end = radMatrix.MapPoint(end);
        canvas.DrawLine(start, end, needlePaint);

        // Draw EV text
        using var textPaint = new SKPaint
        {
            Color = SKColors.White,
            TextSize = 48,
            IsAntialias = true,
            TextAlign = SKTextAlign.Center
        };
        canvas.DrawText($"EV: {Math.Round(_viewModel?.EVValue ?? 0, 1)}", centerX, centerY + radius + 50, textPaint);
    }
}
