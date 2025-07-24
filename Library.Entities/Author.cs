using Library.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Entities;
public class Author
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public virtual List<Book> Books { get; set; } = new();
}


