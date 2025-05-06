using Locations.Core.Shared.Customizations.Alerts.Implementation;
using Locations.Core.Shared.Customizations.Alerts.Interfraces;
using Locations.Core.Shared.Customizations.Logging.Implementation;
using Locations.Core.Shared.Customizations.Logging.Interfaces;
using Locations.Core.Shared.ViewModels.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locations.Core.Business.DataAccess
{
    public class ServiceBase<T> where T : IDTOBase
    {
        public static event EventHandler<AlertEventArgs> AlertRaised;
        private IAlertService alertService;
        private static ILoggerService loggerService;
        public static bool IsError { get; set; } = false;
        public ServiceBase() { }

        public ServiceBase(ILoggerService logger, IAlertService alertService)
        {
            ServiceBase<T>.loggerService = logger;
            this.alertService = alertService;
        }
        public static void RaiseAlert(object? sender, AlertEventArgs e)
        {
            AlertRaised?.Invoke(null, e);
            IsError = true;
            loggerService = new LoggerService();
            loggerService.LogError(e.Message);
        }
        public static void RaiseError(Exception ex)
        {
            RaiseAlert(null, new AlertEventArgs("Error", ex.Message, true, AlertService.AlertType.Error));
            IsError = true;
            loggerService.LogError(ex.Message);
        }
        public static void LogInfo(string message)
        {
            LoggerService ls = new LoggerService();
            ls.LogInformation(message);
        }
    }
}
