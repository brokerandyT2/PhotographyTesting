using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Location.Core.Logging
{
    public interface ILogger
    {
        void LogWarning(string message, Exception ex);
        void LogInfo(string message, Exception ex);
        void LogError(string message, Exception ex);
        void LogWarning(string message);
        void LogInfo(string message);
  
        void LogGenericError(string type, string message, Exception? ex);
    }
}
