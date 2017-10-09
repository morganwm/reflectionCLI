using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime;
using System.Runtime.Loader;

namespace reflectionCli {

    public static class Parser    {
        public static Boolean Parse(string commandString) {
            object result = new nullCommand();
            try {

                string asmname;
                string commandName;

                List<TypeInfo> commandtypes = new List<TypeInfo>();

                //switch to search in a specific assembly
                if (commandString[0] == '@')
                {
                    //search specific assembly
                    asmname = commandString.Split(' ').ToList()[0].Remove(0,1);
                    commandName = commandString.Split(' ').ToList()[1];
                    Program.activeasm.Where(r => r.Value.GetName().Name == asmname).Select(a => a.Value).ToList().ForEach(x => {
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
                }
                else {
                    //search for commands in all active assemblies
                    commandName = commandString.Split(' ').ToList()[0];
                    Program.activeasm.Select(a => a.Value).ToList().ForEach(x => {
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
                }


                

                // var commandtypes = Assembly.GetEntryAssembly().DefinedTypes
                //                     .Where(x => x.ImplementedInterfaces.Contains(typeof(ICommand)))
                //                     .Where(x => (x.Name == commandName))
                //                     .ToList();

                if (commandtypes.Count == 0) {
                    throw new Exception($"unable to find command {commandName}");
                }

                if (commandtypes.Count > 1) {
                    string msg = $"multiple commands found:{Environment.NewLine}";
                    commandtypes.ForEach(x => { msg = msg + $"   {x.FullName}{Environment.NewLine}";});
                    throw new Exception(msg);
                }

                Type type = commandtypes[0].AsType();

                ConstructorInfo constructorInfo = null;
                var args = ArgumentsParser.ParseArgumentsFromString(commandString, type, ref constructorInfo);
                ParameterInfo[] paramsinfo = constructorInfo.GetParameters();

                if (paramsinfo.Length == 0) {
                    result = Activator.CreateInstance(type, null);
                }
                else {
                    result = Activator.CreateInstance(type, args);
                }
            }
            catch (Exception ex) {
                result = (Program.verbose) ?  new error(ex.ToString()) : new error(ex.Message);
            }

            //this has to be done through reflection because anything loaded at runtime won't have the same interface so a cast to ICommand would break
            Boolean exitbool = (Boolean)result.GetType().GetMethod("ExitVal").Invoke(result, null);
            return exitbool;
        }
    }
}