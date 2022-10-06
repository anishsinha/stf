using NLog;
using System;

namespace SiteFuel.Exchange.Logger
{
    public sealed class LogManager
    {
        private static object currentLock = new object();
        private static LogManager _current = null;

        private ILogger _logger;
        private ILogger _emptylogger;

        private ILogger _infoLogger;
        private ILogger _debugLogger;
        private ILogger _errorLogger;
        private ILogger _traceLogger;

        private LogManager()
        {
            _logger = new SiteFuelLogger();
            _emptylogger = new EmptyLogger();

            _errorLogger = _emptylogger;
            _debugLogger = _emptylogger;
            _infoLogger = _emptylogger;
            _traceLogger = _emptylogger;
        }

        public static LogManager Logger
        {
            get
            {
                if (_current == null)
                {
                    lock (currentLock)
                    {
                        _current = new LogManager();
                        _current.ConfigureLogger();
                    }
                }

                return _current;
            }
        }

        public void ConfigureLogger()
        {
            if (_logger.IsDebugEnabled)
            {
                _debugLogger = _logger;
            }

            if (_logger.IsInfoEnabled)
            {
                _infoLogger = _logger;
            }

            if (_logger.IsErrorEnabled)
            {
                _errorLogger = _logger;
            }

            if (_logger.IsTraceEnabled)
            {
                _traceLogger = _logger;
            }
        }

        public void WriteInfo(string controllerClass, string actionMethod, string message)
        {
            _infoLogger.WriteInfo($"{controllerClass}::{actionMethod} => {message}");
        }
        public void WriteAPIInfo(string userName,string controllerClass, string actionMethod,string requestjson,string responseJson,double TotalMilliseconds,string deviceDetails,DateTime startTime,DateTime endTime)
        {
            LogEventInfo myEvent = new LogEventInfo(LogLevel.Info, "", $"{controllerClass}::{actionMethod}");
            myEvent.Properties.Add("userName", userName);
            myEvent.Properties.Add("requestJson", requestjson);
            myEvent.Properties.Add("responseJson", responseJson);
            myEvent.Properties.Add("TotalMilliseconds", TotalMilliseconds);
            myEvent.Properties.Add("device", deviceDetails);
            myEvent.Properties.Add("startTime", startTime);
            myEvent.Properties.Add("endTime", endTime);
            _logger.WriteInfo(myEvent);
        }
        public void WriteDebug(string controllerClass, string actionMethod, string message)
        {
            _debugLogger.WriteDebug($"{controllerClass}::{actionMethod} => {message}");
        }

        public void WriteError(string controllerClass, string actionMethod, string message)
        {
            _errorLogger.WriteError($"{controllerClass}::{actionMethod} => {message}");
        }

        public void WriteException(string controllerClass, string actionMethod, string message, Exception ex)
        {
            _errorLogger.WriteException($"{controllerClass}::{actionMethod} => {message}", ex);
        }

        public void WriteTrace(string controllerClass, string actionMethod, string message)
        {
            _traceLogger.WriteTrace($"{controllerClass}::{actionMethod} => {message}");
        }
        public void CustomException(string controllerClass, string actionMethod, string message, object obj)
        {
            _errorLogger.CustomException($"{controllerClass}::{actionMethod} => {message}", obj);
        }
    }
}
