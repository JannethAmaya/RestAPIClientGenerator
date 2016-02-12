using System;
using System.IO;
using Xamasoft.JsonClassGenerator;
using Xamasoft.JsonClassGenerator.CodeWriters;

namespace ClassGeneratorSample
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var json = File.ReadAllText("sample.json");
            const string targetFolder = @"C:\Projects\GeneratedClasses";
            var gen = Prepare(json, "txtNameSpaceText", targetFolder, "SampleResponse");
            gen.GenerateClasses();
            Console.ReadKey();
        }
        private static JsonClassGenerator Prepare(string json, string nameSpace, string targetFolder, string mainClass )
        {
            var gen = new JsonClassGenerator
            {
                Example = json,
                InternalVisibility = true,
                CodeWriter = new CSharpCodeWriter(),
                ExplicitDeserialization = false,
                Namespace = string.IsNullOrEmpty(nameSpace) ? null : nameSpace,
                NoHelperClass = false,
                SecondaryNamespace = null,
                TargetFolder = targetFolder,
                UseProperties = true,
                MainClass = mainClass,
                UsePascalCase = false,
                UseNestedClasses = false,
                ApplyObfuscationAttributes = false,
                SingleFile = false,
                ExamplesInDocumentation = false
            };

            return gen;
        }
    }
}
