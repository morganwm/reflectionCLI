using System;
using ReflectionCli.Lib;

namespace ReflectionCli
{
    public class Exit : ICommand
    {
        private readonly ILoggingService _loggingService;

        public Exit(ILoggingService loggingService) 
        {
            _loggingService = loggingService;
        }

        public void Run()
        {
            _loggingService.Log("Shutting down....");

            Program.ShutDown = true;
        }
    }
}