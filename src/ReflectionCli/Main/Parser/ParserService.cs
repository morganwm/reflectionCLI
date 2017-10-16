using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ReflectionCli.Lib;

namespace ReflectionCli
{
    public class ParserService : IParserService
    {
        private readonly IAssemblyService _assemblyservice;
        private readonly ILoggingService _loggingservice;

        public ParserService(IAssemblyService assemblyservice, ILoggingService loggingservice)
        {
            _assemblyservice = assemblyservice;
            _loggingservice = loggingservice;
        }

        public void Parse(string commandString)
        {
            try
            {
                string asmName;
                string commandName;

                if (string.IsNullOrEmpty(commandString))
                {
                    throw new Exception($"Please Enter the name of a command");
                }

                var commandtypes = new List<TypeInfo>();

                // switch to search in a specific assembly
                if (commandString[0] == '@')
                {
                    // search specific assembly
                    asmName = commandString.Split(' ')
                        .ToList()[0]
                        .Remove(0, 1);

                    commandName = commandString.Split(' ')
                        .ToList()[1];

                    _assemblyservice.Get().Where(t => t.GetName().Name == asmName)
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
                }
                else
                {
                    // search for commands in all active assemblies
                    commandName = commandString.Split(' ')
                        .ToList()[0];

                    _assemblyservice.Get()
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

                if (commandtypes.Count == 0)
                {
                    throw new Exception($"unable to find command {commandName}");
                }

                if (commandtypes.Count > 1)
                {
                    string msg = $"multiple commands found:{Environment.NewLine}";
                    commandtypes.ForEach(t => { msg = msg + $"   {t.FullName}{Environment.NewLine}"; });

                    throw new Exception(msg);
                }

                Type type = commandtypes[0].AsType();

                var constructors = type.GetConstructors();

                if (constructors.Count() > 1)
                {
                    throw new Exception($"Multiple constructors found for {commandName}");
                }

                var constructorparams = constructors[0].GetParameters()
                    .Select(t => Program.ServiceProvider.GetService(t.ParameterType));

                object[] constructorparamsarray = new object[constructorparams.Count()];

                for (int i = 0; i < constructorparams.Count(); i++)
                {
                    constructorparamsarray[i] = constructorparams.ToArray()[i];
                }

                bool nullconstructor = constructorparams.Count() == 0;
                nullconstructor = nullconstructor || (constructorparams.Where(t => t != null).Count() == 0);

                var functioninstance = nullconstructor ? Activator.CreateInstance(type) : Activator.CreateInstance(type, constructorparamsarray);

                MethodInfo methodinfo = null;
                var args = ArgumentsParser.ParseArgumentsFromString(commandString, type, ref methodinfo);
                ParameterInfo[] paramsinfo = methodinfo.GetParameters();

                methodinfo.Invoke(functioninstance, args);
            }
            catch (Exception ex)
            {
                _loggingservice.LogError(ex);
            }
        }
    }
}