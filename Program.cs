using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using System.Reflection;


namespace reflectionCli {

    class Program    {

        public static List<Assembly> activeasm;
        public static Boolean verbose;

        public static void Main(string[] args)  {
            activeasm = new List<Assembly>();
            activeasm.Add(Assembly.GetEntryAssembly());
            verbose = true;
            while(true) {
                Console.WriteLine();
                Console.WriteLine("Enter command (help to display help): ");
                var command = Parser.Parse(Console.ReadLine());
                if (command.ExitVal()) { break; }
            }
        }
    }
}
