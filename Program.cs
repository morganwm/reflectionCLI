using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using System.Reflection;

namespace reflectionCli {
    class Program    {
        public static void Main(string[] args)  {
            var exit = false;
            while(exit == false)    {
                Console.WriteLine();
                Console.WriteLine("Enter command (help to display help): "); 
                var command = Parser.Parse(Console.ReadLine());
                exit = command.ExitVal();
            }
        }
    }

    public interface ICommand    {
        bool ExitVal();
    }

    public class nullCommand : ICommand    {
        public bool ExitVal()        {
            return false;
        }
    }

    public class errorCommand : ICommand    {
        
        public errorCommand(string error)        {
            Console.WriteLine(error);
        }

        public errorCommand() : this("Generic Error")   {

        }
        
        public bool ExitVal()        {
            return false;
        }
    }

    public class ExitCommand : ICommand    {
        public bool ExitVal()   {
            return true;
        }
    }

    public static class Parser    {
        public static ICommand Parse(string commandString) { 
            
            // Parse your string and create Command object
            var commandParts = commandString.Split(' ').ToList();
            var commandName = commandParts[0];
            var args = commandParts.Skip(1).ToList(); // the arguments are after the command

            Assembly assembly = typeof(commandset).GetTypeInfo().Assembly;
            Type type = assembly.GetType("reflectionCli.commandset+" + commandName);
            if (type == null)   {
                return new errorCommand("Command Set Object Equals null");
            }

            type.GetConstructors().ToList().ForEach(x => {
                Console.WriteLine(x.Name);
                x.GetParameters().ToList().ForEach(y => Console.WriteLine(" " + y.Name + "-" + y.ParameterType));
            });


            //type.GetMethods().ToList().ForEach(x => Console.WriteLine(x.Name));

            // ConstructorInfo constructorInfo = type.GetConstructor(Type.EmptyTypes);
            // if (constructorInfo == null)   {
            //     Type[] _types = new Type[1];
            //     _types[0] = typeof(List<string>);
            //     constructorInfo = type.GetConstructor(_types);
            //     if (constructorInfo == null)   {
            //         return new errorCommand("constructor info is equal to null");
            //     }
            // }

            // object result = null;
            // ParameterInfo[] parameters = constructorInfo.GetParameters();
            // object classInstance = Activator.CreateInstance(type, );

            // if (parameters.Length == 0) {
            //     result = constructorInfo.Invoke(classInstance, null);
            // }
            // else {
            //     result = constructorInfo.Invoke(classInstance, new object[] { args } );
            // }

            return new errorCommand();
        }  
    }
}
