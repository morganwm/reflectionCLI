using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime;
using System.Runtime.Loader;

namespace reflectionCli {

    public class nullCommand : ICommand    {
        public bool ExitVal()        {
            return false;
        }
    }
}