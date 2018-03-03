using System;
using System.Collections.Generic;
using System.Text;
using ReflectionCli.Lib;
using ReflectionCli.Lib.Enums;

namespace ReflectionCli.Test.HelperServices
{
    public class FakeLoggingService : ILoggingService
    {
        public List<string> Output = new List<string>();
        public List<string> Results = new List<string>();
        public List<string> Input = new List<string>();

        public int InputCallCounter = 0;

        public Lib.Enums.Verbosity Verbosity = Lib.Enums.Verbosity.Debug;
        private string _textToBeWrittenInConsole = string.Empty;
        private string _textReadInFromConsole = string.Empty;

        public Lib.Enums.Verbosity GetVerbosity()
        {
            return Verbosity;
        }

        public void LogDebug(string debug)
        {
            if (Verbosity >= Lib.Enums.Verbosity.Debug) {
                Log($"[DBG] {debug}");
            }
        }

        public void LogError(string error)
        {
            if (Verbosity >= Lib.Enums.Verbosity.Error) {
                Log($"[ERR] {error}");
            }
        }

        public void Log(string info)
        {
            _textToBeWrittenInConsole = info;
            Output.Add(info);
        }

        public void Log(object info)
        {
            Log(info.ToString());
        }
        public void LogResult(string info)
        {
            Log(info);
            Results.Add(info);
        }

        public void LogResult(object info)
        {
            LogResult(info.ToString());
        }

        public void Log()
        {
            Log(Environment.NewLine);
        }

        public void LogInfo(string info)
        {
            if (Verbosity >= Lib.Enums.Verbosity.Info) {
                Log($"[INF] {info}");
            }
        }

        public void LogWarning(string warning)
        {
            if (Verbosity >= Lib.Enums.Verbosity.Warning) {
                Log($"[WRN] {warning}");
            }
        }

        public void LogException(Exception ex)
        {
            if (Verbosity >= Lib.Enums.Verbosity.Info) {
                Log();
                Log($"[INF] {ex.Message}");
            }

            if (Verbosity >= Lib.Enums.Verbosity.Debug) {
                Log();
                Log($"[DBG] {ex.ToString()}");
            }

            if (ex.InnerException != null) {
                Log();
                Log("Inner:");
                LogException(ex.InnerException);
            }
        }

        public string Internal(string info)
        {
            _textReadInFromConsole = info;
            return info;
        }

        public string ReadLineFromConsole()
        {
            string stringToReturn = Input[InputCallCounter];
            InputCallCounter++;
            return stringToReturn;
        }

        public void SetVerbosity(Lib.Enums.Verbosity verbosity)
        {
            Verbosity = verbosity;
        }

        public string GetLastTextWrittenInConsole()
        {
            return _textToBeWrittenInConsole;
        }
    }
}
