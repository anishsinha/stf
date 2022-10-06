using NLog;
using System;

namespace SiteFuel.Exchange.Logger
{
    public interface ILogger
    {
        void WriteInfo(string message);
        void WriteInfo(LogEventInfo message);
        void WriteDebug(string message);

        void WriteError(string message);

        void WriteException(string message, Exception ex);

        void CustomException(string message, object obj);

        void WriteTrace(string message);

        bool IsInfoEnabled { get; }
        bool IsDebugEnabled { get; }
        bool IsErrorEnabled { get; }
        bool IsTraceEnabled { get; }
    }
}

