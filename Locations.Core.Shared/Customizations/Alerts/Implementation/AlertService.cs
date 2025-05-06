using Locations.Core.Shared.Customizations.Alerts.Interfraces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Microsoft.Extensions.Logging;
using Locations.Core.Shared.Customizations.Logging.Interfaces;
using Locations.Core.Shared.Customizations.Logging.Implementation;
namespace Locations.Core.Shared.Customizations.Alerts.Implementation
{
    public class AlertService : IAlertService
    {
        public event EventHandler<AlertEventArgs> AlertRaised;
        private ILoggerService loggerService;
        
        public AlertService()
        {
        }

        public AlertService(ILoggerService logger)
        {
            loggerService = logger;
        }
        
        public void LogIt(AlertType? level, string message)
        {
            switch (level)
            {
                case AlertType.Info:
                    loggerService.LogInformation(message);
                    break;
                case AlertType.Warning:
                    loggerService.LogWarning(message);
                    break;
                case AlertType.Error:
                    loggerService.LogError(message);
                    break;
                case AlertType.Success:
                    loggerService.LogInformation(message);
                    break;
                default:
                    loggerService.LogInformation(message);
                    break;
            }
        }

        public void ShowAlert(string title, string message, AlertType alertType)
        {
            throw new NotImplementedException();
        }

        public enum AlertType
        {
            Info,
            Warning,
            Error,
            Success
        }
    }
}
