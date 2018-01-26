using System;
using ReflectionCli.Lib.Enums;

namespace ReflectionCli.Lib
{
    public class LoggingService : ILoggingService
    {
        public Verbosity Verbosity = Verbosity.Debug;
        private readonly ILoggingService _loggingService;

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

        public void LogDebug(Exception ex)
        {
            if (Verbosity <= Verbosity.Debug)
            {
                _textToBeWrittenInConsole = $"[DBG] {ex.ToString()}";
                Console.WriteLine($"[DBG] {ex.ToString()}");
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

        public void Log(string info)
        {
            _textToBeWrittenInConsole = info;
            Console.WriteLine(info);
        }

        public void Log(object info)
        {
            _textToBeWrittenInConsole = info.ToString();
            Console.WriteLine(info);
        }

        public void Log()
        {
            _textToBeWrittenInConsole = Environment.NewLine;
            Console.WriteLine(Environment.NewLine);
        }

        public void LogInfo(string info)
        {
            if (Verbosity <= Verbosity.Info) {
                _textToBeWrittenInConsole = $"[INF] {info}";
                Console.WriteLine($"[INF] {info}");
            }
        }

        public void LogInfo(Exception ex)
        {
            if (Verbosity <= Verbosity.Info)
            {
                _textToBeWrittenInConsole = $"[INF] {ex.GetType().ToString()}";
                Console.WriteLine($"[INF] {ex.GetType().ToString()}");
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
