using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Location.Core.Helpers.AlertService
{
    public class EventAlertService : IAlertService
    {
        // Events that UI layers can subscribe to
        public static event EventHandler<AlertEventArgs> AlertRequested;
        public static event EventHandler<ConfirmationEventArgs> ConfirmationRequested;
        public static event EventHandler<ToastEventArgs> ToastRequested;

        /// <summary>
        /// Displays an informational alert by raising an event
        /// </summary>
        public Task DisplayAlert(string title, string message, string buttonText)
        {
            var tcs = new TaskCompletionSource();

            AlertRequested?.Invoke(this, new AlertEventArgs(
                title, message, buttonText, AlertType.Information));

            // Since we can't await the UI response directly with events,
            // we can consider the task complete when the event is raised
            tcs.SetResult();
            return tcs.Task;
        }

        /// <summary>
        /// Displays a confirmation alert by raising an event
        /// </summary>
        public Task<bool> DisplayConfirmation(string title, string message,
                                             string acceptButtonText, string cancelButtonText)
        {
            var tcs = new TaskCompletionSource<bool>();

            ConfirmationRequested?.Invoke(this, new ConfirmationEventArgs(
                title, message, acceptButtonText, cancelButtonText,
                result => tcs.SetResult(result)));

            return tcs.Task;
        }

        /// <summary>
        /// Displays an error alert by raising an event
        /// </summary>
        public Task DisplayError(string title, string message, string buttonText)
        {
            var tcs = new TaskCompletionSource();

            AlertRequested?.Invoke(this, new AlertEventArgs(
                title, message, buttonText, AlertType.Error));

            tcs.SetResult();
            return tcs.Task;
        }

        /// <summary>
        /// Displays a warning alert by raising an event
        /// </summary>
        public Task DisplayWarning(string title, string message, string buttonText)
        {
            var tcs = new TaskCompletionSource();

            AlertRequested?.Invoke(this, new AlertEventArgs(
                title, message, buttonText, AlertType.Warning));

            tcs.SetResult();
            return tcs.Task;
        }

        /// <summary>
        /// Shows a toast notification by raising an event
        /// </summary>
        public Task DisplayToast(string message, int durationMilliseconds = 3000)
        {
            var tcs = new TaskCompletionSource();

            ToastRequested?.Invoke(this, new ToastEventArgs(
                message, durationMilliseconds));

            tcs.SetResult();
            return tcs.Task;
        }
    }
}
