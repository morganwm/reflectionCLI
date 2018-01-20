using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using ReflectionCli.Lib;

namespace ReflectionCli
{
    public class LoadAssembly : ICommand
    {
        private readonly ILoggingService _loggingService;
        private readonly IAssemblyService _assemblyService;

        public LoadAssembly(ILoggingService loggingService, IAssemblyService assemblyService)
        {
            _loggingService = loggingService;
            _assemblyService = assemblyService;
        }

        public void Run(string path)
        {
            Assembly tempAsm = AssemblyLoadContext.Default.LoadFromStream(new MemoryStream(File.ReadAllBytes(path)));

            var tempicommand = tempAsm.GetTypes()
                .Where(x => x.Name == "ICommand")
                .ToList();

            if (tempicommand.Count == 0)
            {
                throw new Exception("Unable to find ICommand");
            }

            if (tempicommand.Count > 1)
            {
                throw new Exception("Multiple ICommands Found");
            }

            _assemblyService.Add(tempAsm);
        }
    }
}