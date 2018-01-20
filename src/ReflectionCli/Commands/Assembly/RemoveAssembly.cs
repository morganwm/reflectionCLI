using System;
using System.Linq;
using System.Reflection;
using ReflectionCli.Lib;

namespace ReflectionCli
{
    public class RemoveAssembly : ICommand
    {
        private readonly ILoggingService _loggingService;
        private readonly IAssemblyService _assemblyService;

        public RemoveAssembly(ILoggingService loggingService, IAssemblyService assemblyService)
        {
            _loggingService = loggingService;
            _assemblyService = assemblyService;
        }

        public void Run(string name)
        {
            var tempAsmEntries = _assemblyService.Get().Where(t => t.GetName().Name == name);

            if (tempAsmEntries.Count() == 0) {
                throw new Exception($"Unable to find Assembly {name}");
            }

            if (tempAsmEntries.Count() > 1) {
                throw new Exception($"Multiple Assemblies Found with the name: {name}");
            }

            if (tempAsmEntries.ToList()[0] == Assembly.GetEntryAssembly()) {
                throw new Exception($"Cannot remove assembly {Assembly.GetEntryAssembly().GetName().Name} as this is the Entry Assembly");
            }

            // Program.ActiveAsm.Remove(tempAsmEntries.ToList()[0].Key);
            var asmlist = _assemblyService.Get();
            asmlist.RemoveAll(t => t == tempAsmEntries.ToList()[0]);
            _assemblyService.Set(asmlist);
        }

        public bool ExitVal()
        {
            return false;
        }
    }
}