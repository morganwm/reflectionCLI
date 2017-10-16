using System;
using ReflectionCli.Lib.Enums;

namespace ReflectionCli.Lib
{
    public class LoggingService : ILoggingService
    {
        public Verbosity Verbosity = Verbosity.Info;

        private string _textToBeWrittenInConsole = string.Empty;

        public Verbosity GetVerbosity()
        {
            return Verbosity;
        }

        public void LogDebug(string debug)
        {
            if (Verbosity <= Verbosity.Debug) {
                _textToBeWrittenInConsole = $"[DBG] {debug}";
                Console.WriteLine($"[DBG] {debug}");
            }
        }

        public void LogError(string error)
        {
            if (Verbosity <= Verbosity.Error) {
                _textToBeWrittenInConsole = $"[ERR] {error}";
                Console.WriteLine($"[ERR] {error}");
            }
        }

        public void LogError(Exception ex)
        {
            if (Verbosity <= Verbosity.Error) {
                _textToBeWrittenInConsole = $"[ERR] {ex.Message}";
                Console.WriteLine($"[ERR] {ex.Message}");
            }
        }

        public void LogInfo(string info)
        {
            if (Verbosity <= Verbosity.Info) {
                _textToBeWrittenInConsole = $"[INF] {info}";
                Console.WriteLine($"[INF] {info}");
            }
        }

        public void LogWarning(string warning)
        {
            if (Verbosity <= Verbosity.Warning) {
                _textToBeWrittenInConsole = $"[WRN] {warning}";
                Console.WriteLine($"[WRN] {warning}");
            }
        }

        public void SetVerbosity(Verbosity verbosity)
        {
            Verbosity = verbosity;
        }

        public string GetLastTextWrittenInConsole()
        {
            return _textToBeWrittenInConsole;
        }
    }
}
