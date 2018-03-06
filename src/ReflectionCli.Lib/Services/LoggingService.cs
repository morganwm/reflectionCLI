using System;
using System.Collections.Generic;
using System.Linq;
using ReflectionCli.Lib.Enums;
using ReflectionCli.Lib.Models;

namespace ReflectionCli.Lib
{
    public class LoggingService : ILoggingService
    {
        public List<Record> Records = new List<Record>();

        public Verbosity Verbosity = Verbosity.Debug;

        public Verbosity GetVerbosity()
        {
            return Verbosity;
        }

        public void LogDebug(string debug)
        {
            if (Verbosity >= Verbosity.Debug) {
                Log($"[DBG] {debug}", RecordType.Error);
            }
        }

        public void LogError(string error)
        {
            if (Verbosity >= Verbosity.Error) {
                Log($"[ERR] {error}", RecordType.Error);
            }
        }

        public void Log(string info)
        {
            Records.Add(new Record(info));
            Console.WriteLine(info);
        }

        public void Log(object info)
        {
            Records.Add(new Record(info.ToString()));
            Console.WriteLine(info);
        }

        public void Log(string info, RecordType recordType)
        {
            Records.Add(new Record(info, recordType));
            Console.WriteLine(info);
        }

        public void Log(object info, RecordType recordType)
        {
            Records.Add(new Record(info.ToString(), recordType));
            Console.WriteLine(info);
        }

        public void LogResult(string info)
        {
            Log(info, RecordType.Result);
        }
        public void LogResult(object info)
        {
            Log(info, RecordType.Result);
        }

        public void Log()
        {
            Log(Environment.NewLine);
        }

        public void LogInfo(string info)
        {
            if (Verbosity >= Verbosity.Info) {
                Log($"[INF] {info}");
            }
        }

        public void LogWarning(string warning)
        {
            if (Verbosity >= Verbosity.Warning) {
                Log($"[WRN] {warning}");
            }
        }

        public void LogException(Exception ex)
        {
            if (Verbosity >= Verbosity.Info)
            {
                Log();
                Log($"[INF] {ex.Message}", RecordType.Error);
            }

            if (Verbosity >= Verbosity.Debug)
            {
                Log();
                Log($"[DBG] {ex.ToString()}", RecordType.Error);
            }

            if (ex.InnerException !=  null)
            {
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
            return Internal(Console.ReadLine());
        }

        public void SetVerbosity(Verbosity verbosity)
        {
            Verbosity = verbosity;
        }

        public string GetLastTextWrittenInConsole()
        {
            return Records
                .Where(t => t.RecordType == RecordType.Output)
                .OrderBy(t => t.Written)
                .LastOrDefault()?
                .Message;
        }
    }
}
