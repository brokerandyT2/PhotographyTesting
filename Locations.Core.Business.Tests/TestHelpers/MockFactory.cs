// MockFactory.cs
using Location.Core.Helpers.AlertService;
using Locations.Core.Business.DataAccess.Interfaces;
using Locations.Core.Data.Models;
using Locations.Core.Data.Queries.Interfaces;
using Locations.Core.Shared.ViewModels;
using Moq;
//using ILoggerService = Locations.Core.Business.DataAccess.Interfaces.ILoggerService;
using INativeStorageService = Locations.Core.Business.DataAccess.Interfaces.INativeStorageService;

namespace Locations.Core.Business.Tests.TestHelpers
{
    /// <summary>
    /// Factory class to create and configure mocks for unit tests
    /// </summary>
    public static class MockFactory
    {
        /// <summary>
        /// Creates a mocked location repository
        /// </summary>
        public static Mock<ILocationRepository> CreateLocationRepositoryMock()
        {
            var mock = new Mock<ILocationRepository>();

            // Setup default behaviors
            mock.Setup(r => r.GetAllAsync())
                .ReturnsAsync(DataOperationResult<IList<LocationViewModel>>.Success(
                    new List<LocationViewModel>(TestDataFactory.CreateTestLocations())));

            return mock;
        }

        /// <summary>
        /// Creates a mocked settings repository
        /// </summary>
        public static Mock<ISettingsRepository> CreateSettingsRepositoryMock()
        {
            var mock = new Mock<ISettingsRepository>();

            // Setup default behaviors
            mock.Setup(r => r.GetAllAsync())
                .ReturnsAsync(DataOperationResult<IList<SettingViewModel>>.Success(
                    new List<SettingViewModel>
                    {
                        TestDataFactory.CreateTestSetting("Hemisphere", "north"),
                        TestDataFactory.CreateTestSetting("DateFormat", "MM/dd/yyyy"),
                        TestDataFactory.CreateTestSetting("TimeFormat", "h:mm tt")
                    }));

            return mock;
        }

        /// <summary>
        /// Creates a mocked weather repository
        /// </summary>
        public static Mock<IWeatherRepository> CreateWeatherRepositoryMock()
        {
            var mock = new Mock<IWeatherRepository>();

            // Setup default behaviors
            mock.Setup(r => r.GetByLocationIdAsync(It.IsAny<int>()))
                .ReturnsAsync((int id) => DataOperationResult<WeatherViewModel>.Success(
                    TestDataFactory.CreateTestWeather(id)));

            return mock;
        }

        /// <summary>
        /// Creates a mocked tip repository
        /// </summary>
        public static Mock<ITipRepository> CreateTipRepositoryMock()
        {
            var mock = new Mock<ITipRepository>();

            // Setup default behaviors
            mock.Setup(r => r.GetAllAsync())
                .ReturnsAsync(DataOperationResult<IList<TipViewModel>>.Success(
                    new List<TipViewModel>(TestDataFactory.CreateTestTips())));

            return mock;
        }

        /// <summary>
        /// Creates a mocked tip type repository
        /// </summary>
        public static Mock<ITipTypeRepository> CreateTipTypeRepositoryMock()
        {
            var mock = new Mock<ITipTypeRepository>();

            // Setup default behaviors
            mock.Setup(r => r.GetAllAsync())
                .ReturnsAsync(DataOperationResult<IList<TipTypeViewModel>>.Success(
                    new List<TipTypeViewModel> { TestDataFactory.CreateTestTipType() }));

            return mock;
        }

        /// <summary>
        /// Creates a mocked alert service
        /// </summary>
        public static Mock<IAlertService> CreateAlertServiceMock()
        {
            return new Mock<IAlertService>();
        }

        /// <summary>
        /// Creates a mocked logger service
        /// </summary>
        public static Mock<ILoggerService> CreateLoggerServiceMock()
        {
            var mock = new Mock<ILoggerService>();

            // Setup default behaviors
            mock.Setup(l => l.LogInformation(It.IsAny<string>())).Verifiable();
            mock.Setup(l => l.LogWarning(It.IsAny<string>())).Verifiable();
            mock.Setup(l => l.LogError(It.IsAny<string>())).Verifiable();
            mock.Setup(l => l.LogError(It.IsAny<string>(), It.IsAny<Exception>())).Verifiable();

            return mock;
        }

        /// <summary>
        /// Creates a mocked business logger service
        /// </summary>
        public static Mock<Locations.Core.Business.DataAccess.Interfaces.ILoggerService> CreateBusinessLoggerServiceMock()
        {
            var mock = new Mock<Locations.Core.Business.DataAccess.Interfaces.ILoggerService>();

            // Setup default behaviors
            mock.Setup(l => l.LogInformation(It.IsAny<string>())).Verifiable();
            mock.Setup(l => l.LogWarning(It.IsAny<string>())).Verifiable();
            mock.Setup(l => l.LogError(It.IsAny<string>())).Verifiable();
            mock.Setup(l => l.LogError(It.IsAny<string>(), It.IsAny<Exception>())).Verifiable();

            return mock;
        }

        /// <summary>
        /// Creates a mocked native storage service
        /// </summary>
        public static Mock<INativeStorageService> CreateNativeStorageServiceMock()
        {
            return new Mock<INativeStorageService>();
        }
    }
}