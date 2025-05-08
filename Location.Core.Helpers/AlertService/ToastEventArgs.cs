using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Location.Core.Helpers.AlertService
{
    public class ToastEventArgs : EventArgs
    {
        public string Message { get; }
        public int DurationMilliseconds { get; }

        public ToastEventArgs(string message, int durationMilliseconds)
        {
            Message = message;
            DurationMilliseconds = durationMilliseconds;
        }
    }
}
