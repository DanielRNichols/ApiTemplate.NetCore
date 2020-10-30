using System;


namespace ApiTemplate.NetCore.Interfaces
{
    public enum LoggerLevel
    {
        Info,
        Warn,
        Error,
        Debug
    }

    public interface ILoggerService
    {
        void LogInfo(string message);
        void LogWarn(string message);
        void LogError(string message);
        void LogDebug(string message);

        void LogMessage(LoggerLevel level, string message);
        void LogServerError(string msg);
        void LogServerError(Exception e);
    }
}

