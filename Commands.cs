using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;

namespace reflectionCli
{
    public class ExitCommand : ICommand
    {
        public bool Execute()
        {
            return true;
        }
    }

    public class ParrotCommand : ICommand
    {
        
        public ParrotCommand(List<string> input)
        {
            input.ForEach(x => Console.WriteLine(x));
        }
        
        public bool Execute()
        {
            return false;
        }
    }

    public class nullCommand : ICommand    
    {
        public bool Execute()
        {
            return false;
        }
    }
}