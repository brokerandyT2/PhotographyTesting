using CommunityToolkit.Mvvm.ComponentModel;
using SQLite;
using System;

namespace Locations.Core.Shared.ViewModels
{
    /// <summary>
    /// This class is only here for SQLite to generate the table
    /// </summary>
    public class Log : ObservableObject
    {
        private int _id;
        private DateTime _timestamp;
        private string _level;
        private string _message;
        private string _exception;

        [PrimaryKey, AutoIncrement]
        public int Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged(nameof(Id));
            }
        }

        public DateTime Timestamp
        {
            get => _timestamp;
            set
            {
                _timestamp = value;
                OnPropertyChanged(nameof(Timestamp));
            }
        }

        public string Level
        {
            get => _level;
            set
            {
                _level = value;
                OnPropertyChanged(nameof(Level));
            }
        }

        public string Message
        {
            get => _message;
            set
            {
                _message = value;
                OnPropertyChanged(nameof(Message));
            }
        }

        public string Exception
        {
            get => _exception;
            set
            {
                _exception = value;
                OnPropertyChanged(nameof(Exception));
            }
        }

        // Default constructor
        public Log()
        {
            Timestamp = DateTime.Now;
            Level = "Info";
            Message = string.Empty;
            Exception = string.Empty;
        }

        // Constructor with initial values
        public Log(string level, string message, string exception = "")
        {
            Timestamp = DateTime.Now;
            Level = level;
            Message = message;
            Exception = exception;
        }
    }
}