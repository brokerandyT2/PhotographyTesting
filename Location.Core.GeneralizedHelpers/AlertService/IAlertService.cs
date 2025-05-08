using System;
using System.Threading.Tasks;

namespace Location.Core.GeneralizedHelpers.AlertService
{
    /// <summary>
    /// Interface for displaying alerts and user notifications
    /// </summary>
    public interface IAlertService
    {
        /// <summary>
        /// Displays an informational alert to the user
        /// </summary>
        /// <param name="title">The alert title</param>
        /// <param name="message">The alert message</param>
        /// <param name="buttonText">The text for the action button (e.g., "OK")</param>
        /// <returns>Task representing the asynchronous operation</returns>
        Task DisplayAlert(string title, string message, string buttonText);

        /// <summary>
        /// Displays a confirmation alert with accept and cancel options
        /// </summary>
        /// <param name="title">The alert title</param>
        /// <param name="message">The alert message</param>
        /// <param name="acceptButtonText">The text for the accept button (e.g., "Yes")</param>
        /// <param name="cancelButtonText">The text for the cancel button (e.g., "No")</param>
        /// <returns>True if user accepted, false if canceled</returns>
        Task<bool> DisplayConfirmation(string title, string message, string acceptButtonText, string cancelButtonText);

        /// <summary>
        /// Displays an error alert to the user
        /// </summary>
        /// <param name="title">The error title</param>
        /// <param name="message">The error message</param>
        /// <param name="buttonText">The text for the action button (e.g., "OK")</param>
        /// <returns>Task representing the asynchronous operation</returns>
        Task DisplayError(string title, string message, string buttonText);

        /// <summary>
        /// Displays a warning alert to the user
        /// </summary>
        /// <param name="title">The warning title</param>
        /// <param name="message">The warning message</param>
        /// <param name="buttonText">The text for the action button (e.g., "OK")</param>
        /// <returns>Task representing the asynchronous operation</returns>
        Task DisplayWarning(string title, string message, string buttonText);

        /// <summary>
        /// Shows a toast notification that automatically disappears after a set time
        /// </summary>
        /// <param name="message">The toast message</param>
        /// <param name="durationMilliseconds">Duration in milliseconds to display the toast</param>
        /// <returns>Task representing the asynchronous operation</returns>
        Task DisplayToast(string message, int durationMilliseconds = 3000);
    }
}