using Contracts;
using NLog;

namespace LoggerService;

public class LoggerManager : ILoggerManager
{
    private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();

    public LoggerManager()
    {
    }

    /// <summary>
    /// This method used to log the DEBUG logs.
    /// </summary>
    /// <param name="message">A string message to log at Debug level</param>
    public void LogDebug(string message)
    {
        _logger.Debug(message);
    }

    /// <summary>
    /// This method used to log the ERROR logs.
    /// </summary>
    /// <param name="message">A string message to log at Error level</param>
    public void LogError(string message)
    {
        _logger.Error(message);
    }

    /// <summary>
    /// This method used to log the INFO logs.
    /// </summary>
    /// <param name="message">A string message to log at Info level</param>
    public void LogInfo(string message)
    {
        _logger.Info(message);
    }

    /// <summary>
    /// This method used to log the WARN logs.
    /// </summary>
    /// <param name="message">A string message to log at Warn level</param>
    public void LogWarn(string message)
    {
        _logger.Warn(message);
    }
}
