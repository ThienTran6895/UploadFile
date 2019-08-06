using System;
using System.Collections;
using System.Collections.Generic;

using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using WebAPI_DB.Models;
using WebAPI_DB.Models.Upload;
using WebAPI_DB.Services;

namespace WebAPI_DB.Controllers
{
    public class FilesController : Controller
    {
        private static bool UpdateDatabase = false;

        

        [DataContract]
        public class ChunkMetaData
        {
            [DataMember(Name = "uploadUid")]
            public string UploadUid { get; set; }
            [DataMember(Name = "fileName")]
            public string FileName { get; set; }
            [DataMember(Name = "contentType")]
            public string ContentType { get; set; }
            [DataMember(Name = "chunkIndex")]
            public long ChunkIndex { get; set; }
            [DataMember(Name = "totalChunks")]
            public long TotalChunks { get; set; }
            [DataMember(Name = "totalFileSize")]
            public long TotalFileSize { get; set; }
        }
        public class FileResult
        {
            public bool uploaded { get; set; }
            public string fileUid { get; set; }
        }

        private readonly dataContext _context;


        public FilesController(dataContext context)
        {
            _context = context;
        }


       


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Login>>> GetUsers()
        {

            return await _context.Login.ToListAsync();
        }

        public IEnumerable<Login> Read()
        {
            return GetAll();
        }

        public IList<Login> GetAll()
        {
            using (var _context = new dataContext())
            {
                var result = HttpContext.Session.GetObjectFromJson<IList<Login>>("Data");
                if (result == null || UpdateDatabase)
                {
                    result = _context.Login.ToList();
                    HttpContext.Session.SetObjectAsJson("Data", result);
                }
                return result;
            }
        }

        public IList<Login> Create(Login login)
        {
            if (!UpdateDatabase)
            {
                var users = GetAll();
                var first = users.OrderByDescending(e => e.Id).FirstOrDefault();
                var id = (first != null) ? first.Id : 0;

                login.Id = id + 1;

                users.Insert(0, login);

                HttpContext.Session.SetObjectAsJson("Data", users);
            }
            else
            {
                using (var _context = new dataContext())
                {
                    var entity = new Login();

                    entity.Id = login.Id;
                    entity.Username = login.Username;
                    _context.Login.Add(entity);
                    _context.SaveChanges();

                    login.Id = (int)entity.Id;
                }
            }
            return HttpContext.Session.GetObjectFromJson<IList<Login>>("Data");
        }


        public void Update(Login login)
        {
            if (!UpdateDatabase)
            {
                var users = GetAll();
                var target = users.FirstOrDefault(e => e.Id == login.Id);

                if (target != null)
                {
                    target.Id = login.Id;
                    target.Username = login.Username;

                }
                HttpContext.Session.SetObjectAsJson("Data", users);
            }
            else
            {
                using (var _context = new dataContext())
                {
                    var entity = new Login();

                    entity.Id = login.Id;
                    entity.Username = login.Username;
                    _context.Login.Attach(entity);
                    _context.Entry(entity).State = EntityState.Modified;
                    _context.SaveChanges();
                }
            }
        }


        public IList<Login> Destroy(Login login)
        {
            if (!UpdateDatabase)
            {
                var users = GetAll();
                var target = users.FirstOrDefault(e => e.Id == login.Id);

                if (target != null)
                {
                    users.Remove(target);
                }

                HttpContext.Session.SetObjectAsJson("Data", users);               
            }
            else
            {
                using (var _context = new dataContext())
                {
                    var entity = new Login();

                    entity.Id = login.Id;

                    _context.Login.Attach(entity);

                    _context.Login.Remove(entity);

                    _context.SaveChanges();
                }
            }
            return HttpContext.Session.GetObjectFromJson<IList<Login>>("Data");
        }







        // GET: api/Files
        [HttpGet]
        public ActionResult<IEnumerable<File>> GetFile()
        {
            var result = _context.File.OrderBy(file => file.FileName).ToList().Select(file =>
            {
                var user = _context.User.First(c => file.UserId == c.Id);

                return new File
                {
                    Id = file.Id,
                    FileName = file.FileName,
                    ContentType = file.ContentType,
                    Content = file.Content,
                    Day = file.Day,
                    User = new User
                    {
                        Name = user.Name
                    }

                };
            }).ToList();
            return result;
        }


        [HttpPost]
        public ActionResult UploadFiles(List<IFormFile> files)
        {
            //if (files == null || files.Count == 0)
            //    return Content("files not selected");

            foreach (var file in files)
            {

                var path = System.IO.Path.Combine(
                        System.IO.Directory.GetCurrentDirectory(), "wwwroot\\FileUpload",
                        file.GetFilename());

                using (var stream = new System.IO.FileStream(path, System.IO.FileMode.Create))
                {
                    file.CopyToAsync(stream);
                }
            }
            //return Content("");            
            //return Redirect("http://localhost:63669/Home.html");
            return RedirectToAction("Files");
        }

        [HttpPost]
        public ActionResult ChunkSave(List<IFormFile> files, string metaData)
        {
            var users = GetAll();
            var test = _context.User.Select(s => s.Username).ToList();
            foreach (var item in test)
            {
                if (users.Any(n => n.Username.Equals(item)))
                {
                    var id = users.Select(n => n.Id).FirstOrDefault();
                    HttpContext.Session.SetInt32("id", id);
                }
            }
            


            int i = 1;
            if (metaData == null)
            {
                return UploadFiles(files);
            }
            System.IO.MemoryStream ms = new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(metaData));
            var serializer = new DataContractJsonSerializer(typeof(ChunkMetaData));
            ChunkMetaData somemetaData = serializer.ReadObject(ms) as ChunkMetaData;
            string path = String.Empty;

            if (files != null)
            {
                path = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "wwwroot\\FileUpload", somemetaData.FileName);
                //var ext = System.IO.Path.GetExtension(path).ToLowerInvariant();
                foreach (var file in files)
                {
                    using (var stream = new System.IO.FileStream(path, System.IO.FileMode.Create))
                    {
                        file.CopyToAsync(stream);
                    }


                }
            }
            var count = _context.File.Count();
            i = i + count;
            //var user = _context.User.ToListAsync();
            
            if (!_context.File.ToList().Any(n => n.FileName.Equals(somemetaData.FileName)))
            {
                var entity = new File();
                entity.Id = i;
                entity.FileName = somemetaData.FileName;
                entity.ContentType = System.IO.Path.GetExtension(path).ToLowerInvariant();

                entity.Day = DateTime.Now.ToLocalTime();


                //var id = _context.Login.Select(n => n.Id).FirstOrDefault();


                //var login = HttpContext.Session.GetObjectFromJson<IList<Login>>("Data");
                //var id = login.Select(n => n.Id).FirstOrDefault();
                var id = HttpContext.Session.GetInt32("id");
                entity.UserId = id;


                _context.File.Add(entity);
            }
            else
            {
                //entity.Id = i;
                //entity.FileName = somemetaData.FileName;
                //entity.ContentType = System.IO.Path.GetExtension(path).ToLowerInvariant();
                //entity.Day = DateTime.Now.ToLocalTime();
                var id = _context.Login.Select(n => n.Id).FirstOrDefault();
                //entity.UserId = id;
                _context.Update(entity: new File { UserId = id});
            }

            _context.SaveChangesAsync();
            FileResult fileBlob = new FileResult();
            fileBlob.uploaded = somemetaData.TotalChunks - 1 <= somemetaData.ChunkIndex;
            fileBlob.fileUid = somemetaData.UploadUid;
            return Json(fileBlob);
            //return NoContent();
        }


        public async Task<ActionResult<IEnumerable<File>>> DeleteFile(string id)
        {
            //List<FileDetails> fileInfo = new List<FileDetails>();                     

            var fileInfo = await _context.File.ToListAsync();

            string[] value = id.Split(",");
            foreach (string str in value)
            {
                int idx = Int32.Parse(str);

                File file = fileInfo.Find(delegate (File Files)
                {
                    return Files.Id == idx;

                });
                List<string> name = fileInfo.ToList().Where(p => p.Id == idx).Select(p => p.FileName).ToList();

                foreach (var item in name)
                {
                    string path = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "wwwroot\\FileUpload", item);
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }
                }

                var data = await _context.File.FindAsync(idx);
                _context.File.Remove(data);
                await _context.SaveChangesAsync();
            }
            return fileInfo;
        }


        public async Task<ActionResult<IEnumerable<File>>> EditFile(int id, string name)
        {
            var fileInfo = await _context.File.ToListAsync();

            File file = fileInfo.Find(delegate (File Files)
            {
                return Files.Id == id;

            });
            //List<string> info = fileInfo.ToList().Where(p => p.Id == id).Select(p => p.Name).ToList();
            var info = fileInfo.Select(p => p).Where(p => p.Id == id);

            foreach (var item in info)
            {
                string path = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "wwwroot\\FileUpload", item.FileName);
                string path1 = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "wwwroot\\FileUpload", name);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Copy(path, path1);
                    System.IO.File.Delete(path);
                    DateTime dt = System.IO.File.GetLastWriteTime(path1);
                    System.IO.File.SetLastWriteTime(path1, DateTime.Now);
                    dt = System.IO.File.GetLastWriteTime(path1);
                    item.Day = dt;
                    item.FileName = name;
                    _context.Update(item);
                    await _context.SaveChangesAsync();
                }
            }
            return fileInfo;
        }

        


    }
}
