using NLog;
using System;

namespace SiteFuel.Exchange.Logger
{
    class EmptyLogger : ILogger
    {
        public void WriteInfo(string message)
        {
            // For empty logger that implements ILogger
        }
        public void WriteInfo(LogEventInfo message)
        {
            // For empty logger that implements ILogger
        }
        public void WriteDebug(string message)
        {
            // For empty logger that implements ILogger
        }

        public void WriteError(string message)
        {
            // For empty logger that implements ILogger
        }

        public void WriteTrace(string message)
        {
            // For empty logger that implements ILogger
        }

        public void WriteException(string message, Exception ex)
        {
            // For empty logger that implements ILogger
        }

        public void CustomException(string message, object obj)
        {
            // For empty logger that implements ILogger
        }

        public bool IsInfoEnabled { get { return false; } }
        public bool IsDebugEnabled { get { return false; } }
        public bool IsErrorEnabled { get { return false; } }
        public bool IsTraceEnabled { get { return false; } }
    }
}
