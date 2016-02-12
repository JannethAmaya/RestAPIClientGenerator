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
                zip.AddDirectory(Server.MapPath("~/app "));
                zip.Save(Server.MapPath($"~/app/{fileName}"));
            }

            // Read bytes from disk
            var fileBytes = System.IO.File.ReadAllBytes(Server.MapPath($"~/app/{fileName}"));
            

            // Return bytes as stream for download
            return File(fileBytes, "application/zip", fileName);
        }
    }
}