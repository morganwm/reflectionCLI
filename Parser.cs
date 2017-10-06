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
            object result = new nullCommand();
            try {

                var commandParts = commandString.Split(' ').ToList();
                var commandName = commandParts[0];
                var args = commandParts.Skip(1).ToList();

                List<TypeInfo> commandtypes = new List<TypeInfo>();
                Program.activeasm.ForEach(x => {
                    x.DefinedTypes.Where(z => (
                        //this has to be done this way as the ICommand interface is not object equivalent for runtime loaded assemblies
                        z.ImplementedInterfaces.Where(a => (a.Name == "ICommand"))
                                                .ToList()
                                                .Count != 0
                    ))
                    .Where(a => (a.Name == commandName))
                    .ToList()
                    .ForEach(y => commandtypes.Add(y));
                });

                // var commandtypes = Assembly.GetEntryAssembly().DefinedTypes
                //                     .Where(x => x.ImplementedInterfaces.Contains(typeof(ICommand)))
                //                     .Where(x => (x.Name == commandName))
                //                     .ToList();

                if (commandtypes.Count == 0) {
                    return new error("unable to find command");
                }

                if (commandtypes.Count > 1) {
                    string msg = $"multiple commands found:{Environment.NewLine}";
                    commandtypes.ForEach(x => { msg = msg + $"   {x.FullName}{Environment.NewLine}";});
                    return new error(msg);
                }

                Type type = commandtypes[0].AsType();

                ConstructorInfo constructorInfo = null;

                var argstest = ArgumentsParser.ParseArgumentsFromString(commandString, type, ref constructorInfo);

                ParameterInfo[] paramsinfo = constructorInfo.GetParameters();

                if (paramsinfo.Length == 0) {
                    result = Activator.CreateInstance(type, null);
                }
                else {
                    result = Activator.CreateInstance(type, argstest);
                }
            }
            catch (Exception ex) {
                result = new error(ex.ToString());
            }

            return (ICommand)result;
        }
    }
}