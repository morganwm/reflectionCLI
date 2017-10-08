using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using System.Reflection;


namespace reflectionCli {

    public class help : ICommand    {

        public help() {
            Console.WriteLine();
            Console.WriteLine("To run a Command type the name of the command followed by the name of each parameter and it's value.");
            Console.WriteLine("For example to run a command called \"sample\" you would just type: sample ");
            Console.WriteLine("If this command had an input called \"value1\" and you wanted to set that value to \"5\" you would type: ");
            Console.WriteLine("sample -value1 5");
            Console.WriteLine("To see a list of commands simply type \"list\" ");
            Console.WriteLine("To see a list of the inputs for a specific command type \"list -name \" followed by the name of the command");
            Console.WriteLine();
        }

        public bool ExitVal()   {
            return false;
        }
    }
}