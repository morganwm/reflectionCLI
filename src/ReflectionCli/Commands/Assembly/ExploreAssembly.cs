using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using ReflectionCli.Lib;

namespace ReflectionCli
{
    public class ExploreAssembly : ICommand
    {
        private readonly ILoggingService _loggingService;
        private readonly IAssemblyService _assemblyService;

        public ExploreAssembly(ILoggingService loggingService)
        {
            _loggingService = loggingService;
        }

        public void Run(string path)
        {
            try {
                Assembly tempAsm = AssemblyLoadContext.Default.LoadFromStream(new MemoryStream(File.ReadAllBytes(path)));

                var types = tempAsm.GetTypes();

                foreach (var type in types) {
                    _loggingService.Log("Type:  " + type.FullName);

                    type.GetMethods()
                        .ToList()
                        .ForEach(t =>
                        {
                            _loggingService.Log("   Method: " + t.Name);
                        });

                }
            } catch (Exception ex) {
                _loggingService.LogException(ex);
            }

        }
    }
}