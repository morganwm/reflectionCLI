using System;Exc
using ReflectionCli.Lib.Enums;

namespace ReflectionCli.Lib
{
    public interface ILoggingService
    {
        void Log();
        void Log(string info);
        void Log(object info);
        void LogInfo(string info);
        void LogInfo(Exception ex);
        void LogDebug(string debug);
		void LogWarning(string warning);
		void LogError(string error);
        void LogError(Exception ex);

        Verbosity GetVerbosity();
        void SetVerbosity(Verbosity verbosity);

        string GetLastTextWrittenInConsole();
    }
}
