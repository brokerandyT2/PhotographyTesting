
using Microsoft.Extensions.Logging;
using epj.Expander;
using Locations.Core.Shared.ViewModels.Interface;
using Locations.Core.Shared.ViewModels;
using epj.Expander.Maui;
using CommunityToolkit.Maui;
using Camera.MAUI;


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
#if DEBUG
        builder.Logging.AddDebug();
#endif
        Expander.EnableAnimations();
        return builder.Build();
	}
}
