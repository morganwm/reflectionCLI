using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime;
using System.Runtime.Loader;

namespace reflectionCli {

    public static class Parser    {
        public static ICommand Parse(string commandString) {

            var commandParts = commandString.Split(' ').ToList();
            var commandName = commandParts[0];
            var args = commandParts.Skip(1).ToList();



            var commandtypes = Assembly.GetEntryAssembly().DefinedTypes
                                .Where(x => x.ImplementedInterfaces.Contains(typeof(ICommand)))
                                .Where(x => (x.Name == commandName))
                                .ToList();

            if (commandtypes.Count == 0) {
                return new error("unable to find command");
            }

            if (commandtypes.Count > 1) {
                string msg = "multiple commands found: " + Environment.NewLine;
                commandtypes.ForEach(x => { msg = msg + "   " + x.FullName + Environment.NewLine; });
                return new error(msg);
            }

            Type type = commandtypes[0].AsType();

            ConstructorInfo constructorInfo = type.GetConstructors()[0];

            object result = null;
            ParameterInfo[] parameters = constructorInfo.GetParameters();

            if (parameters.Length == 0) {
                result = Activator.CreateInstance(type, null);
            }
            else {
                //result = Activator.CreateInstance(type, new object[] { args } );
                result = Activator.CreateInstance(type, args[0]); //this is only here for now until I can get the parameter stuff working
            }

            return (ICommand)result;
        }
    }


}
