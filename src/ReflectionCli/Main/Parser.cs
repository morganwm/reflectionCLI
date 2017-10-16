using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ReflectionCli.Lib;

namespace ReflectionCli
{
    public static class Parser
    {
        public static void Parse(string commandString)
        {
            object result;
            try {
                string asmName;
                string commandName;

                if (string.IsNullOrEmpty(commandString)) {
                    throw new Exception($"Please Enter the name of a command");
                }

                var commandtypes = new List<TypeInfo>();

                // switch to search in a specific assembly
                if (commandString[0] == '@') {
                    // search specific assembly
                    asmName = commandString.Split(' ')
                        .ToList()[0]
                        .Remove(0, 1);

                    commandName = commandString.Split(' ')
                        .ToList()[1];

                    Program.ActiveAsm.Where(t => t.Value.GetName().Name == asmName)
                        .Select(u => u.Value)
                        .ToList()
                        .ForEach(u =>
                        {
                            u.DefinedTypes.Where(v => (
                                // this has to be done this way as the ICommand interface is not object equivalent for runtime loaded assemblies
                                v.ImplementedInterfaces.Where(w => w.Name == nameof(ICommand))
                                    .ToList()
                                    .Count != 0
                            ))
                            .Where(v => v.Name.Equals(commandName, StringComparison.CurrentCultureIgnoreCase))
                            .ToList()
                            .ForEach(v => commandtypes.Add(v));
                        });
                } else {
                    // search for commands in all active assemblies
                    commandName = commandString.Split(' ')
                        .ToList()[0];

                    Program.ActiveAsm.Select(t => t.Value)
                        .ToList()
                        .ForEach(t =>
                        {
                            t.DefinedTypes.Where(u => (
                                // this has to be done this way as the ICommand interface is not object equivalent for runtime loaded assemblies
                                u.ImplementedInterfaces.Where(v => v.Name == nameof(ICommand))
                                    .ToList()
                                    .Count != 0
                            ))
                            .Where(u => u.Name.Equals(commandName, StringComparison.CurrentCultureIgnoreCase))
                            .ToList()
                            .ForEach(u => commandtypes.Add(u));
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
                    commandtypes.ForEach(t => { msg = msg + $"   {t.FullName}{Environment.NewLine}"; });

                    throw new Exception(msg);
                }

                Type type = commandtypes[0].AsType();

                ConstructorInfo constructorInfo = null;
                var args = ArgumentsParser.ParseArgumentsFromString(commandString, type, ref constructorInfo);
                ParameterInfo[] paramsinfo = constructorInfo.GetParameters();

                result = Activator.CreateInstance(type, (paramsinfo.Length == 0) ? null : args );
            } catch (Exception ex) {
                result = new Error( Program.Verbose ? ex.ToString() : ex.Message );
            }
        }
    }
}