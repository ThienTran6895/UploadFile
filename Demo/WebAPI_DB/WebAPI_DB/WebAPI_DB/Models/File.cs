using System;
using System.Collections.Generic;

namespace WebAPI_DB.Models
{
    public partial class File
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public int? UserId { get; set; }
        public byte[] Content { get; set; }
        public DateTime? Day { get; set; }

        public User User { get; set; }       
        
    }

    
}
