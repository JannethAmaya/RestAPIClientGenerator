using RestSharp;
using System.Web.Mvc;
using RestAPIRequest;

namespace RestClientPoc.Controllers
{
    public class RequestsController : Controller
    {
        private readonly GenericApiCall _genericApiCall;

        public RequestsController()
        {
            _genericApiCall = new GenericApiCall("http://jsonplaceholder.typicode.com/", "","");
        }

        [System.Web.Http.HttpGet]
        public JsonResult Index()
        {
            var jsonResult = _genericApiCall.Request(Method.GET, "/posts", null, null,null, null);
            return Json(jsonResult,"content/json");
        }
    }
}