using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Location.Core.Helpers.AlertService
{
    public class AlertEventArgs : EventArgs
    {
        public string Title { get; }
        public string Message { get; }
        public string ButtonText { get; }
        public AlertType AlertType { get; }

        public AlertEventArgs(string title, string message, string buttonText, AlertType alertType)
        {
            Title = title;
            Message = message;
            ButtonText = buttonText;
            AlertType = alertType;
        }
        public AlertEventArgs(string message) { Message = message; }
    }
}
