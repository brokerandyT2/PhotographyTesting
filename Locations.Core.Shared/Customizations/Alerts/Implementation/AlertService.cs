using Locations.Core.Shared.Customizations.Alerts.Interfraces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Microsoft.Extensions.Logging;
namespace Locations.Core.Shared.Customizations.Alerts.Implementation
{
    public class AlertService : IAlertService
    {

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

        public Task<bool> ShowConfirmationAsync(string title, string message, string accept, string cancel, bool isLogged)
        {
            return null;
        }

        public Task<bool> ShowConfirmationAsync(string title, string message, string accept, string cancel, bool isLogged, AlertType level)
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
