using TechTalk.SpecFlow;
using BoDi;
using MockFactory = Locations.Core.Business.BDD.TestHelpers.MockFactory;
using Moq;
using Location.Core.Helpers.AlertService;
using Location.Core.Helpers.LoggingService;
using Locations.Core.Data.Queries.Interfaces;
using Locations.Core.Business.DataAccess.Interfaces;
using Locations.Core.Business.DataAccess.Services;
using Locations.Core.Shared.ViewModels;
using Locations.Core.Business.BDD.TestHelpers;

namespace Locations.Core.Business.BDD.Hooks
{
    [Binding]
    public class ServiceTestHooks
    {
        private readonly IObjectContainer _objectContainer;

        public ServiceTestHooks(IObjectContainer objectContainer)
        {
            _objectContainer = objectContainer;
        }

        [BeforeScenario]
        public void BeforeScenario()
        {
            // Create mocks for system services
            var mockAlertService = MockFactory.CreateAlertServiceMock();
            var mockLoggerService = MockFactory.CreateLoggerServiceMock();

            // Create repository mocks
            var mockLocationRepository = MockFactory.CreateLocationRepositoryMock();
            var mockWeatherRepository = MockFactory.CreateWeatherRepositoryMock();
            var mockTipRepository = MockFactory.CreateTipRepositoryMock();
            var mockTipTypeRepository = MockFactory.CreateTipTypeRepositoryMock();
            var mockSettingsRepository = MockFactory.CreateSettingsRepositoryMock();

            // Register mocks in container
            _objectContainer.RegisterInstanceAs(mockAlertService);
            _objectContainer.RegisterInstanceAs(mockLoggerService);
            _objectContainer.RegisterInstanceAs(mockAlertService.Object);
            _objectContainer.RegisterInstanceAs(mockLoggerService.Object);

            _objectContainer.RegisterInstanceAs(mockLocationRepository);
            _objectContainer.RegisterInstanceAs(mockWeatherRepository);
            _objectContainer.RegisterInstanceAs(mockTipRepository);
            _objectContainer.RegisterInstanceAs(mockTipTypeRepository);
            _objectContainer.RegisterInstanceAs(mockSettingsRepository);

            _objectContainer.RegisterInstanceAs(mockLocationRepository.Object);
            _objectContainer.RegisterInstanceAs(mockWeatherRepository.Object);
            _objectContainer.RegisterInstanceAs(mockTipRepository.Object);
            _objectContainer.RegisterInstanceAs(mockTipTypeRepository.Object);
            _objectContainer.RegisterInstanceAs(mockSettingsRepository.Object);

            // Create service instances with mocked dependencies
            var locationService = new LocationService<LocationViewModel>(
                mockLocationRepository.Object,
                mockAlertService.Object,
                mockLoggerService.Object);

            var weatherService = new WeatherService<WeatherViewModel>(
                mockWeatherRepository.Object,
                mockAlertService.Object,
                mockLoggerService.Object,
                locationService);

            var tipService = new TipService<TipViewModel>(
                mockTipRepository.Object,
                mockAlertService.Object,
                mockLoggerService.Object);

            var tipTypeService = new TipTypeService<TipTypeViewModel>(
                mockTipTypeRepository.Object,
                mockAlertService.Object,
                mockLoggerService.Object);

            var settingsService = new SettingsService<SettingViewModel>(
                mockSettingsRepository.Object,
                mockAlertService.Object,
                mockLoggerService.Object);

            // Register services in container
            _objectContainer.RegisterInstanceAs<ILocationService<LocationViewModel>>(locationService);
            _objectContainer.RegisterInstanceAs<IWeatherService<WeatherViewModel>>(weatherService);
            _objectContainer.RegisterInstanceAs<ITipService<TipViewModel>>(tipService);
            _objectContainer.RegisterInstanceAs<ITipTypeService<TipTypeViewModel>>(tipTypeService);
            _objectContainer.RegisterInstanceAs<ISettingService<SettingViewModel>>(settingsService);

            // Register concrete instances for direct access in step definitions
            _objectContainer.RegisterInstanceAs(locationService);
            _objectContainer.RegisterInstanceAs(weatherService);
            _objectContainer.RegisterInstanceAs(tipService);
            _objectContainer.RegisterInstanceAs(tipTypeService);
            _objectContainer.RegisterInstanceAs(settingsService);
        }

        [AfterScenario]
        public void AfterScenario()
        {
            // Clean up if necessary
            _objectContainer.Dispose();
        }
    }
}