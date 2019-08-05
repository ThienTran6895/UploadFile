using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UploadFile.Models;
using File = UploadFile.Models.File;

namespace UploadFile.Controllers
{
    public class TablesController : Controller
    {
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
        public ActionResult Upload(HttpPostedFileBase files)
        {
            File model = new File();
            List<File> list = new List<File>();
            //DataTable 
            if (files != null)
            {
                var Extension = Path.GetExtension(files.FileName);
                var fileName = "my-file-" + DateTime.Now.ToString("ddMMyyyyHHmmssfff") + Extension;
                string path = Path.Combine(Server.MapPath("~/UploadedFiles"), fileName);
                model.FileUrl = Url.Content(Path.Combine("~/UploadedFiles/", fileName));
                model.FileName = fileName;
            }
                return View();
        }

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
