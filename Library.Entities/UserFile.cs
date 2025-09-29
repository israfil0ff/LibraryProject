using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Entities
{
    public class UserFile
    {
        public int Id { get; set; }

        public string FileName { get; set; }
        public string FilePath { get; set; }


        public string OwnerUserId { get; set; }


        public string UploadedByUserId { get; set; }

        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
    }
}
