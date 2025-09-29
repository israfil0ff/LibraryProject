using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Entities
{
    public class Feedback
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public string Comment { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
