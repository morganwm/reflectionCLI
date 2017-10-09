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

            var TempAsmEntries = Program.activeasm.Where(x => (x.Value.GetName().Name == name));

            if (TempAsmEntries.Count() == 0) {
                throw new Exception($"Unable to find Assembly {name}");
            }

            if (TempAsmEntries.Count() > 1) {
                throw new Exception($"Multiple Assemblies Found with the name: {name}");
            }

            if (TempAsmEntries.ToList()[0].Value == Assembly.GetEntryAssembly()) {
                throw new Exception($"Cannot remove assemblly {Assembly.GetEntryAssembly().GetName().Name} as this is the Entry Assembly");
            }

            Program.activeasm.Remove(TempAsmEntries.ToList()[0].Key);

        }
        public bool ExitVal()   {
            return false;
        }
    }

}