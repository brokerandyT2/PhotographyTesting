using System;

namespace Locations.Core.Business.DataAccess.Events
{
    /// <summary>
    /// Base event arguments for service events
    /// </summary>
    public class ServiceEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the timestamp when this event occurred
        /// </summary>
        public DateTime Timestamp { get; }

        /// <summary>
        /// Gets the message associated with this event
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// Creates a new service event with a message
        /// </summary>
        /// <param name="message">The event message</param>
        public ServiceEventArgs(string message)
        {
            Timestamp = DateTime.Now;
            Message = message ?? string.Empty;
        }
    }
}