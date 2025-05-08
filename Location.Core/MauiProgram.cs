
using Camera.MAUI;
using CommunityToolkit.Maui;
using epj.Expander.Maui;
using Location.Core.Platforms.Android.Implementation;
using Location.Core.Platforms.Android.Interface;

using Locations.Core.Shared.Customizations.Alerts.Implementation;
using Locations.Core.Shared.Customizations.Alerts.Interfraces;
using Locations.Core.Shared.ViewModels;
using Locations.Core.Shared.ViewModels.Interface;
using Microsoft.Extensions.Logging;
using Serilog;
using SkiaSharp.Views.Maui.Controls.Hosting;
using ZXing.Net.Maui.Controls;
using Location.Core.Logging;
using ILogger = Location.Core.Logging.ILogger;
using Locations.Core.Shared.StorageSvc;

#if ANDROID

#endif
namespace Location.Core;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {

        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .UseMauiCameraView()
            .UseExpander()
            .UseBarcodeReader()
            .UseSkiaSharp()
            .UseMauiCameraView()
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
        builder.Services.AddSingleton<ILogger, Logger>();
        builder.Services.AddSingleton<IAmbientLightSensorService, AmbientLightSensorService>();
        builder.Services.AddTransient(typeof(Locations.Core.Data.Queries.SettingsQuery<>));
        builder.Services.AddSingleton<INativeStorageService, NativeStorageService>();

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
