using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.DBO
{
    public class FeedbackCreateDto
    {
        public string Subject { get; set; }
        public string Comment { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}
