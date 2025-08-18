using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Entities
{
    public class User : AuditableEntity
    {
        public int Id { get; set; }
        public string Nick { get; set; }
        public string Password { get; set; }
    }
}
