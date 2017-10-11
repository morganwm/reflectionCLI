using System;
using System.Linq;
using System.Reflection;

namespace ReflectionCli
{
    public class RemoveAssembly : ICommand
    {
        public RemoveAssembly(string name)
        {
            var tempAsmEntries = Program.ActiveAsm.Where(t => t.Value.GetName().Name == name);

            if (tempAsmEntries.Count() == 0) {
                throw new Exception($"Unable to find Assembly {name}");
            }

            if (tempAsmEntries.Count() > 1) {
                throw new Exception($"Multiple Assemblies Found with the name: {name}");
            }

            if (tempAsmEntries.ToList()[0].Value == Assembly.GetEntryAssembly()) {
                throw new Exception($"Cannot remove assembly {Assembly.GetEntryAssembly().GetName().Name} as this is the Entry Assembly");
            }

            Program.ActiveAsm.Remove(tempAsmEntries.ToList()[0].Key);
        }

        public bool ExitVal()
        {
            return false;
        }
    }
}