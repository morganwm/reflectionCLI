using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;

namespace reflectionCli {
    public class commandset {



        public class ParrotCommand : ICommand    {
            
            public ParrotCommand(List<string> input)
            {
                input.ForEach(x => Console.WriteLine(x));
            }
            
            public bool ExitVal()
            {
                return false;
            }
        }

        
    }
}