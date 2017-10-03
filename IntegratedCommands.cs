using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;

namespace reflectionCli {
    public interface ICommand    {
        bool ExitVal();
    }

    public class nullCommand : ICommand    {
        public bool ExitVal()        {
            return false;
        }
    }

    public class errorCommand : ICommand    {

        public errorCommand(string error)        {
            Console.WriteLine(error);
        }

        public errorCommand() : this("Generic Error")   {

        }

        public bool ExitVal()        {
            return false;
        }
    }

    public class ExitCommand : ICommand    {
        public bool ExitVal()   {
            return true;
        }
    }
}