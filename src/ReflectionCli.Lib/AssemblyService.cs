using System.Collections.Generic;
using System.Reflection;

namespace ReflectionCli.Lib
{
    public class AssemblyService : IAssemblyService
    {
        public static Dictionary<string, Assembly> Vars;

        public Dictionary<string, Assembly> Get()
        {
            return Vars;
        }

        public Assembly Get(string name)
        {
            return Vars[name];
        }
        public void Set(Dictionary<string, Assembly> data)
        {
            Vars = data;
        }

        public void Set(string name, Assembly data)
        {
            if (Vars.ContainsKey(name))
            {
                Vars[name] = data;
            } else {
                Vars.Add(name, data);
            }
        }
    }
}