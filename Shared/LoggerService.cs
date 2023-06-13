using NLog;

namespace Shared
{
    public class LoggerService : ILoggerManager
    {
        private static ILogger logger = LogManager.GetCurrentClassLogger();
        public void LogDebug(string message)
        {
            logger.Debug(message);
        }

        public void LogError(string message, string stackTrace)
        {
            var loggedMessage = $"{message}.\nStack Trace: {stackTrace}";
            logger.Error(loggedMessage);
        }

        public void LogInfo(string message)
        {
            logger.Info(message);
        }

        public void LogWarn(string message)
        {
            logger.Warn(message);
        }
    }
}
