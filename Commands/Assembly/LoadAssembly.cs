using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;

namespace ReflectionCli
{
    public class LoadAssembly : ICommand
    {
        public LoadAssembly(string path)
        {
            Assembly tempAsm = AssemblyLoadContext.Default.LoadFromStream(new MemoryStream(File.ReadAllBytes(path)));

            var tempicommand = tempAsm.GetTypes()
                .Where(x => x.Name == "ICommand")
                .ToList();

            if (tempicommand.Count == 0) {
                throw new Exception("Unable to find ICommand");
            }

            if (tempicommand.Count > 1) {
                throw new Exception("Multiple ICommands Found");
            }

            Type type = tempicommand[0];

            var extvalinfo = type.GetMethods().ToList()
                .Where(x => x.Name == "ExitVal")
                .Where(x => x.ReturnType == typeof(bool))
                .ToList();

            if (extvalinfo.Count == 0) {
                throw new Exception("ICommand did not return the correct exit value");
            }

            Program.ActiveAsm.Add(Guid.NewGuid(), tempAsm);
        }

        public bool ExitVal()
        {
            return false;
        }
    }
}