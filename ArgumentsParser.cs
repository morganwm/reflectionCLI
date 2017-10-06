using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime;
using System.Runtime.Loader;
using System.Text.RegularExpressions;

namespace reflectionCli {

    public static class ArgumentsParser {
        public static object[] ParseArgumentsFromString(string input, Type type, ref ConstructorInfo constructor) {
            List<object> outval = new List<object>();

            //no input case
            var parts = input.Split(new char[] { ' ' }, 2);
            if (parts.Length < 2) {
                 constructor = type.GetConstructors().Where(x => (x.GetParameters().Count() == 0)).ToList()[0];

                if (constructor == null) {
                    throw new Exception($"No Constructors for {type.Name} have 0 arguments {Environment.NewLine}");
                }

                 return null;
            }

            string argstring = parts[1];
            var atoms = Regex.Matches(argstring, "(\\-\\S*)|(\\[.+?\\])|(\\\".+?\\\")|(\\S*)").Cast<Match>().Where(x => !String.IsNullOrEmpty(x.Value));

            var paramnames = atoms.Where(x => (x.Value[0] == '-'));

            if (paramnames == null) {
                throw new Exception("No Parameter Names were found. Please use the convention: -ParameterName Value");
            }

            //see if any of the parameter counts match given inputs
            var validconstructors = type.GetConstructors()
                                        .Where(x => (x.GetParameters().Count() == paramnames.Count()));

            if (validconstructors == null) {
                throw new Exception($"No Constructors for {type.Name} have {paramnames.Count()} arguments {Environment.NewLine}");
            }

            //break input into groups based on the variable name that came before them
            Dictionary<Match, List<string>> parampackages = new Dictionary<Match, List<string>>();
            for (int i = 0; i < paramnames.Count(); i++)
            {
                List<string> objs = new List<string>();
                if (i == paramnames.Count() - 1) {
                    atoms.ToList().Where(x => ((x.Index > paramnames.ToList()[i].Index)))
                                    .ToList()
                                    .ForEach(x => objs.Add(x.Value));
                }
                else {
                    atoms.ToList().Where(x => ((x.Index > paramnames.ToList()[i].Index) &&
                                                (x.Index < paramnames.ToList()[i+1].Index)))
                                    .ToList()
                                    .ForEach(x => objs.Add(x.Value));
                }

                parampackages.Add(paramnames.ToList()[i], objs);
            }

            //find constructor whos vartiable names match those given
            var matchingconstructors = type.GetConstructors()
                                            .Where(c => c.GetParameters()
                                                         .Select(x => x.Name)
                                                         .Intersect(parampackages.Select(y => y.Key.Value.Remove(0,1))).Count() == parampackages.Count()
                                                         &&
                                                         parampackages.Select(y => y.Key.Value.Remove(0,1))
                                                         .Intersect(c.GetParameters().Select(x => x.Name)).Count() == c.GetParameters().Count()
                                                  );

            if (matchingconstructors == null) {
                throw new Exception($"No Constructors for {type.Name} have matching input names to those Provided.");
            }

            if (matchingconstructors.Count() > 1) {
                throw new Exception($"Multiple Constructors for {type.Name} have matching input names, this is an issue with the way that {type.Name} was written.");
            }

            ConstructorInfo chosenconstructor = matchingconstructors.ToList()[0];
            Object[] robjs = new object[chosenconstructor.GetParameters().Count()];
            for (int i = 0; i < robjs.Count(); i++) {
                Type outtype = chosenconstructor.GetParameters().ToList()[i].ParameterType;
                var tempobj = parampackages.Where(x => (x.Key.Value.Remove(0,1) == chosenconstructor.GetParameters().ToList()[i].Name))
                                            .Select(y => y.Value).ToList()[0];


                if (tempobj.Count() == 1) {
                    outval.Add(Convert.ChangeType(tempobj.ToArray()[0], outtype));
                }
                else {
                    outval.Add(Convert.ChangeType(tempobj, outtype));
                }
            }



            constructor = chosenconstructor;
            return outval.ToArray();
        }
    }
}