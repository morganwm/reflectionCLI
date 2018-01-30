using System;
using System.Linq;
using System.Reflection;
using ReflectionCli.Lib;
using System.Collections.Generic;

namespace ReflectionCLI.Commands.Standard
{
    public class Variables : ICommand
    {
        private readonly IVariableService _variableService;
        private readonly ILoggingService _loggingService;

        public Variables(ILoggingService loggingService, IVariableService variableService)
        {
            _loggingService = loggingService;
            _variableService = variableService;
        }

        public void Run()
        {
            _loggingService.Log();

            if (_variableService.Get() == null)
            {
                _loggingService.Log("There are no variables saved.");
                return;
            }

            if (_variableService.Get().Count == 0)
            {
                _loggingService.Log("There are no variables saved.");
                return;
            }

            foreach (var variable in _variableService.Get())
            {
                _loggingService.Log($"  {variable.Key}        {variable.Value}  ({variable.Value.GetType()})");
            }
        }

        public void Run(string name, string value)
        {
            _loggingService.Log();
            _variableService.Set(name, value);
            _loggingService.Log($"{name} => {_variableService.Get(name).ToString()}");
        }
    }
}
