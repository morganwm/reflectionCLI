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
                _loggingService.Log(Environment.NewLine + "Running..." + Environment.NewLine);
                List<MetadataReference> metaReferences = new List<MetadataReference>();
                var options = ScriptOptions.Default;

                if (references != null) {
                    foreach (var refernceString in references) {
                        try {
                            PortableExecutableReference tempRef;
                            if (exactRefPaths) {
                                tempRef = MetadataReference.CreateFromFile(refernceString);
                            } else {
                                tempRef = MetadataReference.CreateFromFile(Assembly.Load(new AssemblyName(refernceString)).Location);
                            }

                            _loggingService.Log($"Loaded {refernceString} from {tempRef.FilePath}");
                            metaReferences.Add(tempRef);
                        } catch (Exception ex) {
                            _loggingService.LogInfo(Environment.NewLine + $"Exception Occured trying to load {refernceString}" + Environment.NewLine);
                            _loggingService.LogException(ex);
                            return;
                        }
                    }

                    options = ScriptOptions.Default.AddReferences(metaReferences);
                }

                if (imports != null) {
                    try {
                        options = ScriptOptions.Default.AddImports(imports);
                    } catch (Exception ex) {
                        _loggingService.LogInfo(Environment.NewLine + "Exception Occured trying to import" + Environment.NewLine);
                        _loggingService.LogException(ex);
                        return;
                    }
                }

                Console.WriteLine();
                try {
                    _loggingService.Log(await CSharpScript.EvaluateAsync(script, options));
                    _loggingService.Log(Environment.NewLine + "Done");
                } catch (CompilationErrorException e) {
                    _loggingService.LogInfo(string.Join(Environment.NewLine, e.Diagnostics));
                }
            }
        }
    }
}