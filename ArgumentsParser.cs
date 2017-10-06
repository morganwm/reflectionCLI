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
        public static object[] ParseArgumentsFromString(string input, Type type) {
            List<object> outval = new List<object>();

            var parts = input.Split(new char[] { ' ' }, 2);
            if (parts.Length < 2) { return null; }

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
            Dictionary<Match, List<Object>> parampackages = new Dictionary<Match, List<Object>>();
            for (int i = 0; i < paramnames.Count(); i++)
            {
                List<Object> objs = new List<object>();
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
                                                  );
            if (matchingconstructors == null) {
                throw new Exception($"No Constructors for {type.Name} have matching input names to those Provided.");
            }

            if (matchingconstructors.Count() > 1) {
                throw new Exception($"Multiple Constructors for {type.Name} have matching input names, this is an issue with the way that {type.Name} was written.");
            }





            //inspect the different constructors and collect parameter info for all of them

            if (!parts[1].Contains(" -")) {
                if (!parts[1].Contains(',')) {
                    //outval = parts[1].Split(' ');
                }
                //needs to break the commands up on spaces and then if there are commas break that up into arrays
            }

            //this needs to break things up based on '-' and then ignore the first word after the '-'
            var args = parts[1].Split('-');

            return outval.ToArray();
        }
    }
}