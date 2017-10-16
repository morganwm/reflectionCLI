using System;
using ReflectionCli.Lib.Enums;

namespace ReflectionCli.Lib
{
    public class LoggingService : ILoggingService
    {
        public Verbosity Verbosity = Verbosity.Info;

        public void LogDebug(string debug)
        {
            if (Verbosity >= Verbosity.Debug) {
				Console.WriteLine($"[DBG] {debug}");
			}
        }

        public void LogError(string error)
		{
            if (Verbosity >= Verbosity.Error) {
                Console.WriteLine($"[ERR] {error}");
            }
        }

        public void LogError(Exception ex)
		{
            if (Verbosity >= Verbosity.Error) {
                Console.WriteLine($"[ERR] {ex.ToString()}");
            }
        }

        public void LogInfo(string info)
		{
            if (Verbosity >= Verbosity.Info) {
                Console.WriteLine($"[INF] {info}");
            }
        }

        public void LogWarning(string warning)
		{
            if (Verbosity >= Verbosity.Warning) {
                Console.WriteLine($"[WRN] {warning}");
            }
        }
    }
}
