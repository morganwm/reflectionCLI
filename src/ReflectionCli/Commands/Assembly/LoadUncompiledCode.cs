using System;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Loader;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CSharp.RuntimeBinder;
using ReflectionCli.Lib;

namespace ReflectionCli
{
    public class LoadUncompiledCode : ICommand
    {
        private readonly ILoggingService _loggingService;
        private readonly IAssemblyService _assemblyService;

        public LoadUncompiledCode(ILoggingService loggingService, IAssemblyService assemblyService)
        {
            _loggingService = loggingService;
            _assemblyService = assemblyService;
        }

        public void Run(string path)
        {
            try
            {
                Assembly tempAsm = null;

                string readText = File.ReadAllText(path);

                var options = new CSharpCompilationOptions(
                    OutputKind.DynamicallyLinkedLibrary,
                    reportSuppressedDiagnostics: true,
                    optimizationLevel: OptimizationLevel.Release,
                    generalDiagnosticOption: ReportDiagnostic.Error,
                    allowUnsafe: true);

                MetadataReference[] references = new MetadataReference[] {
                    MetadataReference.CreateFromFile(typeof(object).GetTypeInfo().Assembly.Location),
                    MetadataReference.CreateFromFile(typeof(System.Console).GetTypeInfo().Assembly.Location),
                    MetadataReference.CreateFromFile(typeof(Enumerable).Assembly.Location),
                    MetadataReference.CreateFromFile(typeof(ExpressionType).GetTypeInfo().Assembly.Location),
                    MetadataReference.CreateFromFile(typeof(RuntimeBinderException).GetTypeInfo().Assembly.Location),
                    MetadataReference.CreateFromFile(typeof(System.Runtime.CompilerServices.DynamicAttribute).GetTypeInfo().Assembly.Location),
                    MetadataReference.CreateFromFile(Assembly.Load(new AssemblyName("mscorlib")).Location),
                    MetadataReference.CreateFromFile(Assembly.Load(new AssemblyName("System.Runtime")).Location),
                    MetadataReference.CreateFromFile(Assembly.Load(new AssemblyName("System.Reflection")).Location),
                };

                var compilation = CSharpCompilation.Create(
                    Path.GetFileName(path) + Guid.NewGuid().ToString(),
                    references: references,
                    syntaxTrees: new SyntaxTree[] { CSharpSyntaxTree.ParseText(readText) },
                    options: options);

                var stream = new MemoryStream();
                var emitResult = compilation.Emit(stream);

                if (emitResult.Success) {
                    stream.Seek(0, SeekOrigin.Begin);
                    tempAsm = AssemblyLoadContext.Default.LoadFromStream(stream);
                } else {
                    _loggingService.Log();
                    emitResult.Diagnostics.Where(t => t.IsWarningAsError || t.Severity == DiagnosticSeverity.Error)
                        .ToList()
                        .ForEach(x => _loggingService.Log($"{x.Id} {x.Location.GetLineSpan().ToString()}: {x.GetMessage()}{Environment.NewLine}"));

                    throw new Exception($"{Environment.NewLine} Assembly could not be created {Environment.NewLine}");
                }

                // need to check and validate ICommand
                var tempicommand = tempAsm.GetTypes()
                    .Where(t => t.Name == "ICommand")
                    .ToList();

                if (tempicommand.Count == 0) {
                    throw new Exception("Unable to find ICommand");
                }

                if (tempicommand.Count > 1) {
                    throw new Exception("Multiple ICommands Found");
                }

                _assemblyService.Add(tempAsm);
            }
            catch (Exception ex)
            {
                _loggingService.LogException(ex);
            }
        }
    }
}