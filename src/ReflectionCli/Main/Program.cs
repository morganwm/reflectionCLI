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
                .AddSingleton<IAssemblyService, AssemblyService>()
                .AddSingleton<ILoggingService, LoggingService>()
<<<<<<< HEAD
                .AddScoped<IVariableService, VariableService>();
            
=======
                .AddSingleton<IVariableService, VariableService>();

>>>>>>> origin/master
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
