using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Entities
{
    public class ApiLog
    {
        public int Id { get; set; }
        public string Endpoint { get; set; }
        public string RequestBody { get; set; }
        public string ResponseBody { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
