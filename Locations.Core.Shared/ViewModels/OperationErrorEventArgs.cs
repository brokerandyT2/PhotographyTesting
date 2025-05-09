using System;

namespace Locations.Core.Shared.ViewModels
{
    public enum OperationErrorSource
    {
        Unknown,
        Network,
        Database,
        LocationService,
        WeatherService,
        MediaService,
        GeolocationService,
        ValidationError,
        InvalidArgument,
        Authentication,
        Authorization
    }

    public class OperationErrorEventArgs : EventArgs
    {
        public OperationErrorSource Source { get; }
        public string Message { get; }
        public Exception? Exception { get; }

        public OperationErrorEventArgs(
            OperationErrorSource source,
            string message,
            Exception? exception = null)
        {
            Source = source;
            Message = message;
            Exception = exception;
        }
    }
}