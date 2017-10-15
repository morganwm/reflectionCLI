using System;
using ReflectionCli.Lib;

namespace ReflectionCli
{
    public class Verbose : ICommand
    {
        public Verbose(bool set)
        {
            Program.Verbose = set;
        }

        public Verbose()
        {
            Console.WriteLine(Program.Verbose);
        }

        public bool ExitVal()
        {
            return false;
        }
    }
}