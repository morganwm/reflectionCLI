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

        public static Dictionary<Guid, Assembly> ActiveAsm;

        public static void Main(string[] args)
        {
            var serviceProvider = new ServiceCollection()
                .AddScoped<IAssemblyService, AssemblyService>()
                .AddSingleton<ILoggingService, LoggingService>()
                .AddScoped<IVariableService, VariableService>();

            ActiveAsm = new Dictionary<Guid, Assembly>
            {
                { Guid.NewGuid(), Assembly.GetEntryAssembly() },
            };

            if (args.Length > 0) {
                Parser.Parse(string.Join(" ", args));
            } else {
                TerminalMode();
            }
        }

        public static void TerminalMode()
        {
            Console.WriteLine("Enter command (help to display help):");
            Console.WriteLine();

            while (!ShutDown) {
                try {
                    Parser.Parse(Console.ReadLine());

                    Console.WriteLine();
                } catch {
                    Console.WriteLine("A fatal exception occured");
                }
            }
        }
    }
}
