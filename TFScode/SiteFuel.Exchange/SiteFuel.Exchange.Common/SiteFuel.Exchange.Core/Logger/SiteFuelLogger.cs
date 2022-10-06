using NLog;
using System;

namespace SiteFuel.Exchange.Logger
{
    public class SiteFuelLogger : ILogger
    {
        private readonly NLog.Logger _logger;

        public SiteFuelLogger()
        {
            _logger = NLog.LogManager.GetCurrentClassLogger();
        }

        public void WriteInfo(string message)
        {
            _logger.Info(message);
        }
        public void WriteInfo(LogEventInfo message)
        {
            _logger.Info(message);
        }
        public void WriteDebug(string message)
        {
            _logger.Debug(message);
        }

        public void WriteError(string message)
        {
            _logger.Error(message);
        }

        public void WriteException(string message, Exception ex)
        {
            _logger.Error(ex, message);
        }

        public void CustomException(string message, object obj)
        {
            _logger.Debug(message, obj);
        }
        public void WriteTrace(string message)
        {
            _logger.Trace(message);
        }

        public bool IsInfoEnabled { get { return _logger.IsInfoEnabled; } }
        public bool IsDebugEnabled { get { return _logger.IsDebugEnabled; } }
        public bool IsErrorEnabled { get { return _logger.IsErrorEnabled; } }
        public bool IsTraceEnabled { get { return _logger.IsTraceEnabled; } }
    }
}

