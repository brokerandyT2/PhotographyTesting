using Locations.Core.Business.Logging.Interfaces;
using Microsoft.Extensions.Logging;
using Log = Locations.Core.Shared.ViewModels.Log;

namespace Locations.Core.Business.Logging.Implementation
{
    public class LoggerService : ILoggerService
    {
        private LoggerService _logger;

        // Constructor injection for ILogger<T> from DI container
        public LoggerService(ILogger<LoggerService> logger)
        {
           
        }

        public LoggerService()
        {
            if(_logger == null)
            {
                
            }

        }
        public void LogInformation(string message)
        {
            Log l = new Log();
            l.Level = "Information";
            l.Message = message;
            l.Exception = null;// = DateTime.Now;
            //  _logger.LogInformation(message);
           
        }

        public void LogWarning(string message)
        {
           // _logger.LogWarning(message);
        }

        public void LogError(string message, Exception exception = null)
        {
            if(_logger == null)
            {
                //_logger = new LoggerService();
            }
           // _logger.LogError(message,exception);
        }

        public void LogDebug(string message)
        {
           // _logger.LogDebug(message);
        }

        public void LogTrace(string message)
        {
           // _logger.LogTrace(message);
        }
    }
}