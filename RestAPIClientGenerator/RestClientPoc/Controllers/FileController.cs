using System;
using System.Web.Mvc;
using Ionic.Zip;

namespace RestClientPoc.Controllers
{
    public class FileController : Controller
    {
        // GET: File
        [System.Web.Http.HttpGet]
        public FileResult Download()
        {
            const string fileName = "ClassProject.zip";

            // Create file on disk
            using (var zip = new ZipFile())
            {
                zip.AddDirectory(@"C:\Projects\GeneratedClasses");
                zip.Save(String.Format("@C:/Projects/GeneratedClasses/{0}",fileName));
            }

            // Read bytes from disk
            var fileBytes = System.IO.File.ReadAllBytes(String.Format("@C:/Projects/GeneratedClasses/{0}", fileName));
            

            // Return bytes as stream for download
            return File(fileBytes, "application/zip", fileName);
        }
    }
}