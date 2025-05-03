using Locations.Core.Shared.Customizations.Alerts.Interfraces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Microsoft.Extensions.Logging;
using Locations.Core.Shared.Customizations.Logging.Interfaces;
namespace Locations.Core.Shared.Customizations.Alerts.Implementation
{
    public class AlertService : IAlertService
    {

        private ILoggerService loggerService;

        public async Task ShowAlertAsync(string title, string message, string cancel)
        {
            var page = Application.Current?.MainPage;
            if (page != null)
            {
                await page.DisplayAlert(title, message, cancel);
            }
        }

        public async Task<bool> ShowConfirmationAsync(string title, string message, string accept, string cancel)
        {
            var page = Application.Current?.MainPage;
            return page != null
                ? await page.DisplayAlert(title, message, accept, cancel)
                : false;
        }
        private bool _isLogged;
        public bool ShowConfirmationAsync(string title, string message, string accept, string cancel, bool isLogged)
        {

            return this.ShowConfirmationAsync(title, message, accept, cancel).Result;
        }

        public bool ShowConfirmationAsync(string title, string message, string accept, string cancel, bool isLogged, AlertType level)
        {
            
            if ((_isLogged))
            {
                LogIt(level, message);
            }
            return this.ShowConfirmationAsync(title, message, accept, cancel, isLogged);
            
        }

        public Task ShowAlertAsync(string title, string message, string cancel, bool logged)
        {
            LogIt(null, message);
            return ShowAlertAsync(title, message, cancel);
        }
        private void LogIt(AlertType? level, string message)
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
        public enum AlertType
        {
            Info,
            Warning,
            Error,
            Success
        }
    }
}
