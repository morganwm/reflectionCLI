using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using System.Reflection;


namespace reflectionCli {

    class Program    {

        public static Dictionary<Guid, Assembly> activeasm;
        public static Boolean verbose;

        public static void Main(string[] args)  {
            activeasm = new Dictionary<Guid, Assembly>();
            activeasm.Add(Guid.NewGuid(), Assembly.GetEntryAssembly());
            verbose = true;
            while(true) {
                Console.WriteLine();
                Console.WriteLine("Enter command (help to display help):");
                if (Parser.Parse(Console.ReadLine())) { break; }
            }
        }
    }
}
