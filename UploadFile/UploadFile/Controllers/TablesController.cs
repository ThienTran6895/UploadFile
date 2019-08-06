using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UploadFile.Models;
using UploadFile.Repository;
using File = UploadFile.Models.File;

namespace UploadFile.Controllers
{
    public class TablesController : Controller
    {
        public FileRepository fileRepository = new FileRepository();
        // GET: Tables
        public ActionResult Index()
        {
            return View();
        }

        // GET: Tables/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Tables/Upload
        [HttpPost]
        public JsonResult Upload(File files)
        {
            for (int i = 0; i < Request.Files.Count; i++)
            {
                HttpPostedFileBase file = Request.Files[i]; //Uploaded file
                //Use the following properties to get file's name, size and MIMEType
                //var fileupload = Request.Files[i];   
                files.FileID = Guid.NewGuid();
                files.FileName = file.FileName;
                //files.UploadDate = DateTime.Now.ToLocalTime();
                files.UploadPerson = "Admin";
                //int fileSize = file.ContentLength;
                //string fileName = file.FileName;
                //string mimeType = file.ContentType;
                //System.IO.Stream fileContent = file.InputStream;
                //To save file, use SaveAs method
                file.SaveAs(Server.MapPath("~/UploadFiles/") + files.FileName); //File will be saved in application root
            }
            var result = fileRepository.AddFile(files);
            return Json(result);
            //return Json("Uploaded " + Request.Files.Count + " files");
        }
        //public ActionResult Upload(HttpPostedFileBase files)
        //{
        //    File model = new File();
        //    List<File> list = new List<File>();
        //    //DataTable 
        //    if (files != null)
        //    {
        //        var Extension = Path.GetExtension(files.FileName);
        //        var fileName = "my-file-" + DateTime.Now.ToString("ddMMyyyyHHmmssfff") + Extension;
        //        string path = Path.Combine(Server.MapPath("~/UploadedFiles"), fileName);
        //        model.FileUrl = Url.Content(Path.Combine("~/UploadedFiles/", fileName));
        //        model.FileName = fileName;
        //    }
        //        return View();
        //}

        // POST: Tables/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Tables/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Tables/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Tables/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Tables/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
