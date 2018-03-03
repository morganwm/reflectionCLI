using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReflectionCli.Lib;
using ReflectionCli.Lib.Enums;
using ReflectionCli.Lib.Models;

namespace ReflectionCli.Test.HelperServices
{
    public class FakeLoggingService : ILoggingService
    {
        public List<Record> Records = new List<Record>();

        public List<string> Input = new List<string>();
        public int InputCallCounter = 0;

        public Lib.Enums.Verbosity Verbosity = Lib.Enums.Verbosity.Debug;

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
            Records.Add(new Record(info));
        }

        public void Log(object info)
        {
            Log(info.ToString());
        }
        public void LogResult(string info)
        {
            Records.Add(new Record(info, RecordType.Result));
            Log(info);
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
            Records.Add(new Record(info, RecordType.Input));
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
            return Records
                .Where(t => t.RecordType == RecordType.Output)
                .OrderBy(t => t.Written)
                .LastOrDefault()
                .Message;
        }
    }
}
