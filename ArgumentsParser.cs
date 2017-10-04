using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime;
using System.Runtime.Loader;

namespace reflectionCli {

    public static class ArgumentsParser {
        public static object[] ParseArgumentsFromString(string input, ParameterInfo[] paramsinfo) {
            object[] outval = new object[0];

            var parts = input.Split(new char[] { ' ' }, 2);
            if (parts.Length < 2) { return null; }

            if (!parts[1].Contains(" -")) {
                if (!parts[1].Contains(',')) {
                    outval = parts[1].Split(' ');
                }
                //needs to break the commands up on spaces and then if there are commas break that up into arrays
            }

            //this needs to break things up based on '-' and then ignore the first word after the '-'
            var args = parts[1].Split('-');

            return outval;
        }
    }
}