using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime;
using System.Runtime.Loader;

namespace reflectionCli {

    public class error : ICommand    {

        public error(string error)        {
            Console.WriteLine(error);
        }

        public error() : this("Generic Error")  {}

        public bool ExitVal()        {
            return false;
        }
    }

}