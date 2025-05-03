
using Camera.MAUI;
using CommunityToolkit.Maui;
using epj.Expander.Maui;
using Locations.Core.Shared.Customizations.Alerts.Implementation;
using Locations.Core.Shared.Customizations.Alerts.Interfraces;
using Locations.Core.Shared.Customizations.Logging.Implementation;
using Locations.Core.Shared.Customizations.Logging.Interfaces;
using Locations.Core.Shared.ViewModels;
using Locations.Core.Shared.ViewModels.Interface;
using Microsoft.Extensions.Logging;
using Serilog;
using SkiaSharp.Views.Maui.Controls.Hosting;
using ZXing.Net.Maui.Controls;
#if ANDROID

#endif
namespace Location.Core;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
        Log.Logger = new LoggerConfiguration()
          .WriteTo.Console()         // Log to the console
          .MinimumLevel.Information() // Set minimum log level to Information
          .CreateLogger();

        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .UseMauiCameraView()
            .UseExpander()
            .UseBarcodeReader()
            .UseSkiaSharp()

			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});
        builder.Services.AddTransient<ILocationViewModel, LocationViewModel>();
        builder.Services.AddTransient<IWeatherViewModel, WeatherViewModel>();
        builder.Services.AddTransient<ITip, TipViewModel>();
        builder.Services.AddTransient<ITipType, TipTypeViewModel>();
        builder.Services.AddTransient<ISettingsViewModel, SettingsViewModel>();
        builder.Services.AddTransient<ILocationList, LocationsListViewModel>();
        builder.Services.AddTransient<ITipsViewmodel, TipsViewModel>();
        builder.Services.AddTransient<IDetailsView, DetailsViewModel>();
        builder.Services.AddSingleton<IAlertService, AlertService>();
        builder.Services.AddSingleton<ILoggerService, LoggerService>();
        builder.Services.AddTransient(typeof(Locations.Core.Data.Queries.SettingsQuery<>));


#if ANDROID
        //builder.Services.AddSingleton<Platforms.Android.IGoogleAuthService, Platforms.Android.GoogleAuthService>();
#endif
        builder.Services.AddLogging(logging =>
        {
            logging.ClearProviders();   // Remove default providers
            logging.AddSerilog();       // Add Serilog as the logging provider
        });
        Expander.EnableAnimations();
        return builder.Build();
	}
}
