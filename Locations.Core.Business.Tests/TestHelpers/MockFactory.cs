// MockFactory.cs - Enhancement of existing class
using Location.Core.Helpers.AlertService;
using Location.Core.Helpers.LoggingService;
using Locations.Core.Business.DataAccess.Interfaces;
using Locations.Core.Data.Models;
using Locations.Core.Data.Queries.Interfaces;
using Locations.Core.Shared.ViewModels;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ErrorSource = Locations.Core.Data.Models.ErrorSource;
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
            var testLocations = TestDataFactory.CreateTestLocations();

            // Setup default behaviors
            mock.Setup(r => r.GetAllAsync())
                .ReturnsAsync(DataOperationResult<IList<LocationViewModel>>.Success(testLocations));

            // Setup GetByIdAsync
            mock.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                .Returns((int id) => Task.FromResult(
                    id <= testLocations.Count && id > 0
                        ? DataOperationResult<LocationViewModel>.Success(testLocations[id - 1])
                        : DataOperationResult<LocationViewModel>.Failure(ErrorSource.Database, $"Location with ID {id} not found")
                ));

            // Setup GetByCoordinatesAsync
            mock.Setup(r => r.GetByCoordinatesAsync(It.IsAny<double>(), It.IsAny<double>()))
                .Returns((double lat, double lon) => Task.FromResult(
                    DataOperationResult<LocationViewModel>.Success(
                        new LocationViewModel { Id = 1, Lattitude = lat, Longitude = lon, Title = "Test Location" }
                    )
                ));

            // Setup SaveAsync
            mock.Setup(r => r.SaveAsync(It.IsAny<LocationViewModel>()))
                .Returns((LocationViewModel loc) => {
                    if (loc.Id <= 0) loc.Id = testLocations.Count + 1;
                    return Task.FromResult(DataOperationResult<LocationViewModel>.Success(loc));
                });

            // Setup UpdateAsync
            mock.Setup(r => r.UpdateAsync(It.IsAny<LocationViewModel>()))
                .Returns((LocationViewModel loc) => {
                    return Task.FromResult(DataOperationResult<bool>.Success(true));
                });

            // Setup DeleteAsync
            mock.Setup(r => r.DeleteAsync(It.IsAny<int>()))
                .Returns((int id) => Task.FromResult(DataOperationResult<bool>.Success(true)));

            mock.Setup(r => r.DeleteAsync(It.IsAny<LocationViewModel>()))
                .Returns((LocationViewModel loc) => Task.FromResult(DataOperationResult<bool>.Success(true)));

            return mock;
        }

        /// <summary>
        /// Creates a mocked settings repository
        /// </summary>
        public static Mock<ISettingsRepository> CreateSettingsRepositoryMock()
        {
            var mock = new Mock<ISettingsRepository>();
            var testSettings = new List<SettingViewModel>
            {
                TestDataFactory.CreateTestSetting("Hemisphere", "north"),
                TestDataFactory.CreateTestSetting("DateFormat", "MM/dd/yyyy"),
                TestDataFactory.CreateTestSetting("TimeFormat", "h:mm tt")
            };

            // Setup default behaviors
            mock.Setup(r => r.GetAllAsync())
                .ReturnsAsync(DataOperationResult<IList<SettingViewModel>>.Success(testSettings));

            // Setup GetByIdAsync
            mock.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                .Returns((int id) => Task.FromResult(
                    id <= testSettings.Count && id > 0
                        ? DataOperationResult<SettingViewModel>.Success(testSettings[id - 1])
                        : DataOperationResult<SettingViewModel>.Failure(ErrorSource.Database, $"Setting with ID {id} not found")
                ));

            // Setup GetByNameAsync
            mock.Setup(r => r.GetByNameAsync(It.IsAny<string>()))
                .Returns((string name) => Task.FromResult(
                    testSettings.Find(s => s.Key == name) != null
                        ? DataOperationResult<SettingViewModel>.Success(testSettings.Find(s => s.Key == name))
                        : DataOperationResult<SettingViewModel>.Failure(ErrorSource.Database, $"Setting with name '{name}' not found")
                ));

            // Setup GetValueByNameAsync
            mock.Setup(r => r.GetValueByNameAsync(It.IsAny<string>()))
                .Returns((string name) => Task.FromResult(
                    testSettings.Find(s => s.Key == name) != null
                        ? DataOperationResult<string>.Success(testSettings.Find(s => s.Key == name).Value)
                        : DataOperationResult<string>.Failure(ErrorSource.Database, $"Setting with name '{name}' not found")
                ));

            // Setup SaveAsync
            mock.Setup(r => r.SaveAsync(It.IsAny<SettingViewModel>()))
                .Returns((SettingViewModel setting) => {
                    if (setting.Id <= 0) setting.Id = testSettings.Count + 1;
                    return Task.FromResult(DataOperationResult<SettingViewModel>.Success(setting));
                });

            // Setup UpdateAsync
            mock.Setup(r => r.UpdateAsync(It.IsAny<SettingViewModel>()))
                .Returns((SettingViewModel setting) => {
                    return Task.FromResult(DataOperationResult<bool>.Success(true));
                });

            // Setup DeleteAsync
            mock.Setup(r => r.DeleteAsync(It.IsAny<int>()))
                .Returns((int id) => Task.FromResult(DataOperationResult<bool>.Success(true)));

            mock.Setup(r => r.DeleteAsync(It.IsAny<SettingViewModel>()))
                .Returns((SettingViewModel setting) => Task.FromResult(DataOperationResult<bool>.Success(true)));

            // Setup SaveSettingAsync
            mock.Setup(r => r.SaveSettingAsync(It.IsAny<string>(), It.IsAny<string>()))
                .Returns((string name, string value) => Task.FromResult(DataOperationResult<bool>.Success(true)));

            return mock;
        }

        /// <summary>
        /// Creates a mocked weather repository
        /// </summary>
        public static Mock<IWeatherRepository> CreateWeatherRepositoryMock()
        {
            var mock = new Mock<IWeatherRepository>();
            var testWeather = TestDataFactory.CreateTestWeather();

            // Setup default behaviors
            mock.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                .Returns((int id) => Task.FromResult(DataOperationResult<WeatherViewModel>.Success(
                    TestDataFactory.CreateTestWeather(id))));

            // Setup GetByLocationIdAsync
            mock.Setup(r => r.GetByLocationIdAsync(It.IsAny<int>()))
                .Returns((int id) => Task.FromResult(DataOperationResult<WeatherViewModel>.Success(
                    TestDataFactory.CreateTestWeather(id))));

            // Setup GetByCoordinatesAsync
            mock.Setup(r => r.GetByCoordinatesAsync(It.IsAny<double>(), It.IsAny<double>()))
                .Returns((double lat, double lon) => Task.FromResult(DataOperationResult<WeatherViewModel>.Success(
                    new WeatherViewModel { Id = 1, Latitude = lat, Longitude = lon })));

            // Setup GetByCoordinatesOrDefaultAsync
            mock.Setup(r => r.GetByCoordinatesOrDefaultAsync(It.IsAny<double>(), It.IsAny<double>()))
                .Returns((double lat, double lon) => Task.FromResult(DataOperationResult<WeatherViewModel>.Success(
                    new WeatherViewModel { Id = 1, Latitude = lat, Longitude = lon })));

            return mock;
        }

        /// <summary>
        /// Creates a mocked tip repository
        /// </summary>
        public static Mock<ITipRepository> CreateTipRepositoryMock()
        {
            var mock = new Mock<ITipRepository>();
            var testTips = TestDataFactory.CreateTestTips();

            // Setup default behaviors
            mock.Setup(r => r.GetAllAsync())
                .ReturnsAsync(DataOperationResult<IList<TipViewModel>>.Success(testTips));

            // Setup GetByIdAsync
            mock.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                .Returns((int id) => Task.FromResult(
                    id <= testTips.Count && id > 0
                        ? DataOperationResult<TipViewModel>.Success(testTips[id - 1])
                        : DataOperationResult<TipViewModel>.Failure(ErrorSource.Database, $"Tip with ID {id} not found")
                ));

            // Setup GetByTitleAsync
            mock.Setup(r => r.GetByTitleAsync(It.IsAny<string>()))
                .Returns((string title) => Task.FromResult(
                    testTips.Find(t => t.Title == title) != null
                        ? DataOperationResult<TipViewModel>.Success(testTips.Find(t => t.Title == title))
                        : DataOperationResult<TipViewModel>.Failure(ErrorSource.Database, $"Tip with title '{title}' not found")
                ));

            return mock;
        }

        /// <summary>
        /// Creates a mocked tip type repository
        /// </summary>
        public static Mock<ITipTypeRepository> CreateTipTypeRepositoryMock()
        {
            var mock = new Mock<ITipTypeRepository>();
            var testTipTypes = new List<TipTypeViewModel> { TestDataFactory.CreateTestTipType() };

            // Setup default behaviors
            mock.Setup(r => r.GetAllAsync())
                .ReturnsAsync(DataOperationResult<IList<TipTypeViewModel>>.Success(testTipTypes));

            // Setup GetByIdAsync
            mock.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                .Returns((int id) => Task.FromResult(
                    id <= testTipTypes.Count && id > 0
                        ? DataOperationResult<TipTypeViewModel>.Success(testTipTypes[id - 1])
                        : DataOperationResult<TipTypeViewModel>.Failure(ErrorSource.Database, $"Tip type with ID {id} not found")
                ));

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
            mock.Setup(l => l.LogWarning(It.IsAny<string>(), It.IsAny<Exception>())).Verifiable();
            mock.Setup(l => l.LogError(It.IsAny<string>(), It.IsAny<Exception>())).Verifiable();

            return mock;
        }

        /// <summary>
        /// Creates a mocked native storage service
        /// </summary>
        public static Mock<INativeStorageService> CreateNativeStorageServiceMock()
        {
            var mock = new Mock<INativeStorageService>();

            // Setup default behaviors
            mock.Setup(s => s.GetSettingAsync(It.IsAny<string>()))
                .Returns((string key) => Task.FromResult($"Value for {key}"));

            mock.Setup(s => s.SaveSettingAsync(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            mock.Setup(s => s.SaveSettingAsync(It.IsAny<string>(), It.IsAny<object>()))
                .Returns(Task.CompletedTask);

            mock.Setup(s => s.UpdateSettingAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(true);

            mock.Setup(s => s.UpdateSettingAsync(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<object>()))
                .ReturnsAsync(true);

            mock.Setup(s => s.DeleteSettingAsync(It.IsAny<string>()))
                .ReturnsAsync(true);

            return mock;
        }
    }
}