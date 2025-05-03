using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Locations.Core.Shared.Customizations.Alerts.Interfraces
{
    public interface IAlertService
    {
        Task ShowAlertAsync(string title, string message, string cancel);
        Task<bool> ShowConfirmationAsync(string title, string message, string accept, string cancel);
        Task<bool> ShowConfirmationAsync(string title, string message, string accept, string cancel, bool isLogged);
        Task<bool> ShowConfirmationAsync(string title, string message, string accept, string cancel, bool isLogged, Locations.Core.Shared.Customizations.Alerts.Implementation.AlertService.AlertType level);
    }

}
