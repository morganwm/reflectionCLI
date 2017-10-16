using System;
using System.Linq;
using System.Reflection;
using ReflectionCli.Lib;

namespace ReflectionCli
{
    public class List : ICommand
    {
        private readonly ILoggingService _loggingService;
        private readonly IAssemblyService _assemblyService;

        public List(ILoggingService loggingService, IAssemblyService assemblyService)
        {
            _loggingService = loggingService;
            _assemblyService = assemblyService;
        }

        public void Run()
        {
            Console.WriteLine("Valid Commands:");

            _assemblyService.Get().ToList().ForEach(t =>
            {
                Console.WriteLine($"{Environment.NewLine}   - {t.FullName}");

                t.DefinedTypes.Where(u => (
                    // this has to be done this way as the ICommand interface is not object equivalent for runtime loaded assemblies
                    u.ImplementedInterfaces.Where(v => v.Name == nameof(ICommand))
                        .ToList()
                        .Count != 0
                ))
                .ToList()
                .ForEach(v => Console.WriteLine($"       - {v.Name}"));
            });
        }

        public void Run(string name)
        {
            Console.WriteLine($"Valid Commands for {name}:");
            _assemblyService.Get().ToList().ForEach(t =>
            {
                Console.WriteLine($"{Environment.NewLine}   - {t.FullName}");
                t.DefinedTypes.Where(u => (
                    // this has to be done this way as the ICommand interface is not object equivalent for runtime loaded assemblies
                    u.ImplementedInterfaces.Where(v => v.Name == nameof(ICommand))
                        .ToList()
                        .Count != 0
                ))
                .Where(u => u.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase))
                .ToList()
                .ForEach(u =>
                {
                    u.AsType()
                    .GetMethods()
                    .Where(v => v.Name == "Run")
                    .ToList()
                    .ForEach(v =>
                    {
                        Console.WriteLine($"{Environment.NewLine} +       - {name}");

                        v.GetParameters().ToList().ForEach(w =>
                        {
                            Console.WriteLine($"        - {w.Name} ({w.ParameterType.FullName})");
                        });
                    });
                });
            });
        }
    }
}