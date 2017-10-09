using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime;
using System.Runtime.Loader;

namespace reflectionCli {
    public class ListAssemblies : ICommand    {

        public ListAssemblies() {
            try
            {
                Program.activeasm.ToList().ForEach(x => Console.WriteLine($"{x.Value.GetName().Name}: {x.Key}"));
            }
            catch (Exception ex)
            {
                Console.WriteLine( (Program.verbose) ? ex.ToString() : ex.Message );
            }

        }
        public bool ExitVal()   {
            return false;
        }
    }

}