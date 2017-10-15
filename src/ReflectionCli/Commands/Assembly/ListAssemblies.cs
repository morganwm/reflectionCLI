using System;
using System.Linq;
using ReflectionCli.Lib;

namespace ReflectionCli
{
    public class ListAssemblies : ICommand
    {
        public ListAssemblies()
        {
            Program.ActiveAsm.ToList().ForEach(x => Console.WriteLine($"{x.Value.GetName().Name}: {x.Key}"));
        }

        public bool ExitVal()
        {
            return false;
        }
    }
}