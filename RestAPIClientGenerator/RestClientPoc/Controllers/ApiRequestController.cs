using RestClientPoc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using RestAPIRequest;

namespace RestClientPoc.Controllers
{
    [RoutePrefix("api/request")]
    public class ApiRequestController : ApiController
    {
        //private readonly GenericApiCall _genericApiCall;

        //public ApiRequestController()
        //{
        //    _genericApiCall = new GenericApiCall("http://jsonplaceholder.typicode.com/", "", "");
        //}

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
    }
}
