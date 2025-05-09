using System;

namespace Location.Core.Helpers.AlertService
{
    /// <summary>
    /// Service for displaying alerts to the user
    /// </summary>
    public interface IAlertService
    {
        /// <summary>
        /// Shows an information message
        /// </summary>
        /// <param name="message">The message to show</param>
        void ShowInfo(string message);

        /// <summary>
        /// Shows a warning message
        /// </summary>
        /// <param name="message">The message to show</param>
        void ShowWarning(string message);

        /// <summary>
        /// Shows an error message
        /// </summary>
        /// <param name="message">The message to show</param>
        void ShowError(string message);
    }
}