
using Camera.MAUI;
using CommunityToolkit.Maui;
using EncryptedSQLite;
using epj.Expander.Maui;
using Location.Core.Helpers.LoggingService;
using Location.Core.Platforms.Android.Implementation;
using Location.Core.Platforms.Android.Interface;
using Locations.Core.Shared.StorageSvc;
using Locations.Core.Shared.ViewModels;
using Locations.Core.Shared.ViewModels.Interface;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Hosting;
using Serilog;
using SkiaSharp.Views.Maui.Controls.Hosting;
using SQLite;
using ZXing.Net.Maui.Controls;

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
        builder.Services.AddTransient<MainPage>();
        builder.Services.AddTransient<ITipsViewmodel, TipsViewModel>();
        builder.Services.AddTransient<IDetailsView, DetailsViewModel>();

        builder.Services.AddSingleton<IAmbientLightSensorService, AmbientLightSensorService>();
        builder.Services.AddSingleton<INativeStorageService, NativeStorageService>();
        builder.Services.AddSingleton<LoggerService>();
        builder.Services.AddSingleton<ISQLiteAsyncConnection>(sp => DataEncrypted.GetAsyncConnection());
        builder.Services.AddSingleton<ILoggerService, LoggerService>();
        builder.Services.AddSingleton<ILoggerService, LoggerService>();
#if ANDROID
        //builder.Services.AddSingleton<Platforms.Android.IGoogleAuthService, Platforms.Android.GoogleAuthService>();
#endif
        builder.Services.AddLogging(logging =>
        {
            logging.ClearProviders();   // Remove default providers
            logging.AddSerilog();       // Add Serilog as the logging provider
        });
        Expander.EnableAnimations();

        // Build the app
        var app = builder.Build();

        // Initialize ResourceProvider
        ResourceProvider.Initialize();

        return app;
    }
}
