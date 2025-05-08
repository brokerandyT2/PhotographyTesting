using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Location.Core.GeneralizedHelpers.AlertService
{
    public class ConfirmationEventArgs : EventArgs
    {
        public string Title { get; }
        public string Message { get; }
        public string AcceptButtonText { get; }
        public string CancelButtonText { get; }
        public Action<bool> Callback { get; }

        public ConfirmationEventArgs(string title, string message, string acceptButtonText,
                                    string cancelButtonText, Action<bool> callback)
        {
            Title = title;
            Message = message;
            AcceptButtonText = acceptButtonText;
            CancelButtonText = cancelButtonText;
            Callback = callback;
        }
    }
}
