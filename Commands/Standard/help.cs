using System;

namespace ReflectionCli
{
    public class Help : ICommand
    {
        public Help()
        {
            Console.WriteLine();
            Console.WriteLine("     To run a Command simply type the name of the command followed by the name of each parameter and it's value.");
            Console.WriteLine("     For example to run a command called \"sample\" you would just type: sample ");
            Console.WriteLine();
            Console.WriteLine("     If this command had an input called \"value1\" and you wanted to set that value to \"5\" you would type: ");
            Console.WriteLine("     sample -value1 5");
            Console.WriteLine();
            Console.WriteLine("     To see a list of commands simply type \"list\" ");
            Console.WriteLine("     To see a list of the inputs for a specific command type \"list -name \" followed by the name of the command");
            Console.WriteLine();
            Console.WriteLine("     To Input an array or list simply enter values in the list seperated by spaces:");
            Console.WriteLine("     samplefunction -inputarray element1 element2 element3");
            Console.WriteLine();
            Console.WriteLine("     To Exit, simply type \"exit\" ");
        }

        public bool ExitVal()
        {
            return false;
        }
    }
}