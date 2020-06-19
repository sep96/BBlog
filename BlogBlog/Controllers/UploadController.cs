using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BlogBlog.Controllers
{
    public class UploadController : Controller
    {
        // GET: Upload  
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult UploadFile()
        {
            return View();
        }
        [HttpPost]
        public ActionResult UploadFiles()
        {

            string fname = "";
            // Checking no of files injected in Request object  
            if (Request.Files.Count > 0)
            {
                try
                {
                    //  Get all files from Request object  
                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        //string path = AppDomain.CurrentDomain.BaseDirectory + "Uploads/";  
                        //string filename = Path.GetFileName(Request.Files[i].FileName);  

                        HttpPostedFileBase file = files[i];
                       

                        // Checking for Internet Explorer  
                        if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                        {
                            string[] testfiles = file.FileName.Split(new char[] { '\\' });
                            fname = testfiles[testfiles.Length - 1];
                        }
                        else
                        {
                            fname = file.FileName;
                        }
                        var counter = 2;
                        // Get the complete folder path and store the file inside it.  
                        fname = Path.Combine(Server.MapPath("~/Upload/"), fname);
                        if (! System.IO.File.Exists(fname))
                        {
                            file.SaveAs(fname);
                        }
                        else
                        {
                            while (System.IO.File.Exists(fname))
                            {
                                fname = Path.Combine(Server.MapPath("~/Upload/"), Path.GetFileNameWithoutExtension(fname) + "(" + counter + ")" + Path.GetExtension(fname));
                                counter++;
                            }
                            file.SaveAs(fname);
                        }
                    } 
                    return Json(Path.GetFileName(fname));
                }
                catch (Exception ex)
                {
                    return Json("Error occurred. Error details: " + ex.Message);
                }
            }
            else
            {
                return Json("No files selected.");
            }
        }
    }
}