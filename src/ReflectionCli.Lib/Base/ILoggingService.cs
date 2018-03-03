using System;
using ReflectionCli.Lib.Enums;

namespace ReflectionCli.Lib
{
    public interface ILoggingService
    {
        void Log();
        void Log(string info);
        void Log(object info);
        void LogResult(string info);
        void LogResult(object info);
        void LogInfo(string info);
        void LogDebug(string debug);
		void LogWarning(string warning);
		void LogError(string error);
        void LogException(Exception ex);
        string Internal(string info);
        string ReadLineFromConsole();

        Verbosity GetVerbosity();
        void SetVerbosity(Verbosity verbosity);

        string GetLastTextWrittenInConsole();
    }
}
