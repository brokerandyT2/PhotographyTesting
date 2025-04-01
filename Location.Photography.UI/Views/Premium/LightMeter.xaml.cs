
using ZXing.Net.Maui.Readers;
using Microsoft.Maui.Graphics.Platform;
using SkiaSharp;
using Camera.MAUI;
using Location.Photography.Business.DataAccess;
using Locations.Core.Shared;
using Location.Photography.Shared.ViewModels;
namespace Location.Photography.UI.Views.Premium;

public partial class LightMeter : ContentPage
{
    SettingsService ss = new SettingsService();
    private double iso;
    private double shutterSpeed;  // User-specified shutter speed (seconds)
    private double aperture; // User-specified f-stop
    private bool _activate = true;
    private double _brightness;
    private double _ev;
    private string _exposure;
    public LightMeter()
    {
        InitializeComponent();
    }
    protected override void OnNavigatedFrom(NavigatedFromEventArgs args)
    {
        _activate = false;
        base.OnNavigatedFrom(args);
        var y = cameraView.StopCameraAsync().Result;
    }
    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        _activate = true;
        base.OnNavigatedTo(args);
        foreach (var camera in new CameraView().Cameras)
        {
            if (camera.Position == CameraPosition.Back)
            {
                cameraView.Camera = camera;
            }
        }
        var x = cameraView.StartCameraAsync().Result;
        cameraView.TakeAutoSnapShot = true;
        while (_activate == true)
        {
            var path = Path.Combine(FileSystem.CacheDirectory, Guid.NewGuid().ToString() + ".png");
            var y = cameraView.SaveSnapShot(Camera.MAUI.ImageFormat.PNG, path).Result;

            SkiaSharp.SKBitmap bmp = SKBitmap.Decode(path);

            var exist = File.Exists(path);

            if (y != null && exist)
            {
                _brightness = CalculateBrightness(bmp.Bytes, bmp.Height, bmp.Width);
                _ev = ConvertToEV(_brightness);
                _exposure = CompareExposure(_ev, iso, shutterSpeed, aperture);
            }


            try
            {
                File.Delete(path);
            }
            catch { }

            Thread.Sleep(Convert.ToInt32(ss.GetSettingByName(MagicStrings.CameraRefresh).Value));
        }
        ;
    }



    // 📌 Step 3: Calculate Brightness from Camera Frame
    private double CalculateBrightness(byte[] imageData, int width, int height)
    {
        // Average Luminance from YUV Image Data
        double sum = imageData.Take(width * height).Sum(b => (double)b);
        return sum / (width * height); // Normalize
    }

    // 📌 Step 4: Convert Brightness to Exposure Value (EV)
    private double ConvertToEV(double brightness)
    {
        return Math.Log2(brightness / 100.0 + 1); // Normalize to EV Scale
    }

    // 📌 Step 5: Compare User Settings to Ideal Exposure
    private string CompareExposure(double ev, double iso, double shutter, double aperture)
    {
        // Calculate ideal EV for given settings (EV100 Standard)
        double idealEV = Math.Log2((aperture * aperture) / shutter) - Math.Log2(iso / 100.0);

        if (ev < idealEV - 1) return "Underexposed";
        if (ev > idealEV + 1) return "Overexposed";
        return "Proper Exposure";
    }
   

    private void ShutterSpeed_Picker_SelectedIndexChanged(object sender, EventArgs e)
    {
        shutterSpeed = Convert.ToDouble(((Picker)sender).SelectedItem.ToString().Replace("1/",""));
    }

    private void fstop_Picker_SelectedIndexChanged(object sender, EventArgs e)
    {
        aperture = Convert.ToDouble(((Picker)sender).SelectedItem.ToString().Replace("f/", ""));
    }

    private void ISO_Picker_SelectedIndexChanged(object sender, EventArgs e)
    {
        iso = Convert.ToDouble(((Picker)sender).SelectedItem.ToString().Replace("f/", ""));
    }

   
}