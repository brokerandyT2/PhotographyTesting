using Android.App;
using Android.Content.PM;
using ac = Android;
using Microsoft.Maui.Authentication;
using Microsoft.Maui.Controls;
using Android.Util;
namespace Location.Core.Platforms.Android
{
    /*[Activity(
            NoHistory = true,
            Exported = true,
            LaunchMode = LaunchMode.SingleTop)]
    [IntentFilter(
            new[] { ac.Content.Intent.ActionView },
            Categories = new[] { ac.Content.Intent.CategoryDefault, ac.Content.Intent.CategoryBrowsable },
            DataScheme = "com.googleusercontent.apps.1064488372076-9juboi2k00j5rta6f5vbl1q3dr5cn56b")] // <-- Your actual scheme here*/
    public class WebAuthenticationCallbackActivity : Microsoft.Maui.Authentication.WebAuthenticatorCallbackActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            WebAuthenticator.Default.OnResume(this.Intent);
            var intentData = Intent?.DataString;
            if (!string.IsNullOrEmpty(intentData))
            {
                Log.Debug("WebAuthCallback", $"Received redirect URI: {intentData}");
            }
            else
            {
                Log.Debug("WebAuthCallback", "No intent data received.");
            }
        }
    }
}
