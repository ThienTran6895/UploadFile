using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.Net.Http.Headers;
using WebAPI.Models;
using WebAPI.Models.Upload;

namespace WebAPI.Controllers
{

    public class UploadController : Controller
    {
        List<FileDetails> fileInfo = new List<FileDetails>();


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


        private readonly IFileProvider fileProvider;

        public UploadController(IFileProvider fileProvider)
        {
            this.fileProvider = fileProvider;
        }


        [HttpPost]
        public async Task<IActionResult> UploadFiles(List<IFormFile> files)
        {
            if (files == null || files.Count == 0)
                return Content("files not selected");

            foreach (var file in files)
            {
                var path = Path.Combine(
                        Directory.GetCurrentDirectory(), "wwwroot\\FileUpload",
                        file.GetFilename());

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
            }
            //return Content("");            
            //return Redirect("http://localhost:63669/Home.html");
            return RedirectToAction("Files");
        }

        [HttpPost]
        public async Task<IActionResult> ChunkSave(List<IFormFile> files, string metaData)
        {
            if (metaData == null)
            {
                return await UploadFiles(files);
            }
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(metaData));
            var serializer = new DataContractJsonSerializer(typeof(ChunkMetaData));
            ChunkMetaData somemetaData = serializer.ReadObject(ms) as ChunkMetaData;
            string path = String.Empty;
            if (files != null)
            {
                foreach (var file in files)
                {
                    path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\FileUpload", somemetaData.FileName);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                }
            }
            FileResult fileBlob = new FileResult();
            fileBlob.uploaded = somemetaData.TotalChunks - 1 <= somemetaData.ChunkIndex;
            fileBlob.fileUid = somemetaData.UploadUid;
            //return RedirectToAction("Files");
            return Json(fileBlob);
            //return Redirect("http://localhost:63669/Home.html");
        }


        //public IActionResult Files()
        //{
        //    var model = new FileViewModel();
        //    foreach (var item in this.fileProvider.GetDirectoryContents("FileUpload"))
        //    {
        //        model.Files.Add(
        //            new FileDetails { Name = item.Name, Path = item.PhysicalPath });
        //    }
        //    return View(model);           
        //}
        [HttpGet]
        //public IEnumerable<FileDetails> GetFile(IEnumerable<IFormFile> files)
        public IEnumerable<FileDetails> GetFile()
        {
            int i = 1;

            foreach (var item in this.fileProvider.GetDirectoryContents("FileUpload"))
            {
                string path = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\FileUpload", item.Name);
                var ext = Path.GetExtension(path).ToLowerInvariant();
                fileInfo.Add(
                new FileDetails
                {
                    Id = i,
                    Name = item.Name,
                    Day = item.LastModified.LocalDateTime.ToString(),
                    Type = ext
                });
                i = i + 1;
            }
            return fileInfo;
        }


        public IEnumerable<FileDetails> DeleteFile(string id)
        {
            //List<FileDetails> fileInfo = new List<FileDetails>();                     

            GetFile();

            string[] value = id.Split(",");
            foreach (string str in value)
            {
                int idx = Int32.Parse(str);

                FileDetails file = fileInfo.Find(delegate (FileDetails Files)
                {
                    return Files.Id == idx;

                });
                List<string> name = fileInfo.ToList().Where(p => p.Id == idx).Select(p => p.Name).ToList();

                foreach (var item in name)
                {
                    string path = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\FileUpload", item);
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);

                    }
                }
            }
            return fileInfo;
        }

        //[Route("api/[controller]/[action]/[id?]/[name]")]
        public IEnumerable<FileDetails> EditFile(int id, string name)
        {
            GetFile();

            FileDetails file = fileInfo.Find(delegate (FileDetails Files)
            {
                return Files.Id == id;

            });
            //List<string> info = fileInfo.ToList().Where(p => p.Id == id).Select(p => p.Name).ToList();
            var info = fileInfo.Select(p => p).Where(p => p.Id == id);

            foreach (var item in info)
            {
                string path = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\FileUpload", item.Name);
                string path1 = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\FileUpload", name);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Copy(path, path1);
                    System.IO.File.Delete(path);
                    DateTime dt = System.IO.File.GetLastWriteTime(path1);
                    System.IO.File.SetLastWriteTime(path1, DateTime.Now);
                    dt = System.IO.File.GetLastWriteTime(path1);
                    item.Day = dt.ToString();
                }                               
            }

            return fileInfo;

        }

        public async Task<IActionResult> Download(long id)
        {
            GetFile();
            //string[] value = id.Split(",");
            //foreach (string str in value)
            //{
            //    int idx = Int32.Parse(str);

            //    FileDetails file = fileInfo.Find(delegate (FileDetails Files)
            //    {
            //        return Files.Id == idx;

            //    });
            //    List<string> info = fileInfo.ToList().Where(p => p.Id == idx).Select(p => p.Name).ToList();
            //    foreach (var item in info)
            //    {
            //        string path = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\FileUpload", item);
            //        var memory = new MemoryStream();
            //        using (var stream = new FileStream(path, FileMode.Open))
            //        {
            //            await stream.CopyToAsync(memory);
            //        }
            //        memory.Position = 0;
            //        return File(memory, GetContentType(path), Path.GetFileName(path));
            //    }
            //}
            string info = fileInfo.ToList().Where(p => p.Id == id).Select(p => p.Name).FirstOrDefault();
            string path = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\FileUpload", info);
            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, GetContentType(path), Path.GetFileName(path));
        }

        private string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }

        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"}
            };
        }
    }
}