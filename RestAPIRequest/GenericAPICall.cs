﻿using System.Collections.Generic;
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

        public string Request(HttpVerbs method, string endPoint, Dictionary<string,object> headers, Dictionary<string, object> parameters, Dictionary<string, object> queryParameters, string body) 
        {
            var restSharpMethod = GetRestsharpMethod(method);
            var client = new RestClient(_baseUrl);
            var request = new RestRequest(endPoint, restSharpMethod);
            client.Authenticator = _authenticator;

            //Headers
            if (headers != null)
            {
                foreach (var key in headers.Keys)
                {
                    request.AddHeader(key,
                        headers[key].GetType().ToString().StartsWith("System.Collections.Generics.List")
                            ? JsonConvert.SerializeObject(headers[key])
                            : headers[key].ToString());
                }
            }
            
            //Parameters
            if (parameters != null)
            {
                foreach (var key in parameters.Keys)
                {
                    request.AddParameter(key, parameters[key]);
                }
            }

            //QueryParameters
            if (queryParameters != null && headers != null)
            { 
                foreach (var key in queryParameters.Keys)
                {
                    request.AddQueryParameter(key,
                        headers[key].GetType().ToString().StartsWith("System.Collections.Generics.List")
                            ? JsonConvert.SerializeObject(queryParameters[key])
                            : queryParameters[key].ToString());
                }
            }

            var response = client.Execute(request);
            return response.Content;
        }

        private Method GetRestsharpMethod(HttpVerbs method)
        {
            Method result;

            switch (method)
            {
                case HttpVerbs.Get:
                    result = Method.GET;
                    break;
                case HttpVerbs.Post:
                    result = Method.POST;
                    break;
                case HttpVerbs.Put:
                    result = Method.PUT;
                    break;
                case HttpVerbs.Patch:
                    result = Method.PATCH;
                    break;
                case HttpVerbs.Delete:
                    result = Method.DELETE;
                    break;
                case HttpVerbs.Head:
                    result = Method.HEAD;
                    break;
                default:
                    result = Method.GET;
                    break;
            }

            return result;
        }
        public enum HttpVerbs
        {
            Get = 1,
            Post = 2,
            Put = 3,
            Patch = 4,
            Delete = 5,
            Head = 6
        }
    }
}
