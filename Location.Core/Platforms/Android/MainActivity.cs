using aa = Android.App;

using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using AndroidX.Activity.Result;
using AndroidX.Activity.Result.Contract;
using Android.Content;
using Android.Views;
using Location.Core.Platforms.Android.Interface;
using Microsoft.Maui.Controls;

namespace Location.Core.Platforms.A;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
    public override bool OnKeyDown(Keycode keyCode, KeyEvent e)
    {
        if (keyCode == Keycode.VolumeDown || keyCode == Keycode.VolumeUp)
        {
            var currentPage = Microsoft.Maui.Controls.Application.Current?.MainPage?.Navigation?.NavigationStack?.LastOrDefault();

            if (currentPage is IVolumeKeyHandler volumeHandler)
            {
                volumeHandler.OnVolumeKeyPressed();
                return true; // consume event
            }
        }

        return base.OnKeyDown(keyCode, e);
    }
}
public static class AppContextProvider
{
    private static Location.Core.App appInstance = new Location.Core.App();
    public static Context Context => aa.Application.Context;// Android.App.Application.Context;
}