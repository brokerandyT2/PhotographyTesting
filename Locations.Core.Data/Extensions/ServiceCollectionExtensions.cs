using Microsoft.Extensions.DependencyInjection;
using Locations.Core.Data.Queries;
using Locations.Core.Data.Queries.Interfaces;

namespace Locations.Core.Data.Extensions
{
    /// <summary>
    /// Extension methods for registering repositories with the dependency injection container
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Registers all repositories with the dependency injection container
        /// </summary>
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            // Register repositories
            services.AddTransient<ILocationRepository, LocationRepository>();
            services.AddTransient<ISettingsRepository, SettingsRepository>();
            services.AddTransient<IWeatherRepository, WeatherRepository>();
            services.AddTransient<ITipRepository, TipRepository>();
            services.AddTransient<ITipTypeRepository, TipTypeRepository>();

            return services;
        }
    }
}