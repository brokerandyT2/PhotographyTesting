using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Java.Interop;
using Java.Util;


namespace Location.Core.Platforms.Android;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
    
}
/*
[Activity(Name = "com.x3squaredcircles.locations.WebAuthenticationCallbackActivity", NoHistory = true, LaunchMode = LaunchMode.SingleTop, Exported =true)]
[IntentFilter(new[] { Intent.ActionView }, Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable }, DataScheme = "com.x3squaredcircles.locations")]
public class WebAuthenticationCallbackActivity : WebAuthenticatorCallbackActivity
{
}
*/