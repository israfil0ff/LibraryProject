using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.DBO
{
    public class CategoryWithBooksDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<BookShortDto> Books { get; set; } = new();
    }
}
