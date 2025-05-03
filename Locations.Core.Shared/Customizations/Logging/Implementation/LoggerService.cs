using Locations.Core.Shared.Customizations.Logging.Interfaces;
using Microsoft.Extensions.Logging;

using Serilog;

namespace Locations.Core.Shared.Customizations.Logging.Implementation
{
    public class LoggerService : ILoggerService
    {
        private readonly ILogger<LoggerService> _logger;

        // Constructor injection for ILogger<T> from DI container
        public LoggerService(ILogger<LoggerService> logger)
        {
            _logger = logger;
        }

        public void LogInformation(string message)
        {
            _logger.LogInformation(message);
        }

        public void LogWarning(string message)
        {
            _logger.LogWarning(message);
        }

        public void LogError(string message, Exception exception = null)
        {
            _logger.LogError(exception, message);
        }

        public void LogDebug(string message)
        {
            _logger.LogDebug(message);
        }

        public void LogTrace(string message)
        {
            _logger.LogTrace(message);
        }
    }
}