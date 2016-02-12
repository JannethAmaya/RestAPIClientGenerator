using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace RestClientPoc.Controllers
{
    public class FileController : Controller
    {
        // GET: File
        [System.Web.Http.HttpGet]
        public FileResult Download()
        {
           return null;
        }
    }
}