using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.DBO
{
    public class AdminUploadDTO
    {
        public string TargetUserId { get; set; } 
        public IFormFile File { get; set; }
    }
}
