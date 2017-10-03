using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime;
using System.Runtime.Loader;

namespace reflectionCli {
    public interface ICommand    {
        bool ExitVal();
    }

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
            Assembly.GetEntryAssembly().DefinedTypes
                    .Where(x => x.ImplementedInterfaces.Contains(typeof(ICommand)))
                    .ToList()
                    .ForEach(x => Console.WriteLine("   - " + x.Name));
        }
        public bool ExitVal()   {
            return false;
        }
    }

    public class LoadAssembly : ICommand    {

        public LoadAssembly(String path) {
            try
            {
                Program.activeasm.Add(AssemblyLoadContext.Default.LoadFromAssemblyPath(path));
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

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