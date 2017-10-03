using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime;
using System.Runtime.Loader;

namespace reflectionCli {

    public class nullCommand : ICommand    {
        public bool ExitVal()        {
            return false;
        }
    }

    public class error : ICommand    {

        public error(string error)        {
            Console.WriteLine(error);
        }

        public error() : this("Generic Error")  {}

        public bool ExitVal()        {
            return false;
        }
    }

    public class help : ICommand    {

        public help() {
            Console.WriteLine("Valid Commands:");
            Program.activeasm.ForEach(x => {
                Console.WriteLine(Environment.NewLine + "   - " + x.FullName);
                x.DefinedTypes.Where(z => (
                    //this has to be done this way as the ICommand interface is not object equivalent for runtime loaded assemblies
                    z.ImplementedInterfaces.Where(a => (a.Name == "ICommand")).ToList().Count != 0
                ))
                .ToList()
                .ForEach(y => Console.WriteLine("       - " + y.Name));
            });
        }
        public bool ExitVal()   {
            return false;
        }
    }

    public class exit : ICommand    {
        public bool ExitVal()   {
            return true;
        }
    }
}