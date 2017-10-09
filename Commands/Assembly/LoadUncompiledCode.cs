using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime;
using System.Runtime.Loader;
using System.CodeDom.Compiler;
using Microsoft.CSharp;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace reflectionCli {
    public class LoadUncompiledCode : ICommand    {

        public LoadUncompiledCode(String path) {
            try
            {
                Assembly TempAsm = null;

                string readText = File.ReadAllText(path);

                CSharpCompilationOptions options = new CSharpCompilationOptions(
                    OutputKind.DynamicallyLinkedLibrary,
		            reportSuppressedDiagnostics: true,
		            optimizationLevel: OptimizationLevel.Release,
		            generalDiagnosticOption: ReportDiagnostic.Error,
                    allowUnsafe: true
                );

                MetadataReference[] references = new MetadataReference[] {
                    MetadataReference.CreateFromFile(typeof(object).GetTypeInfo().Assembly.Location),
                    MetadataReference.CreateFromFile(typeof(ValueTuple<>).GetTypeInfo().Assembly.Location),
                    MetadataReference.CreateFromFile(typeof(object).GetTypeInfo().Assembly.Location),
                    //MetadataReference.CreateFromFile(typeof(RuntimeBinderException).GetTypeInfo().Assembly.Location),
                    MetadataReference.CreateFromFile(typeof(System.Runtime.CompilerServices.DynamicAttribute).GetTypeInfo().Assembly.Location),
                    //MetadataReference.CreateFromFile(typeof(ExpressionType).GetTypeInfo().Assembly.Location),
                    MetadataReference.CreateFromFile(Assembly.Load(new AssemblyName("mscorlib")).Location),
                    MetadataReference.CreateFromFile(Assembly.Load(new AssemblyName("System.Runtime")).Location)
                };

                var compilation = CSharpCompilation.Create(
                    Path.GetFileName(path) + Guid.NewGuid().ToString(),
                    references: references,
                    //syntaxTrees: new SyntaxTree[] { CSharpSyntaxTree.ParseText(code) },
                    syntaxTrees: new SyntaxTree[] { CSharpSyntaxTree.ParseText(readText) },
                    options: options
                );

                var stream = new MemoryStream();
                var emitResult = compilation.Emit(stream);


                if (emitResult.Success){
                    stream.Seek(0, SeekOrigin.Begin);
                    TempAsm = AssemblyLoadContext.Default.LoadFromStream(stream);
                }
                else {
                    string error = Environment.NewLine + "Assembly could not be created:" + Environment.NewLine;
                    emitResult.Diagnostics.Select(x =>  error = $"{x.Descriptor.Description.ToString()} : {x.Location.ToString()} {Environment.NewLine}");
                    throw new Exception(error);
                }

                //need to check and
                var tempicommand = TempAsm.GetTypes().ToList().Where(x => (x.Name == "ICommand")).ToList();

                if (tempicommand.Count == 0) {
                    throw new Exception("Unable to find ICommand");
                }

                if (tempicommand.Count > 1) {
                    throw new Exception("Multiple ICommands Found");
                }

                Type type = tempicommand[0];

                var extvalinfo = type.GetMethods().ToList()
                                    .Where(x => (x.Name == "ExitVal"))
                                    .Where(x => (x.ReturnType == typeof(bool)))
                                    .ToList();

                if (extvalinfo.Count == 0) {
                    throw new Exception("ICommand did not return the correct exit value");
                }

                Program.activeasm.Add(Guid.NewGuid() ,TempAsm);

            }
            catch (Exception ex)
            {
                Console.WriteLine( (Program.verbose) ? ex.ToString() : ex.Message );
            }

        }
        public bool ExitVal()   {
            return false;
        }


    }

}