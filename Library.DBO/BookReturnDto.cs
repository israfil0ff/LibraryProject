using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.DBO
{
    public class BookReturnDto
    {
        public int RentalId { get; set; }
        public string Nick { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
