using Camera.MAUI;
using Locations.Core.Shared;
using Locations.Core.Business.DataAccess;
using SkiaSharp;
using static Microsoft.Maui.ApplicationModel.Permissions;
using System.ComponentModel;
using Syncfusion.Maui.Toolkit.Internals;
using System.IO;
using Location.Core.Helpers;
namespace Location.Core.Views.Premium;

public partial class LightMeter : ContentPage
{
    SettingsService ss = new SettingsService();
    private double iso;
    private double shutterSpeed;  // User-specified shutter speed (seconds)
    private double aperture; // User-specified f-stop
    private bool _activate = false;
    private double _brightness;
    private double _ev;
    private string _exposure;
    public LightMeter()
    {
        InitializeComponent();
        cameraView.CamerasLoaded += CameraView_CamerasLoaded;
    }
    protected override void OnNavigatedFrom(NavigatedFromEventArgs args)
    {
        _activate = false;
        base.OnNavigatedFrom(args);


        var y = cameraView.StopCameraAsync().Result;
        BeginCapture(false);
    }
    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        _activate = true;
        base.OnNavigatedTo(args);
        Locations.Core.Business.DataAccess.SettingsService ss = new Locations.Core.Business.DataAccess.SettingsService();


        var isAds = ss.GetSettingByName(MagicStrings.FreePremiumAdSupported).ToBoolean();

        PageHelpers.CheckVisit(MagicStrings.LightMeterViewed, PageEnums.LightMeter, ss, Navigation);

        //BeginCapture(_activate);
    }

    private void BeginCapture(bool active)
    {
        string localFilePath = Path.Combine(Microsoft.Maui.Storage.FileSystem.AppDataDirectory, Guid.NewGuid() + ".png");
        while (active)
        {
            if (cameraView.IsLoaded)
            {
                Stream stream = cameraView.TakePhotoAsync().Result;
                using FileStream localStream = File.OpenWrite(localFilePath);
                stream.CopyTo(localStream);

                SkiaSharp.SKBitmap bmp = SKBitmap.Decode(localFilePath);

                _brightness = CalculateBrightness(bmp.Bytes, bmp.Height, bmp.Width);
                _ev = ConvertToEV(_brightness);
                _exposure = CompareExposure(_ev, iso, shutterSpeed, aperture);

            }
            Thread.Sleep(Convert.ToInt32(ss.GetSettingByName(MagicStrings.CameraRefresh).Value));
            var x = string.Empty;
        }
    }

    private void CameraView_CamerasLoaded(object? sender, EventArgs e)
    {

        foreach (var camera in new CameraView().Cameras)
        {
            if (camera.Position == CameraPosition.Back)
            {
                cameraView.Camera = camera;
                cameraView.PropertyChanged -= CameraView_PropertyChanged;
                // cameraView.PropertyChanged += CameraView_PropertyChanged;

                /* cameraView.TakeAutoSnapShot = true;
                 cameraView.AutoSnapShotAsImageSource = true;
                 cameraView.AutoSnapShotFormat = Camera.MAUI.ImageFormat.PNG;
                 cameraView.AutoSnapShotSeconds = TimeSpan.FromMilliseconds(Convert.ToInt32(ss.GetSettingByName(MagicStrings.CameraRefresh).Value)).Seconds; */
                cameraView.StartCameraAsync();


            }
        }
    }

    private void CameraView_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {

        var y = (CameraView)sender;
        var x = y.SnapShotStream;


        if (e.PropertyName == nameof(cameraView.SnapShotStream))// && cameraView.SnapShotStream != null)
        {
            string localFilePath = Path.Combine(Microsoft.Maui.Storage.FileSystem.AppDataDirectory, Guid.NewGuid() + ".png");

            Stream stream = cameraView.SnapShotStream;
            using FileStream localFileStream = File.OpenWrite(localFilePath);

            stream.CopyToAsync(localFileStream);
            SkiaSharp.SKBitmap bmp = SKBitmap.Decode(localFilePath);

            _brightness = CalculateBrightness(bmp.Bytes, bmp.Height, bmp.Width);
            _ev = ConvertToEV(_brightness);
            _exposure = CompareExposure(_ev, iso, shutterSpeed, aperture);

            var exists = File.Exists(localFilePath);
            if (exists)
                File.Delete(localFilePath);
        }
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
        shutterSpeed = Convert.ToDouble(((Picker)sender).SelectedItem.ToString().Replace("1/", ""));
    }

    private void fstop_Picker_SelectedIndexChanged(object sender, EventArgs e)
    {
        aperture = Convert.ToDouble(((Picker)sender).SelectedItem.ToString().Replace("f/", ""));
    }

    private void ISO_Picker_SelectedIndexChanged(object sender, EventArgs e)
    {
        iso = Convert.ToDouble(((Picker)sender).SelectedItem.ToString().Replace("f/", ""));
    }

    private void Button_Pressed(object sender, EventArgs e)
    {
        var stream = cameraView.TakePhotoAsync().Result;
        string localFilePath = string.Empty;
        if (stream != null)
        {
            localFilePath = Path.Combine(Microsoft.Maui.Storage.FileSystem.AppDataDirectory, Guid.NewGuid() + ".png");
            var _ = cameraView.SaveSnapShot(Camera.MAUI.ImageFormat.PNG, localFilePath);

            // save the file into local storage

            /* using Stream sourceStream = photo.OpenReadAsync().Result;
             using FileStream localFileStream = File.OpenWrite(localFilePath);
             sourceStream.CopyToAsync(localFileStream);

             SkiaSharp.SKBitmap bmp = SKBitmap.Decode(localFilePath);

             _brightness = CalculateBrightness(bmp.Bytes, bmp.Height, bmp.Width);
             _ev = ConvertToEV(_brightness);
             _exposure = CompareExposure(_ev, iso, shutterSpeed, aperture);

             var exists = File.Exists(localFilePath);
             if (exists)
                 File.Delete(localFilePath);*/
        }
    }
}