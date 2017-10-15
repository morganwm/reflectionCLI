using System;
using System.Collections.Generic;
using System.Reflection;

namespace ReflectionCli
{
    public interface IAssemblyService
    {
        Dictionary<string, Assembly> Get();

        Assembly Get(string name);
        void Set(Dictionary<string, Assembly> data);

        void Set( string name, Assembly data);
    }
}