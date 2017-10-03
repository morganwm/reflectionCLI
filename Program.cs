using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime;
using System.Runtime.Loader;

namespace reflectionCli {

    class Program    {

        public static List<Assembly> activeasm;

        public static void Main(string[] args)  {
            var exit = false;
            activeasm = new List<Assembly>();
            activeasm.Add(Assembly.GetEntryAssembly());
            while(exit == false)    {
                Console.WriteLine();
                Console.WriteLine("Enter command (help to display help): ");
                var command = Parser.Parse(Console.ReadLine());
                exit = command.ExitVal();
            }
        }
    }

}
