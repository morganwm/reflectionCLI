using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;

namespace reflectionCli
{
    class Program
    {
        public static void Main(string[] args)
        {
            var exit = false;
            while(exit == false)
            {
                Console.WriteLine();
                Console.WriteLine("Enter command (help to display help): "); 
                var command = Parser.Parse(Console.ReadLine());
                exit = command.Execute();
            }
        }
    }

    public interface ICommand
    {
        bool Execute();
    }

    public static class Parser
    {
        public static ICommand Parse(string commandString) { 
            // Parse your string and create Command object
            var commandParts = commandString.Split(' ').ToList();
            var commandName = commandParts[0];
            var args = commandParts.Skip(1).ToList(); // the arguments is after the command
            switch(commandName)
            {
                // Create command based on CommandName (and maybe arguments)
                case "exit": 
                    return new ExitCommand();

                case "parrot": 
                    return new ParrotCommand(args);

                default:
                    Console.WriteLine("defaultcase");
                    return new nullCommand();

            }
        }  
    }
}
