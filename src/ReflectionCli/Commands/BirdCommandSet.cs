using System;
using System.Collections.Generic;
using ReflectionCli.Lib;

namespace ReflectionCli.extended
{
    public class BirdCommandSet
    {
        public class Parrot : ICommand
        {
            private readonly ILoggingService _loggingService;
            public Parrot(ILoggingService loggingService)
            {
                _loggingService = loggingService;
            }
            public void Run(List<string> input) 
            {
                input.ForEach(x => _loggingService.LogResult(x));
            }
        }

        public class Parakeet : ICommand
        {
            private readonly ILoggingService _loggingService;
            public Parakeet(ILoggingService loggingService)
            {
                _loggingService = loggingService;
            }
            public void Run(List<string> input)
            {
                input.ForEach(x => _loggingService.LogResult($"  +{x}"));
            }

            public void Run(List<string> input, int number)
            {
                input.ForEach(x => _loggingService.LogResult($"     {number}: {x}"));
            }

            public void Run(List<int> inputints, int number)
            {
                inputints.ForEach(x => _loggingService.LogResult($"     {number}: {x}"));
            }
        }
    }
}