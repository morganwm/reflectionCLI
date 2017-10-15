using System;
using System.Collections.Generic;

namespace ReflectionCli
{
    public interface IVariableService
    {
        Dictionary<string, dynamic> Get();

        object Get(string name);
        void Set(Dictionary<string, dynamic> data);

        void Set( string name, dynamic data);
    }
}