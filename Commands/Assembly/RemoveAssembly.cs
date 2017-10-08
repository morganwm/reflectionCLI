using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime;
using System.Runtime.Loader;

namespace reflectionCli {
    public class RemoveAssembly : ICommand    {

        public RemoveAssembly(String name) {
            try
            {
                var TempAsm = Program.activeasm.Where(x => (x.GetName().Name == name));

                if (TempAsm.Count() == 0) {
                    throw new Exception($"Unable to find Assembly {name}");
                }

                if (TempAsm.Count() > 1) {
                    throw new Exception($"Multiple Assemblies Found with the name: {name}");
                }

                if (TempAsm.ToList()[0] == Assembly.GetEntryAssembly()) {
                    throw new Exception($"Cannot remove assemblly {Assembly.GetEntryAssembly().GetName().Name} as this is the Entry Assembly");
                }

                Program.activeasm.Remove(TempAsm.ToList()[0]);
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