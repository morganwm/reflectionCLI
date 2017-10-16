using System;
using System.Collections.Generic;
using System.Reflection;

namespace ReflectionCli.Lib
{
    public interface IAssemblyService
    {
        List<Assembly> Get();
        void Set(List<Assembly> data);
        void Add(Assembly data);
    }
}