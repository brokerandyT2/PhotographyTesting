namespace Location.Core.Helpers.AlertService
{
    public class DataErrorEventArgs : EventArgs
    {
       // public ErrorSource Source { get; }
        public string Message { get; }
        public Exception? Exception { get; }
        public object? RelatedEntity { get; }

      /*  public DataErrorEventArgs(ErrorSource source, string message, Exception? ex = null, object? entity = null)
        {
            Source = source;
            Message = message;
            Exception = ex;
            RelatedEntity = entity;
        }*/
    }
}
