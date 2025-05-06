using Locations.Core.Shared.Customizations.Alerts.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Locations.Core.Shared.Customizations.Alerts.Implementation.AlertService;


namespace Locations.Core.Shared.Customizations.Alerts.Interfraces
{
    public interface IAlertService
    {


        event EventHandler<AlertEventArgs> AlertRaised;
        public void ShowAlert(string title, string message, AlertType alertType);

    }

}
