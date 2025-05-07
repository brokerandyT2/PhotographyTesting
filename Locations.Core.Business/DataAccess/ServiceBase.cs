using Locations.Core.Business.Logging.Implementation;
using Locations.Core.Shared;
using Locations.Core.Shared.Customizations.Alerts.Implementation;
using Locations.Core.Shared.StorageSvc;
using Locations.Core.Shared.ViewModels.Interface;

namespace Locations.Core.Business.DataAccess
{
    public class ServiceBase<T> where T : IDTOBase
    {
        public static event EventHandler<AlertEventArgs> AlertRaised;
        public static AlertEventArgs alertArgs;
        public static bool IsError { get; set; } = false;
        public string SB_email { get; private set; }
        public string SB_guid { get; private set; }
        public string KEY { get { return SB_guid + SB_email; } }
        public ServiceBase()
        {
            SB_email = NativeStorageService.GetSetting(MagicStrings.Email);
            SB_guid = NativeStorageService.GetSetting(MagicStrings.UniqueID);
        }


        public static void RaiseAlert(object? sender, AlertEventArgs e)
        {
            AlertRaised?.Invoke(null, e);
            IsError = true;
            alertArgs = new AlertEventArgs(e.Title, e.Message, true);

        }
        public static void RaiseError(Exception ex)
        {
            alertArgs = new AlertEventArgs("Error", ex.Message, true, AlertService.AlertType.Error);
            RaiseAlert(null, alertArgs);
            IsError = true;


        }
        public static void LogInfo(string message)
        {
            LoggerService ls = new LoggerService();
            ls.LogInformation(message);
        }
    }
}
