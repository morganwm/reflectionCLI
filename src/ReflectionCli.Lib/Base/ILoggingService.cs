using System;
namespace ReflectionCli.Lib
{
    public interface ILoggingService
    {
        void LogInfo(string info);
        void LogDebug(string debug);
		void LogWarning(string warning);
		void LogError(string error);
        void LogError(Exception ex);
    }
}
