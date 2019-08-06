﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UploadFile.Models
{
    public class File
    {
        public Guid FileID { get; set; }
        public string FileName { get; set; }
        public DateTime UploadDate { get; set; }
        public string UploadPerson { get; set; }
        
    }
}