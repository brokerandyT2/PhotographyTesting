using CommunityToolkit.Mvvm.ComponentModel;
using Locations.Core.Shared.Customizations.Logging.Implementation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Locations.Core.Shared.Customizations.Alerts.Implementation
{
    public class AlertEventArgs : EventArgs, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        private string title;
        private string message;
        private bool isError = false;

        public string Title
        {
            get { return title; }
            set
            {
                title = value;
                OnPropertyChanged();
            }
        }
        public string Message
        {
            get { return message; }
            set
            {
                message = value;
                OnPropertyChanged();
            }
        }
        public bool IsError
        {
            get { return isError; }
            set
            {
                isError = value;
                OnPropertyChanged();
            }
        }
        SqliteLoggerService logger = new SqliteLoggerService();
        AlertService alert;
        public AlertEventArgs()
        {
            Title = string.Empty;
            Message = string.Empty;
            IsError = false;
            alert = new AlertService(logger);
        }
        public AlertEventArgs(string title, string message) : this()
        {
            Title = title;
            Message = message;

        }
        public AlertEventArgs(string title, string message, AlertService.AlertType type) : this(title, message)
        {

            alertType = type;
            if (_writeToLog)
            {
                SqliteLoggerService logger = new SqliteLoggerService();
                alert.LogIt(type, message);
            }

        }
        private bool _writeToLog = false;
        public AlertEventArgs(string title, string message, bool writeToLog) : this(title, message)
        {
            _writeToLog = writeToLog;

        }
        private AlertService.AlertType alertType;
        public AlertEventArgs(string title, string message, bool writeToLog, AlertService.AlertType type) : this(title, message, writeToLog)
        {
            alertType = type;
        }
        public AlertEventArgs(string title, string message, bool isError, bool writeToLog, AlertService.AlertType type)
        {
            Title = title;
            Message = message;
            IsError = isError;
            if (writeToLog)
            {
                SqliteLoggerService logger = new SqliteLoggerService();
                alert.LogIt(type, message);
            }
        }
    }
}