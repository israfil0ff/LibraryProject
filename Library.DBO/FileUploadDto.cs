using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Library.DBO.FileDTOs
{
    public class FileUploadDto
    {
        [Required]
        public IFormFile File { get; set; } = null!;
    }
}
