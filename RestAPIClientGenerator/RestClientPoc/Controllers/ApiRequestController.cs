using RestClientPoc.Models;
using System;
using System.Collections.Generic;
using System.Web.Http;
using RestAPIRequest;
using Xamasoft.JsonClassGenerator;
using Xamasoft.JsonClassGenerator.CodeWriters;
using System.Text;

namespace RestClientPoc.Controllers
{
    [RoutePrefix("api/request")]
    public class ApiRequestController : ApiController
    {
        [Route("execute")]
        [HttpPost]
        public IHttpActionResult Execute(RequestViewModel request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var url = new Uri(request.ApiUrl);
            var apiCall = new GenericApiCall($"{url.Scheme}://{url.Host}", request.UserName, request.Password);

            var headers = new Dictionary<string, object>();
            var parameters = new Dictionary<string, object>();

            request.Headers.ForEach(h =>
            {
                if (!headers.ContainsKey(h.Name))
                {
                    headers.Add(h.Name, h.Value);
                }
            });

            request.Parameters.ForEach(p =>
            {
                if (!parameters.ContainsKey(p.Name))
                {
                    parameters.Add(p.Name, p.Value);
                }
            });
            var result = apiCall.Request(request.Verb, $"{url.LocalPath}{url.Query}", headers, parameters, null, string.Empty);
            var classMethod = Request(request.Verb, $"{url.Scheme}://{url.Host}", $"{url.LocalPath}{url.Query}", headers, parameters, null, string.Empty, request.MethodName, string.Empty, string.Empty, request.Password != null || request.UserName != null, request.UserName, request.Password);
            return Ok(result);

        }

        [Route("generate")]
        [HttpPost]
        public IHttpActionResult GenerateClasses([FromBody]string json)
        {
            const string targetFolder = @"C:\Projects\GeneratedClasses";
            var gen = Prepare(json, "txtNameSpaceText", targetFolder, "SampleResponse");
            gen.GenerateClasses();
            
            return Ok();
        }

        private static JsonClassGenerator Prepare(string json, string nameSpace, string targetFolder, string mainClass)
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

        public string Request(RestAPIRequest.GenericApiCall.HttpVerbs method, string baseURL, string endPoint, Dictionary<string, object> headers, Dictionary<string, object> parameters, Dictionary<string, object> queryParameters, string body, string methodName, string nameSpace, string responseType, bool authentication, string username, string password)
        {
            //Create method call
            var sb = new StringBuilder("public " + responseType + " " + methodName + "(");
            var sbHeaders = new StringBuilder();
            var sbParameters = new StringBuilder();
            var sbQueryParameters = new StringBuilder();
            bool first = true;
            foreach (var key in headers.Keys)
            {
                if (!first)
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
            if(queryParameters != null)
            {
                foreach (var key in queryParameters.Keys)
                {
                    if (!first)
                    {
                        sb.Append(", ");
                    }
                    sb.AppendFormat(" object {0}", key);
                    sbQueryParameters.AppendLine(String.Format("queryParameters.Add(\"{0}\", {0});", key));
                }
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
            sb.AppendLine(String.Format("    var method = {0};", method.ToString()));
            if (authentication)
            {
                sb.AppendLine("    var APICall = new GenericAPICall(baseUrl, username, password);");
            }
            else
            {
                sb.AppendLine("    var APICall = new GenericAPICall(baseUrl, null);");
            }
            sb.AppendLine("var json = APICall.Request(method, endPoint, headers, parameters, queryParameters, \"\");");
            sb.AppendLine("    return result;");
            sb.AppendLine("}");

            return sb.ToString();
        }

    }

}
