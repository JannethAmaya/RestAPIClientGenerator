using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamasoft.JsonClassGenerator;
using Xamasoft.JsonClassGenerator.CodeWriters;
using System.IO;
using System.Configuration;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            var json = File.ReadAllText("sample.json");
            string targetFolder=@"C:\Projects\Generated Classes";
            var gen = Prepare(json, "txtNameSpaceText", targetFolder, "SampleResponse");
            gen.GenerateClasses();
        }
        private static JsonClassGenerator Prepare(string json, string nameSpace, string targetFolder, string mainClass )
        {
            var gen = new JsonClassGenerator();
            gen.Example = json;
            gen.InternalVisibility = true; 
            gen.CodeWriter = new CSharpCodeWriter();
            gen.ExplicitDeserialization = false;
            gen.Namespace = string.IsNullOrEmpty(nameSpace) ? null : nameSpace;
            gen.NoHelperClass = false;
            gen.SecondaryNamespace = null;
            gen.TargetFolder = targetFolder;
            gen.UseProperties = true;
            gen.MainClass = mainClass;
            gen.UsePascalCase = false;
            gen.UseNestedClasses = false;
            gen.ApplyObfuscationAttributes = false;
            gen.SingleFile = false;
            gen.ExamplesInDocumentation = false;
            return gen;
        }
    }
}
