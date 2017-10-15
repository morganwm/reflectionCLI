using System;
using System.Collections.Generic;
using System.Reflection;
using ReflectionCli.Lib;

using Microsoft.Extensions.DependencyInjection;

namespace ReflectionCli
{
    public class Program
    {
        public static Dictionary<Guid, Assembly> ActiveAsm;
        public static bool Verbose;

        public static void Main(string[] args)
        {
            var serviceProvider = new ServiceCollection()
                .AddScoped<IAssemblyService, AssemblyService>()
                .AddScoped<IVariableService, VariableService>();

            ActiveAsm = new Dictionary<Guid, Assembly>
            {
                { Guid.NewGuid(), Assembly.GetEntryAssembly() },
            };
            Verbose = true;

            while (true) {
                Console.WriteLine();
                Console.WriteLine("Enter command (help to display help):");

                if (Parser.Parse(Console.ReadLine())) {
                    break;
                }
            }
        }
    }
}
