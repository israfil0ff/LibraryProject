using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.DBO
{
    public class BookUpdateDto
    {
        public int Id { get; set; }           
        public string Title { get; set; }     
        public int AuthorId { get; set; }
        public int? CategoryId { get; set; }
    }
}
