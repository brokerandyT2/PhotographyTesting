

using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using AndroidX.Activity.Result;
using AndroidX.Activity.Result.Contract;
using Android.Content;

namespace Location.Core.Platforms.A;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{

}
public static class AppContextProvider
{
    private static Location.Core.App appInstance = new Location.Core.App();
    public static Context Context => aa.Application.Context;// Android.App.Application.Context;
}