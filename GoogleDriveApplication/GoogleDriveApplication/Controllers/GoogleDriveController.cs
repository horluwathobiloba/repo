using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GoogleDriveApi;


namespace GoogleDriveApplication.Controllers
{
    public class GoogleDriveController : Controller
    {
        FileOperations fileOperations;
        public GoogleDriveController()
        {
            fileOperations = new FileOperations();
        }
        // GET: GoogleDrive
        public ActionResult Index()
        {
             
            return View(fileOperations.FetchListOfFiles());
        }
        public void DeleteFile(string fileId)
        {
            fileOperations.DeleteFile(fileId);
            ViewBag.Message = "File Deleted";
        }
    }
}