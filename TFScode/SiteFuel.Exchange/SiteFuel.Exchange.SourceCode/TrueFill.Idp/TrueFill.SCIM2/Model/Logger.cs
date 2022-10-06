namespace TrueFill.SCIM2.Model
{
    public static class Logger
    {
        private static string _tempLog = string.Empty;

        public static void ClearLog() { _tempLog = string.Empty; }

        public static string GetLog() { return _tempLog; }

        public static void LogException(string message)
        {
            _tempLog = string.Format("{0}{1}", message, _tempLog);
        }
    }
}