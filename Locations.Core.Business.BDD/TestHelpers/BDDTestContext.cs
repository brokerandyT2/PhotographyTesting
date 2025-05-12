using Moq;
using Location.Core.Helpers.AlertService;
using Location.Core.Helpers.LoggingService;
using Locations.Core.Data.Queries.Interfaces;
using Locations.Core.Business.DataAccess.Interfaces;
using Locations.Core.Shared.ViewModels;
using System.Collections.Generic;

namespace Locations.Core.Business.BDD.TestHelpers
{
    /// <summary>
    /// Test context for BDD scenarios to store mocks and test data
    /// </summary>
    public class BDDTestContext
    {
        public Mock<IAlertService> MockAlertService { get; set; }
        public Mock<ILoggerService> MockLoggerService { get; set; }
        public Mock<ILocationRepository> MockLocationRepository { get; set; }
        public Mock<IWeatherRepository> MockWeatherRepository { get; set; }
        public Mock<ITipRepository> MockTipRepository { get; set; }
        public Mock<ITipTypeRepository> MockTipTypeRepository { get; set; }
        public Mock<ISettingsRepository> MockSettingsRepository { get; set; }

        // Test data storage
        public Dictionary<string, SettingViewModel> TestSettings { get; set; } = new Dictionary<string, SettingViewModel>();
        public List<LocationViewModel> TestLocations { get; set; } = new List<LocationViewModel>();
        public List<WeatherViewModel> TestWeatherData { get; set; } = new List<WeatherViewModel>();
        public List<TipViewModel> TestTips { get; set; } = new List<TipViewModel>();
        public List<TipTypeViewModel> TestTipTypes { get; set; } = new List<TipTypeViewModel>();

        // Current context data
        public LocationViewModel CurrentLocation { get; set; }
        public WeatherViewModel CurrentWeather { get; set; }
        public TipViewModel CurrentTip { get; set; }
        public string CurrentUserEmail { get; set; } = "test@example.com";
        public bool IsUserLoggedIn { get; set; } = false;
        public string SubscriptionType { get; set; } = "Free";
    }
}