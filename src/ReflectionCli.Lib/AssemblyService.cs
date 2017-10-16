using System;
using System.Collections.Generic;
using System.Reflection;

namespace ReflectionCli.Lib
{
    public class AssemblyService : IAssemblyService
    {
        public static List<Assembly> Vars;

        public List<Assembly> Get()
        {
            return Vars;
        }

        public void Set(List<Assembly> data)
        {
            Vars = data;
        }

        public void Add(Assembly data)
        {
            Vars.Add(data);
        }
    }
}