using System;
using System.Collections.Generic;
using System.Reflection;
using ReflectionCli.Lib;

using Microsoft.Extensions.DependencyInjection;

namespace ReflectionCli
{
    public class Program
    {
        public static bool ShutDown { get; set; }

        //public static Dictionary<Guid, Assembly> ActiveAsm;

        public static void Main(string[] args)
        {
            var serviceProvider = new ServiceCollection()
                .AddSingleton<IAssemblyService, AssemblyService>()
                .AddSingleton<ILoggingService, LoggingService>()
                .AddSingleton<IParserService, ParserService>()
                .AddSingleton<IVariableService, VariableService>();
            

            var parseservice = serviceProvider.BuildServiceProvider()
                .GetService<IParserService>();

            var assemblyservice = serviceProvider.BuildServiceProvider()
                .GetService<IAssemblyService>();

            assemblyservice.Add(Assembly.GetEntryAssembly());

            //ActiveAsm = new Dictionary<Guid, Assembly>
            //{
            //    { Guid.NewGuid(), Assembly.GetEntryAssembly() },
            //};

            if (args.Length > 0) {
                parseservice.Parse(string.Join(" ", args));
            } else {
                TerminalMode(parseservice);
            }
        }

        public static void TerminalMode(IParserService parseservice)
        {
            Console.WriteLine("Enter command (help to display help):");
            Console.WriteLine();

            while (!ShutDown) {
                try {
                    Console.WriteLine();
                    parseservice.Parse(Console.ReadLine());
                    Console.WriteLine();
                } catch (Exception ex) {
                    Console.WriteLine();
                    Console.WriteLine("A fatal exception occured");
                }
            }
        }
    }
}
