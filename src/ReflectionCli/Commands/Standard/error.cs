using System;
using ReflectionCli.Lib;

namespace ReflectionCli
{
    public class Error : ICommand
    {
        public Error(string error)
        {
            Console.Error.WriteLine(error);
        }

        public Error()
            : this("Generic Error")
        {
        }

        public bool ExitVal()
        {
            return false;
        }
    }
}