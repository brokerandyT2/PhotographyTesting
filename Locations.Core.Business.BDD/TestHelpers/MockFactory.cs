using Moq;
using Location.Core.Helpers.AlertService;
using Location.Core.Helpers.LoggingService;
using Locations.Core.Data.Queries.Interfaces;
using Locations.Core.Data.Models;
using Locations.Core.Shared.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Locations.Core.Business.BDD.TestHelpers
{
    public static class MockFactory
    {
        public static Mock<IAlertService> CreateAlertServiceMock()
        {
            var mock = new Mock<IAlertService>();

            // Setup IAlertService methods based on the actual interface
            mock.Setup(x => x.ShowInfo(It.IsAny<string>()));
            mock.Setup(x => x.ShowWarning(It.IsAny<string>()));
            mock.Setup(x => x.ShowError(It.IsAny<string>()));

            return mock;
        }
        public static Mock<ILoggerService> CreateLoggerServiceMock()
        {
            var mock = new Mock<ILoggerService>();

            // Setup for single parameter calls
            mock.Setup(x => x.LogInformation(It.IsAny<string>()));

            // Setup for calls with a specific null exception (handles optional parameter)
            mock.Setup(x => x.LogWarning(It.IsAny<string>(), null));
            mock.Setup(x => x.LogError(It.IsAny<string>(), null));
            mock.Setup(x => x.LogDebug(It.IsAny<string>(), null));

            // Setup for calls with any exception (including null)
            mock.Setup(x => x.LogWarning(It.IsAny<string>(), It.IsAny<Exception>()));
            mock.Setup(x => x.LogError(It.IsAny<string>(), It.IsAny<Exception>()));
            mock.Setup(x => x.LogDebug(It.IsAny<string>(), It.IsAny<Exception>()));

            return mock;
        }
        public static Mock<ILocationRepository> CreateLocationRepositoryMock()
        {
            var mock = new Mock<ILocationRepository>();

            // Setup default successful responses
            mock.Setup(x => x.GetAllAsync())
                .ReturnsAsync(DataOperationResult<IList<LocationViewModel>>.Success(new List<LocationViewModel>()));

            mock.Setup(x => x.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((int id) => DataOperationResult<LocationViewModel>.Success(new LocationViewModel { Id = id }));

            mock.Setup(x => x.SaveAsync(It.IsAny<LocationViewModel>()))
                .ReturnsAsync((LocationViewModel loc) => DataOperationResult<LocationViewModel>.Success(loc));

            mock.Setup(x => x.UpdateAsync(It.IsAny<LocationViewModel>()))
                .ReturnsAsync(DataOperationResult<bool>.Success(true));

            mock.Setup(x => x.DeleteAsync(It.IsAny<int>()))
                .ReturnsAsync(DataOperationResult<bool>.Success(true));

            mock.Setup(x => x.GetByCoordinatesAsync(It.IsAny<double>(), It.IsAny<double>()))
                .ReturnsAsync((double lat, double lon) => DataOperationResult<LocationViewModel>.Success(
                    new LocationViewModel { Lattitude = lat, Longitude = lon }));

            return mock;
        }

        public static Mock<IWeatherRepository> CreateWeatherRepositoryMock()
        {
            var mock = new Mock<IWeatherRepository>();

            mock.Setup(x => x.GetAllAsync())
                .ReturnsAsync(DataOperationResult<IList<WeatherViewModel>>.Success(new List<WeatherViewModel>()));

            mock.Setup(x => x.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((int id) => DataOperationResult<WeatherViewModel>.Success(new WeatherViewModel { Id = id }));

            mock.Setup(x => x.GetByLocationIdAsync(It.IsAny<int>()))
                .ReturnsAsync((int locationId) => DataOperationResult<WeatherViewModel>.Success(
                    new WeatherViewModel { LocationId = locationId }));

            mock.Setup(x => x.GetByCoordinatesAsync(It.IsAny<double>(), It.IsAny<double>()))
                .ReturnsAsync((double lat, double lon) => DataOperationResult<WeatherViewModel>.Success(
                    new WeatherViewModel { Latitude = lat, Longitude = lon }));

            return mock;
        }

        public static Mock<ITipRepository> CreateTipRepositoryMock()
        {
            var mock = new Mock<ITipRepository>();

            mock.Setup(x => x.GetAllAsync())
                .ReturnsAsync(DataOperationResult<IList<TipViewModel>>.Success(new List<TipViewModel>()));

            mock.Setup(x => x.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((int id) => DataOperationResult<TipViewModel>.Success(new TipViewModel { Id = id }));

            mock.Setup(x => x.SaveAsync(It.IsAny<TipViewModel>()))
                .ReturnsAsync((TipViewModel tip) => DataOperationResult<TipViewModel>.Success(tip));

            return mock;
        }

        public static Mock<ITipTypeRepository> CreateTipTypeRepositoryMock()
        {
            var mock = new Mock<ITipTypeRepository>();

            mock.Setup(x => x.GetAllAsync())
                .ReturnsAsync(DataOperationResult<IList<TipTypeViewModel>>.Success(new List<TipTypeViewModel>()));

            mock.Setup(x => x.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((int id) => DataOperationResult<TipTypeViewModel>.Success(new TipTypeViewModel { Id = id }));

            return mock;
        }

        public static Mock<ISettingsRepository> CreateSettingsRepositoryMock()
        {
            var mock = new Mock<ISettingsRepository>();

            mock.Setup(x => x.GetAllAsync())
                .ReturnsAsync(DataOperationResult<IList<SettingViewModel>>.Success(new List<SettingViewModel>()));

            mock.Setup(x => x.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((int id) => DataOperationResult<SettingViewModel>.Success(new SettingViewModel { Id = id }));

            mock.Setup(x => x.GetByNameAsync(It.IsAny<string>()))
                .ReturnsAsync((string name) => DataOperationResult<SettingViewModel>.Success(
                    new SettingViewModel { Key = name, Value = string.Empty }));

            mock.Setup(x => x.SaveAsync(It.IsAny<SettingViewModel>()))
                .ReturnsAsync((SettingViewModel setting) => DataOperationResult<SettingViewModel>.Success(setting));

            mock.Setup(x => x.UpdateAsync(It.IsAny<SettingViewModel>()))
                .ReturnsAsync(DataOperationResult<bool>.Success(true));

            return mock;
        }
    }
}