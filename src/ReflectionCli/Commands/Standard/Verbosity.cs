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
            Console.WriteLine();
            Console.WriteLine(_loggingService.GetVerbosity().ToString());
        }

        public void Run(string verbosity)
        {
            Lib.Enums.Verbosity setting;
            if (!Enum.TryParse<Lib.Enums.Verbosity>(verbosity, true, out setting))
            {
                Console.WriteLine("Failed To Parse Input value into verbosity setting. The Settings are:");
                foreach (var val in Enum.GetValues(typeof(Lib.Enums.Verbosity)))
                {
                    Console.WriteLine($" {val.ToString()}");
                }
                return;
            }

            _loggingService.SetVerbosity(setting);

            Console.WriteLine();
            Console.WriteLine($"Verbosity set to {_loggingService.GetVerbosity().ToString()}");
        }
    }
}