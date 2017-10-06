using System;
using System.Linq;
using System.IO;


namespace reflectionCli {

    public class nullCommand : ICommand    {
        public bool ExitVal()        {
            return false;
        }
    }
}