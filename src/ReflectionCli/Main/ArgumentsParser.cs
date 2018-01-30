using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using ReflectionCli.Lib;

namespace ReflectionCli
{
    public static class ArgumentsParser
    {
        public static object[] ParseArgumentsFromString(string input, Type type, ref MethodInfo method)
        {
            var outval = new List<object>();

            // no input case
            var parts = input.Split(new char[] { ' ' }, 2);
            if (parts.Length < 2) {
                var methodlistList = type.GetMethods()
                    .Where(t => t.GetParameters().Count() == 0)
                    .ToList();

                if (methodlistList.Count == 0) {
                    throw new Exception($"No Constructors for {type.Name} have 0 arguments {Environment.NewLine}");
                }

                method = methodlistList[0];

                return null;
            }

            string argstring = parts[1];
            var atoms = Regex.Matches(argstring, "(\\-\\S*)|(\\[.+?\\])|(\\\".+?\\\")|(\\S*)")
                .Cast<Match>()
                .Where(x => !string.IsNullOrEmpty(x.Value));

            var paramNames = atoms.Where(t => t.Value[0] == '-');
            var paramNamesTrimmed = paramNames.Select(t => t.Value.Remove(0, 1));

            if (paramNames == null) {
                throw new Exception("No Parameter Names were found. Please use the convention: -ParameterName Value");
            }

            // see if any of the parameter counts match given inputs
            var validconstructors = type.GetConstructors()
                .Where(x => x.GetParameters().Count() == paramNames.Count());

            if (validconstructors == null) {
                throw new Exception($"No Constructors for {type.Name} have {paramNames.Count()} arguments {Environment.NewLine}");
            }

            // break input into groups based on the variable name that came before them
            var paramPackages = new Dictionary<Match, List<string>>();
            for (int i = 0; i < paramNames.Count(); i++) {
                var objects = new List<string>();
                if (i == paramNames.Count() - 1) {
                    objects = atoms.Where(t => t.Index > paramNames.ToList()[i].Index)
                        .Select(t => t.Value)
                        .ToList();
                } else {
                    objects = atoms.Where(t => (t.Index > paramNames.ToList()[i].Index) && (t.Index < paramNames.ToList()[i + 1].Index))
                        .Select(t => t.Value)
                        .ToList();
                }

                // strip quotes off of things that start and end with them
                for (int j = 0; j < objects.Count(); j++) {
                    if (objects[j][0] == '"' && objects[j].Last() == '"') {
                        objects[j] = objects[j].Substring(1, objects[j].Length - 2);
                    }
                }

                paramPackages.Add(paramNames.ToList()[i], objects);
            }

            // find constructor whos vartiable names match those given
            var methods = type.GetMethods().Where(t => t.Name == "Run");
            var methodParams = methods.ToDictionary(t => t, t => t.GetParameters().Select(u => u.Name));

            var matchingmethods = new List<MethodInfo>();
            foreach (var methodToSelect in methods) {
                bool isGood = paramNamesTrimmed.Count() <= methodToSelect.GetParameters().Count();
                foreach (var paramToCheck in methodToSelect.GetParameters()) {
                    isGood = isGood && (paramNamesTrimmed.Contains(paramToCheck.Name) || paramToCheck.HasDefaultValue);
                }
                if (isGood) {
                    matchingmethods.Add(methodToSelect);
                }
            }

            if (matchingmethods.Count() == 0) {
                throw new Exception($"No Constructors for {type.Name} have matching input names to those Provided.");
            }

            if (matchingmethods.Count() > 1) {
                throw new Exception($"Multiple Constructors for {type.Name} have matching input names, this is an issue with the way that {type.Name} was written.");
            }

            MethodInfo chosenmethod = matchingmethods[0];
            for (int i = 0; i < chosenmethod.GetParameters().Count(); i++) {
                ParameterInfo outTypeInfo = chosenmethod.GetParameters().ToList()[i];

                if (outTypeInfo.HasDefaultValue && !paramNamesTrimmed.Contains(outTypeInfo.Name)) {
                    outval.Add(null);
                    continue;
                }

                Type outType = outTypeInfo.ParameterType;
                var tempObject = paramPackages
                    .Where(t => t.Key.Value.Remove(0, 1) == outTypeInfo.Name)
                    .Select(y => y.Value)
                    .ToList()[0];

                if (tempObject.Count() == 1 && !outType.IsArray && !outType.GetInterfaces().Contains(typeof(System.Collections.IList))) {
                    outval.Add(Convert.ChangeType(tempObject.ToArray()[0], outType));
                } else {
                    Type nesttype = outType.GetTypeInfo().GenericTypeArguments[0];
                    switch (nesttype.Name) {
                        case "Int32":
                            outval.Add(tempObject.Select(x => Convert.ToInt32(x)).ToList());
                            break;

                        case "Double":
                            outval.Add(tempObject.Select(x => Convert.ToDouble(x)).ToList());
                            break;

                        case "Boolean":
                            outval.Add(tempObject.Select(x => Convert.ToBoolean(x)).ToList());
                            break;

                        case "Decimal":
                            outval.Add(tempObject.Select(x => Convert.ToDecimal(x)).ToList());
                            break;

                        case "DateTime":
                            outval.Add(tempObject.Select(x => Convert.ToDateTime(x)).ToList());
                            break;

                        case "Byte":
                            outval.Add(tempObject.Select(x => Convert.ToByte(x)).ToList());
                            break;

                        default:
                            outval.Add(tempObject);
                            break;
                    }
                }
            }

            method = chosenmethod;
            return outval.ToArray();
        }
    }
}