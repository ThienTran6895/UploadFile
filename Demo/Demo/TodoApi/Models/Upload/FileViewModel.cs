using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models.Upload
{
    public class FileDetails
    {
        public long Id { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string Day { get; set; }
        public string Area { get; set; }
        public string Person { get; set; }
        public string Path { get; set; }
    }

    public class FileViewModel
    {
        public List<FileDetails> Files { get; set; }
    = new List<FileDetails>();
    }


}
