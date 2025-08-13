using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.DBO
{
    public class AddBookCountDto
    {
        public int BookId { get; set; }
        public int Count { get; set; }
        public string Nick { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}