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

        public static IServiceProvider ServiceProvider;

        public static void Main(string[] args)
        {
            var serviceCollection = new ServiceCollection()
                .AddSingleton<IAssemblyService, AssemblyService>()
                .AddSingleton<ILoggingService, LoggingService>()
                .AddSingleton<IParserService, ParserService>()
                .AddSingleton<IVariableService, VariableService>();

            ServiceProvider = serviceCollection.BuildServiceProvider();

            var parseservice = ServiceProvider.GetService<IParserService>();

            var assemblyservice = ServiceProvider.GetService<IAssemblyService>();

            var loggingservice = ServiceProvider.GetService<ILoggingService>();

            assemblyservice.Add(Assembly.GetEntryAssembly());

            if (args.Length > 0) {
                parseservice.Parse(string.Join(" ", args));
            } else {
                TerminalMode(parseservice, loggingservice);
            }
        }

        public static void TerminalMode(IParserService parseservice, ILoggingService loggingservice)
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
                    loggingservice.LogError(ex);
                }
            }
        }
    }
}
