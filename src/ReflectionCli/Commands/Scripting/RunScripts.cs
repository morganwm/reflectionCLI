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
            public async void Run(string script, List<string> references = null, List<string> imports = null, bool exactRefPaths = false)
            {
                Console.WriteLine();
                Console.WriteLine("Running...");
                Console.WriteLine();
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
                            
                            Console.WriteLine($"Loaded {refernceString} from {tempRef.FilePath}");
                            metaReferences.Add(tempRef);

                        } catch (Exception ex) {
                            Console.WriteLine();
                            Console.WriteLine($"Exception Occured trying to load {refernceString}");
                            Console.WriteLine();
                            Console.WriteLine(ex.ToString());
                            return;
                        }
                    }

                    options = ScriptOptions.Default.AddReferences(metaReferences);
                }

                if (imports != null) {
                    try {
                        options = ScriptOptions.Default.AddImports(imports);
                    } catch (Exception ex) {
                        Console.WriteLine();
                        Console.WriteLine($"Exception Occured trying to imports");
                        Console.WriteLine();
                        Console.WriteLine(ex.ToString());
                        return;
                    }
                }

                Console.WriteLine();
                try {
                    Console.WriteLine(await CSharpScript.EvaluateAsync(script, options));
                    Console.WriteLine();
                    Console.WriteLine("Done");
                } catch (CompilationErrorException e) {
                    Console.WriteLine(string.Join(Environment.NewLine, e.Diagnostics));
                }
            }
        }
    }
}