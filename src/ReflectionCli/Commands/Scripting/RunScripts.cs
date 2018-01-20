using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ReflectionCli.Lib;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;

namespace ReflectionCli.extended
{
    public class ScriptingCommandSet
    {
        public class RunScript : ICommand
        {
            public async void Run(string script)
            {
                try {
                    Console.WriteLine(await CSharpScript.EvaluateAsync(script));
                } catch (CompilationErrorException e) {
                    Console.WriteLine(string.Join(Environment.NewLine, e.Diagnostics));
                }
            }
        }
    }
}