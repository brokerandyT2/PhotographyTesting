using CommunityToolkit.Mvvm.ComponentModel;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Locations.Core.Shared.Alerts.Implementation
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
       
        
        public AlertEventArgs()
        {
            if(string.IsNullOrEmpty(Title))
                Title = string.Empty;
            if(string.IsNullOrEmpty(Message))
                Message = string.Empty;

            //IsError = false;
   
        }
        public AlertEventArgs(string title, string message) : this()
        {
            Title = title;
            Message = message;

        }
        public AlertEventArgs(string title, string message, object type) : this(title, message)
        {

           // alertType = type;
            if (_writeToLog)
            {
               // SqliteLoggerService logger = new SqliteLoggerService();
                //alert.LogIt(type, message);
            }

        }
        private bool _writeToLog = false;
       
        public AlertEventArgs(string title, string message, bool writeToLog) : this(title, message)
        {

            this.title = title;
            this.message = message;
            this._writeToLog = writeToLog;
        }
        public AlertEventArgs(string title, string message, bool isError, bool writeToLog)
        {
            Title = title;
            Message = message;
            IsError = isError;
            if (writeToLog)
            {
               
            }
        }
    }
}