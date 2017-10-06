using System;
using System.Linq;
using System.IO;


namespace reflectionCli {

    public class exit : ICommand    {
        public bool ExitVal()   {
            return true;
        }
    }
}