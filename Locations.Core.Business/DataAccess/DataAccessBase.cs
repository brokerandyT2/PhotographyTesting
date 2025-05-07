using Locations.Core.Business.Logging.Implementation;
using Locations.Core.Business.StorageSvc;
using Locations.Core.Shared;
using Locations.Core.Shared.Customizations.Alerts.Implementation;
using Locations.Core.Shared.Customizations.Alerts.Interfraces;
using iLog = Locations.Core.Business.Logging.Interfaces;

namespace Locations.Core.Business.DataAccess
{
    public class DataAccessBase
    {
        public static event EventHandler<AlertEventArgs> AlertRaised;
        AlertEventArgs alertEventArgs;
        private IAlertService alertService;
        public static bool IsError { get; set; } = false;

        public object Connection { get; set; }
        public string KEY
        {
            get { return guid + email; }
        }
        private string email;
        private string guid;
        IAlertService _alert;
        static iLog.ILoggerService _loggingService = new SqliteLoggerService();
        AlertService _alertService;
        private static SqliteLoggerService _logger = new SqliteLoggerService();
        public DataAccessBase()
        {
            _alertService = new AlertService();
            _loggingService = new SqliteLoggerService();
            AlertRaised += OnAlertRaised;
            getKey();
        }

        private void getKey()
        {

            var email = NativeStorageService.GetSetting(MagicStrings.Email);
            var guid = NativeStorageService.GetSetting(MagicStrings.UniqueID);
            Connection = EncryptedSQLite.DataEncrypted.GetConnection(KEY);
        }

        public DataAccessBase(IAlertService alert, iLog.ILoggerService logger) : this()
        {

            Type alertType = alert.GetType();
            Type loggerType = logger.GetType();


            if ((alertType.IsClass && !alertType.IsInterface) && (loggerType.IsClass && !loggerType.IsInterface))
            {
                _alert = alert;
                _loggingService = logger;
            }
            else
            {
                _alert = _alertService;
                _loggingService = _logger;
            }
            _alert = alert;

        }

        public static void LogInfo(string message)
        {
            LoggerService ls = new LoggerService();
            ls.LogInformation(message);
        }

        private void OnAlertRaised(object? sender, AlertEventArgs e)
        {
            alertEventArgs = e;
            AlertRaised?.Invoke(this, e);
            IsError = true;

        }

        protected virtual void OnAlertRaised(string title, string message)
        {
            alertEventArgs = new AlertEventArgs
            {
                Title = title,
                Message = message,
                IsError = true
            };
            AlertRaised?.Invoke(this, alertEventArgs);
            IsError = true;
        }
        public static void RaiseAlert(object? sender, AlertEventArgs e)
        {
            AlertRaised?.Invoke(null, e);
            IsError = true;

            _logger.LogError(e.Message);
        }
        public static void RaiseError(Exception ex)
        {
            AlertRaised?.Invoke(null, new AlertEventArgs("Error", ex.Message, true, AlertService.AlertType.Error));
            IsError = true;
            _logger.LogError(ex.Message);
        }

    }
}
