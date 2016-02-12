using RestClientPoc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using RestAPIRequest;
using Xamasoft.JsonClassGenerator;
using Xamasoft.JsonClassGenerator.CodeWriters;
using System.Text;
using WebGrease.Css.Extensions;

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
            var apiCall = new GenericApiCall(String.Format("{0}://{1}",url.Scheme,url.Host), request.UserName, request.Password);

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

            var s = url.Query.Replace("?","").Split('&');
            var queryParameters = s.Select(x => x.Split('=')).ToDictionary<string[], string, object>(value => value[0], value => value[1]);

            var result = apiCall.Request(request.Verb, String.Format("{0}{1}", url.LocalPath,url.Query), headers, parameters, null, string.Empty);
            var classMethod = Request(request.Verb, String.Format("{0}://{1}",url.Scheme,url.Host), String.Format("{0}{1}", url.LocalPath,url.Query), headers, parameters, queryParameters, string.Empty, request.RestClient.MethodName, request.RestClient.Namespace, request.RestClient.ResultClassName, request.Password != null || request.UserName != null, request.UserName, request.Password);
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

        public string Request(GenericApiCall.HttpVerbs method, string baseURL, string endPoint, Dictionary<string, object> headers, Dictionary<string, object> parameters, Dictionary<string, object> queryParameters, string body, string methodName, string nameSpace, string mainClassName, bool authentication, string username, string password)
        {
            //Create method call
            var sb = new StringBuilder("public " + mainClassName + " " + methodName + "(");
            var sbHeaders = new StringBuilder();
            var sbParameters = new StringBuilder();
            var sbQueryParameters = new StringBuilder();
            bool first = true;
            if(headers!=null)
            {
                foreach (var key in headers.Keys)
                {
                    if (!first)
                    {
                        sb.Append(", ");
                    }
                    sb.AppendFormat(" object {0}", key);
                    sbHeaders.AppendLine(String.Format("headers.Add(\"{0}\", {0});", key));
                }
            }
            if(parameters!=null)
            {
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
            }
            if(queryParameters!=null)
            {
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
            sb.AppendLine(String.Format("    return JsonConvert.DeserializeObject<{0}>(json);",mainClassName));
            sb.AppendLine("}");

            return sb.ToString();
        }

    }

}
