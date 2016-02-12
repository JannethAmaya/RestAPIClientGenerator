using System;
using System.Collections.Generic;

namespace RestClientPoc.Models
{
    public class RequestViewModel
    {
        public RestAPIRequest.GenericApiCall.HttpVerbs Verb;

        public string ApiUrl { get; set; }

        public string MethodName { get; set; }

        public string ApiKey { get; set; }

        public int Offset { get; set; }

        public int Limit { get; set; }

        public List<ParameterViewModel> Parameters { get; set; }

        public List<ParameterViewModel> Headers { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public RestClientViewModel RestClient { get; set; }

    }

    public class ParameterViewModel
    {
        public string Name { get; set; }

        public string Value { get; set; }
    }

    public class HeaderViewModel
    {
        public string Name { get; set; }

        public string Value { get; set; }
    }

    public class RestClientViewModel
    {
        public string MethodName { get; set; }

        public string Namespace { get; set; }

        public string ResultClassName { get; set; }

    }

}