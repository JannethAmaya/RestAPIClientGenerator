using RestClientPoc.Models;
using System;
using System.Collections.Generic;
using System.Web.Http;
using RestAPIRequest;
using Xamasoft.JsonClassGenerator;
using Xamasoft.JsonClassGenerator.CodeWriters;

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
            var url = new Uri(request.Endpoint);
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

    }

}
