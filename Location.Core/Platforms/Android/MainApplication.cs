using aa= Android.App;
using Android.Runtime;
using Android.Views;
using Location.Core.Platforms.Android.Interface;

namespace Location.Core.Platforms.Android;

[Application(UsesCleartextTraffic = true)]
public class MainApplication : MauiApplication
{
	public MainApplication(nint handle, JniHandleOwnership ownership)
		: base(handle, ownership)
	{
	}

    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
}
