using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models.Upload
{
    public class FileInputModel
    {
        public IFormFile FileToUpLoad { get; set; }
    }
}
