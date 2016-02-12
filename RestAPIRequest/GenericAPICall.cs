using System.Collections.Generic;
using RestSharp;
using RestSharp.Authenticators;
using Newtonsoft.Json;

namespace RestAPIRequest
{
    public class GenericApiCall
    {
        private readonly string _baseUrl;
        private readonly IAuthenticator _authenticator;

        public GenericApiCall(string baseUrl, IAuthenticator authenticator)
        {
            _baseUrl = baseUrl;
            _authenticator = authenticator;
        }
        public GenericApiCall(string baseUrl, string username, string password)
        {
            _baseUrl = baseUrl;
            _authenticator = new HttpBasicAuthenticator(username, password);
        }
        public string Request(Method method, string endPoint, Dictionary<string,object> headers, Dictionary<string, object> parameters, Dictionary<string, object> queryParameters, string body) 
        {
            var client = new RestClient(_baseUrl);
            var request = new RestRequest(endPoint, method);
            client.Authenticator = _authenticator;
            foreach (var key in headers.Keys)
            { 
                if(headers[key].GetType().ToString().StartsWith("System.Collections.Generics.List"))
                {
                    request.AddHeader(key,JsonConvert.SerializeObject(headers[key]));
                }
                else
                {
                    request.AddHeader(key,headers[key].ToString());
                }
            }
            foreach (var key in parameters.Keys)
            {
                request.AddParameter(key, parameters[key]);
            }
            foreach (var key in queryParameters.Keys)
            {
                if (headers[key].GetType().ToString().StartsWith("System.Collections.Generics.List"))
                {
                    request.AddQueryParameter(key, JsonConvert.SerializeObject(queryParameters[key]));
                }
                else
                {
                    request.AddQueryParameter(key, queryParameters[key].ToString());
                }
            }
            var response = client.Execute(request);
            return response.Content;
        }
    }
}
