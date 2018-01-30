using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using ReflectionCli.Lib;

namespace ReflectionCli.extended
{
    public class ScriptingCommandSet
    {
        public class RunScript : ICommand
        {
            private readonly ILoggingService _loggingService;
            public RunScript(ILoggingService loggingService)
            {
                _loggingService = loggingService;
            }
            public async void Run(string script, List<string> references = null, List<string> imports = null, bool exactRefPaths = false)
            {
                try
                {
                    _loggingService.Log(Environment.NewLine + "Running..." + Environment.NewLine);
                    List<MetadataReference> metaReferences = new List<MetadataReference>();
                    var options = ScriptOptions.Default;

                    if (references != null)
                    {
                        foreach (var refernceString in references)
                        {
                            _loggingService.Log($"Loading {refernceString}");
                            PortableExecutableReference tempRef;
                            if (exactRefPaths)
                            {
                                tempRef = MetadataReference.CreateFromFile(refernceString);
                            }
                            else
                            {
                                tempRef = MetadataReference.CreateFromFile(Assembly.Load(new AssemblyName(refernceString)).Location);
                            }

                            _loggingService.Log($"Loaded {refernceString} from {tempRef.FilePath}");
                            metaReferences.Add(tempRef);
                        }
                        options = ScriptOptions.Default.AddReferences(metaReferences);
                    }

                    if (imports != null)
                    {
                        options = ScriptOptions.Default.AddImports(imports);
                    }

                    Console.WriteLine();

                    try
                    {
                        _loggingService.Log(await CSharpScript.EvaluateAsync(script, options));
                    }
                    catch (CompilationErrorException e)
                    {
                        _loggingService.LogInfo(string.Join(Environment.NewLine, e.Diagnostics));
                        throw new Exception("Unable to Run Script.", e);
                    }

                    _loggingService.Log(Environment.NewLine + "Done");
                }
                catch (Exception ex)
                {
                    _loggingService.LogException(ex);
                }
            }
        }
    }
}