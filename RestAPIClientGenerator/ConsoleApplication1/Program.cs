using System;
using System.IO;
using Xamasoft.JsonClassGenerator;
using Xamasoft.JsonClassGenerator.CodeWriters;
using RestSharp;
using System.Collections.Generic;
using System.Text;

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
        public string Request(Method method, string baseURL, string endPoint, Dictionary<string, object> headers, Dictionary<string, object> parameters, Dictionary<string, object> queryParameters, string body, string methodName, string nameSpace, string responseType, bool authentication, string username, string password) {
            //Create method call
            var sb = new StringBuilder("public " + responseType + " " +methodName+"(");
            var sbHeaders = new StringBuilder();
            var sbParameters = new StringBuilder();
            var sbQueryParameters = new StringBuilder();
            bool first=true;
            foreach (var key in headers.Keys)
            { 
                if(!first)
                {
                    sb.Append(", ");
                }
                sb.AppendFormat(" object {0}", key);
                sbHeaders.AppendLine(String.Format("headers.Add(\"{0}\", {0});", key));
            }
            
            first = true;
            foreach (var key in parameters.Keys)
            {
                if (!first)
                {
                    sb.Append(", ");
                }
                sb.AppendFormat(" object {0}", key);
                sbParameters.AppendLine(String.Format("parameters.Add(\"{0}\", {0});", key));
            }
            first = true;
            foreach (var key in queryParameters.Keys)
            {
                if (!first)
                {
                    sb.Append(", ");
                }
                sb.AppendFormat(" object {0}", key);
                sbQueryParameters.AppendLine(String.Format("queryParameters.Add(\"{0}\", {0});", key));
            }
            sb.Append(")");
            sb.AppendLine("{");
            sb.AppendLine(String.Format("    string baseUrl=\"{0}\";", baseURL));
            sb.AppendLine(String.Format("    string endPoint=\"{0}\";", endPoint));
            sb.AppendLine("    Dictionary<string, object> headers = new Dictionary<string, object>();");
            sb.AppendLine("    Dictionary<string, object> parameters = new Dictionary<string, object>();");
            sb.AppendLine("    Dictionary<string, object> queryParameters = new Dictionary<string, object>();");
            sb.AppendLine();
            sb.Append(sbHeaders.ToString());
            sb.AppendLine();
            sb.Append(sbParameters.ToString());
            sb.AppendLine();
            sb.Append(sbQueryParameters.ToString());
            sb.AppendLine(String.Format("    var = Restsharp.Method.{0};", method.ToString()));
            if (authentication)
            {
                sb.AppendLine("    var APICall = new GenericAPICall(baseUrl, username, password);");
            }
            else
            {
                sb.AppendLine("    var APICall = new GenericAPICall(baseUrl, null);");
            }
            sb.AppendLine("var json = APICall.Request(method, baseUrl, headers, parameters, queryParameters, \"\");");
            sb.AppendLine("    return result;");
            sb.AppendLine("}");

            return sb.ToString();
        }
    }
}
