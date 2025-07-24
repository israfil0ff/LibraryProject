using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class BookDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public int AuthorId { get; set; }

    public string? AuthorName { get; set; }
}
