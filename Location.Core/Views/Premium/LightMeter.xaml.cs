using vm = Location.Photography.Shared.ViewModels;
using ZXing.Net.Maui;
using SkiaSharp;
using Locations.Core.Business.DataAccess;
using SkiaSharp.Views.Maui;
#if ANDROID
using Java.Nio;
#endif
namespace Location.Core.Views.Premium;

public partial class LightMeter : ContentPage
{
    private readonly vm.LightMeter _viewModel;

    private static SettingsService sss = new SettingsService();
    int delay = Convert.ToInt32(sss.GetSettingByName(Locations.Core.Shared.MagicStrings.CameraRefresh).Value);
    public LightMeter()
    {
        InitializeComponent();

        _viewModel = BindingContext as vm.LightMeter;
    }

    private void OnFrameReady(object sender, CameraFrameBufferEventArgs e)
    {
       

        


    }
    protected override void OnNavigatedFrom(NavigatedFromEventArgs args)
    {
        base.OnNavigatedFrom(args);
        _isActive = false;
        cameraView.BarcodesDetected -= cameraView_BarcodesDetected;

    }
    private bool _isActive = false;
    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        Thread.Sleep(delay);
        Thread.Sleep(delay);
        _isActive = true;
        SettingsService sss = new SettingsService();

        Thread.Sleep(delay);
       
    }
    private void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs e)
    {
        var canvas = e.Surface.Canvas;
        var info = e.Info;
        canvas.Clear();

        float centerX = info.Width / 2f;
        float centerY = info.Height / 2f;
        float radius = Math.Min(centerX, centerY) * 0.8f;

        // Draw meter background
        using (var paint = new SKPaint
        {
            Style = SKPaintStyle.Stroke,
            Color = SKColors.Gray,
            StrokeWidth = 5
        })
        {
            canvas.DrawCircle(centerX, centerY, radius, paint);
        }

        // Draw needle
        using (var paint = new SKPaint
        {
            Style = SKPaintStyle.Stroke,
            Color = SKColors.Red,
            StrokeWidth = 4,
            IsAntialias = true
        })
        {
            var angle = (float)(_viewModel?.NeedleRotation ?? 0);
            var rad = SKMatrix.CreateRotationDegrees(angle, centerX, centerY);

            SKPoint start = new(centerX, centerY);
            SKPoint end = new(centerX, centerY - radius);

            end = rad.MapPoint(end);

            canvas.DrawLine(start, end, paint);
        }

        // Draw EV Value text
        using (var paint = new SKPaint
        {
            Color = SKColors.White,
            TextSize = 48,
            IsAntialias = true,
            TextAlign = SKTextAlign.Center
        })
        {
            canvas.DrawText($"EV: {Math.Round((decimal)_viewModel?.EVValue, 1).ToString()}", centerX, centerY + radius + 50, paint);
        }
    }
    private void Button_Pressed(object sender, EventArgs e)
    {
        cameraView.BarcodesDetected += cameraView_BarcodesDetected;
        cameraView.CameraLocation = CameraLocation.Rear;
       
    }

    private void cameraView_BarcodesDetected(object sender, BarcodeDetectionEventArgs e)
    {
#if ANDROID
        var z = e.Results[0].Raw;

        byte[] outputBytes = new byte[16]; 
        var outputBuffer = ByteBuffer.Wrap(outputBytes);             //... filling buffer (using tflite model)
                                                                     //var bufferBytes = outputBuffer.GetFloat(2); 
        //var x = ((ZXing.Net.Maui.Controls.CameraView)sender).CaptureAsync().Result;// SaveAsImage(Guid.NewGuid().ToString());
        var ss = SKBitmap.Decode(z);
       // var z = string.Empty;
        _viewModel.ProcessFrame(ss);
#endif
    }

    private void cameraView_FrameReady(object sender, CameraFrameBufferEventArgs e)
    {
        
    }
}

public class LightMeterScaleDrawable : IDrawable
{
    public double NeedleRotation { get; set; }
    public double PeakNeedleRotation { get; set; }

    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        canvas.StrokeColor = Colors.Black;
        canvas.StrokeSize = 2;

        float centerX = dirtyRect.Center.X;
        float centerY = dirtyRect.Center.Y;
        float radius = Math.Min(dirtyRect.Width, dirtyRect.Height) / 2 - 20;

        // Draw Arc
        canvas.DrawArc(
            centerX - radius,
            centerY - radius,
            radius * 2,
            radius * 2,
            135, // Start angle
            270, // Sweep angle
            false, false);

        // Draw Tick Marks
        int divisions = 25; // from -5 EV to +20 EV
        for (int i = 0; i <= divisions; i++)
        {
            double angle = 135 + (270.0 / divisions) * i;
            double rad = Math.PI * angle / 180.0;
            float innerRadius = radius - 10;
            float outerRadius = radius;

            float x1 = centerX + (float)(innerRadius * Math.Cos(rad));
            float y1 = centerY + (float)(innerRadius * Math.Sin(rad));
            float x2 = centerX + (float)(outerRadius * Math.Cos(rad));
            float y2 = centerY + (float)(outerRadius * Math.Sin(rad));

            canvas.DrawLine(x1, y1, x2, y2);

            // Every 5 EVs: Draw label
            if (i % 5 == 0)
            {
                string label = (-5 + i).ToString();
                float labelRadius = radius - 25;
                float lx = centerX + (float)(labelRadius * Math.Cos(rad));
                float ly = centerY + (float)(labelRadius * Math.Sin(rad));
                canvas.FontColor = Colors.Black;
                canvas.FontSize = 12;
                canvas.DrawString(label, lx - 8, ly - 8, 20, 20, HorizontalAlignment.Center, VerticalAlignment.Center);
            }
        }

        // Draw Needle (Current)
        DrawNeedle(canvas, centerX, centerY, radius - 30, NeedleRotation, Colors.Red, 3);

        // Draw Peak Needle (optional, thinner)
        DrawNeedle(canvas, centerX, centerY, radius - 35, PeakNeedleRotation, Colors.Orange, 1);
    }

    private void DrawNeedle(ICanvas canvas, float centerX, float centerY, float length, double rotation, Color color, float thickness)
    {
        double angle = 135 + (rotation + 90); // Adjust because our scale starts at 135°
        double rad = Math.PI * angle / 180.0;

        float x = centerX + (float)(length * Math.Cos(rad));
        float y = centerY + (float)(length * Math.Sin(rad));

        canvas.StrokeColor = color;
        canvas.StrokeSize = thickness;
        canvas.DrawLine(centerX, centerY, x, y);
    }
    

   
}