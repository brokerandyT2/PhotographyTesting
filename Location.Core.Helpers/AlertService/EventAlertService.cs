using System;

namespace Location.Core.Helpers.AlertService
{
    /// <summary>
    /// Alert service implementation that raises events
    /// </summary>
    public class EventAlertService : IAlertService
    {
        /// <summary>
        /// Event raised when an info message is shown
        /// </summary>
        public event EventHandler<AlertEventArgs> InfoShown;

        /// <summary>
        /// Event raised when a warning message is shown
        /// </summary>
        public event EventHandler<AlertEventArgs> WarningShown;

        /// <summary>
        /// Event raised when an error message is shown
        /// </summary>
        public event EventHandler<AlertEventArgs> ErrorShown;

        /// <summary>
        /// Shows an information message
        /// </summary>
        public void ShowInfo(string message)
        {
            InfoShown?.Invoke(this, new AlertEventArgs(message));
        }

        /// <summary>
        /// Shows a warning message
        /// </summary>
        public void ShowWarning(string message)
        {
            WarningShown?.Invoke(this, new AlertEventArgs(message));
        }

        /// <summary>
        /// Shows an error message
        /// </summary>
        public void ShowError(string message)
        {
            ErrorShown?.Invoke(this, new AlertEventArgs(message));
        }
    }

    /// <summary>
    /// Event arguments for alerts
    /// </summary>
    
}