// BaseServiceTests.cs
using Location.Core.Helpers.AlertService;
using Location.Core.Helpers.LoggingService;
using Moq;


namespace Locations.Core.Business.Tests.Base
{
    /// <summary>
    /// Base test class to provide common setup and functionality for service tests
    /// </summary>
    [TestClass]
    public abstract class BaseServiceTests
    {
        // Common mocks that might be used across test classes
        protected Mock<IAlertService> MockAlertService;
        protected Mock<ILoggerService> MockLoggerService;
        protected Mock<ILoggerService> MockBusinessLoggerService;

        [TestInitialize]
        public virtual void Setup()
        {
            // Initialize common mocks
            MockAlertService = new Mock<IAlertService>();
            MockLoggerService = new Mock<ILoggerService>();
            MockBusinessLoggerService = new Mock<ILoggerService>();

            // Setup common mock behaviors
            MockLoggerService.Setup(l => l.LogInformation(It.IsAny<string>())).Verifiable();
            MockLoggerService.Setup(l => l.LogWarning(It.IsAny<string>(), new Exception())).Verifiable();
            MockLoggerService.Setup(l => l.LogError(It.IsAny<string>(), new Exception())).Verifiable();
            MockLoggerService.Setup(l => l.LogError(It.IsAny<string>(), It.IsAny<Exception>())).Verifiable();

            MockBusinessLoggerService.Setup(l => l.LogInformation(It.IsAny<string>())).Verifiable();
            MockBusinessLoggerService.Setup(l => l.LogWarning(It.IsAny<string>(), new Exception())).Verifiable();
            MockBusinessLoggerService.Setup(l => l.LogError(It.IsAny<string>(), new Exception())).Verifiable();
            MockBusinessLoggerService.Setup(l => l.LogError(It.IsAny<string>(), It.IsAny<Exception>())).Verifiable();
        }
    }
}