using System;
using System.Collections.Generic;

namespace ReflectionCli.Lib
{
    public class VariableService : IVariableService
    {
        public static Dictionary<string, dynamic> Vars = new Dictionary<string, dynamic>();

        public Dictionary<string, dynamic> Get()
        {
            return Vars;
        }

        public object Get(string name)
        {
            return Vars[name];
        }

        public void Set(Dictionary<string, dynamic> data)
        {
            Vars = data;
        }

        public void Set(string name, dynamic data)
        {
            if (Vars.ContainsKey(name)) {
                Vars[name] = data;
            } else {
                Vars.Add(name, data);
            }
        }
    }
}