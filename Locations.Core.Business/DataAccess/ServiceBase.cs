using Locations.Core.Business.Logging.Implementation;
using Locations.Core.Business.Logging.Interfaces;
using Locations.Core.Shared;
using Locations.Core.Shared.Customizations.Alerts.Implementation;
using Locations.Core.Shared.Customizations.Alerts.Interfraces;
using Locations.Core.Shared.Customizations.Logging.Implementation;
using Locations.Core.Shared.Customizations.Logging.Interfaces;
using Locations.Core.Shared.StorageSvc;
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

        public static bool IsError { get; set; } = false;
        public string SB_email {  get; private set; }
        public string SB_guid {  get; private set; }
        public string KEY { get;{ return guid + eamil; } }
        public ServiceBase() {
            SB_email = NativeStorageService.GetSetting(MagicStrings.Email);
            SB_guid = NativeStorageService.GetSetting(MagicStrings.UniqueID);
        }


        public static void RaiseAlert(object? sender, AlertEventArgs e)
        {
            AlertRaised?.Invoke(null, e);
            IsError = true;

        }
        public static void RaiseError(Exception ex)
        {
            RaiseAlert(null, new AlertEventArgs("Error", ex.Message, true, AlertService.AlertType.Error));
            IsError = true;

        }
        public static void LogInfo(string message)
        {
            LoggerService ls = new LoggerService();
            ls.LogInformation(message);
        }
    }
}
