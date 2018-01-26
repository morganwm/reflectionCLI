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
            loggingservice.Log("Enter command (help to display help):" + Environment.NewLine);

            while (!ShutDown) {
                try {
                    loggingservice.Log();
                    parseservice.Parse(Console.ReadLine());
                    loggingservice.Log();
                } catch (Exception ex) {
                    loggingservice.Log();
                    loggingservice.LogError(ex);
                }
            }
        }
    }
}
