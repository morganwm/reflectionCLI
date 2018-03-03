using System;
using System.Linq;
using System.Reflection;
using ReflectionCli.Lib;
using ReflectionCli.Lib.Enums;

namespace ReflectionCli
{
    public class Verbosity : ICommand
    {
        private readonly ILoggingService _loggingService;

        public Verbosity(ILoggingService loggingService)
        {
            _loggingService = loggingService;
        }

        public void Run()
        {
            _loggingService.Log();
            _loggingService.LogResult(_loggingService.GetVerbosity().ToString());
        }

        public void Run(string verbosity)
        {
            Lib.Enums.Verbosity setting;
            if (!Enum.TryParse<Lib.Enums.Verbosity>(verbosity, true, out setting))
            {
                _loggingService.Log("Failed To Parse Input value into verbosity setting. The Settings are:");
                foreach (var val in Enum.GetValues(typeof(Lib.Enums.Verbosity)))
                {
                    _loggingService.Log($" {val.ToString()}");
                }
                return;
            }

            _loggingService.SetVerbosity(setting);

            _loggingService.Log();
            _loggingService.Log($"Verbosity set to {_loggingService.GetVerbosity().ToString()}");
        }
    }
}