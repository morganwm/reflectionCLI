using System;
using System.Linq;
using ReflectionCli.Lib;

namespace ReflectionCli
{
    public class ListAssemblies : ICommand
    {
        private readonly ILoggingService _loggingService;
        private readonly IAssemblyService _assemblyService;

        public ListAssemblies(ILoggingService loggingService, IAssemblyService assemblyService)
        {
            _loggingService = loggingService;
            _assemblyService = assemblyService;
        }

        public void Run()
        {
            _assemblyService.Get().ToList().ForEach(x => _loggingService.LogResult(x.GetName().Name));
        }

        public bool ExitVal()
        {
            return false;
        }
    }
}