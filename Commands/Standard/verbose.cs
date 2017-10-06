using System;
using System.Linq;
using System.IO;


namespace reflectionCli {

    public class verbose : ICommand    {

        public verbose(Boolean set)        {
            Program.verbose = set;
        }

        public verbose()  {
            Console.WriteLine(Program.verbose);
        }

        public bool ExitVal()        {
            return false;
        }
    }

}