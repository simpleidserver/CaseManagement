using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace CaseManagement.Workflow.Engine
{
    public static class ExpressionParser
    {
        public static bool IsValid(string expression, ProcessFlowInstanceExecutionContext context)
        {
            var syntaxTree = CSharpSyntaxTree.ParseText(expression);
            var assemblyName = Path.GetRandomFileName();
            var references = new MetadataReference[]
            {
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Enumerable).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(ProcessFlowInstanceExecutionContext).Assembly.Location)
            };

            var compilation = CSharpCompilation.Create(
                assemblyName,
                syntaxTrees: new[] { syntaxTree },
                references: references,
                options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
            using (var ms = new MemoryStream())
            {
                var result = compilation.Emit(ms);
                ms.Seek(0, SeekOrigin.Begin);
                var assembly = Assembly.Load(ms.ToArray());
                var type = assembly.GetType("CaseManagement.Conditions");
                var obj = Activator.CreateInstance(type);
                return bool.Parse(type.InvokeMember("Check", BindingFlags.Default | BindingFlags.InvokeMethod, null, obj, new object[]
                {
                    context
                }).ToString());
            }
        }
    }
}
