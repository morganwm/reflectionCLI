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
                Program.activeasm.Select(a => a.Value).ToList().ForEach(x => Console.WriteLine(x.GetName().Name));
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