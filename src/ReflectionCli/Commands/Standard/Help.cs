using System;
using ReflectionCli.Lib;

namespace ReflectionCli
{
    public class Help : ICommand
    {
        private readonly ILoggingService _loggingService;
        public Help(ILoggingService loggingService)
        {
            _loggingService = loggingService;
        }
        public void Run()
        {
            _loggingService.Log();
            _loggingService.LogResult("     To run a Command simply type the name of the command followed by the name of each parameter and it's value.");
            _loggingService.LogResult("     For example to run a command called \"sample\" you would just type: sample ");
            _loggingService.Log();
            _loggingService.LogResult("     If this command had an input called \"value1\" and you wanted to set that value to \"5\" you would type: ");
            _loggingService.LogResult("     sample -value1 5");
            _loggingService.Log();
            _loggingService.LogResult("     To see a list of commands simply type \"list\" ");
            _loggingService.LogResult("     To see a list of the inputs for a specific command type \"list -name \" followed by the name of the command");
            _loggingService.Log();
            _loggingService.LogResult("     To Input an array or list simply enter values in the list seperated by spaces:");
            _loggingService.LogResult("     samplefunction -inputarray element1 element2 element3");
            _loggingService.Log();
            _loggingService.LogResult("     To Exit, simply type \"exit\" ");
        }
    }
}