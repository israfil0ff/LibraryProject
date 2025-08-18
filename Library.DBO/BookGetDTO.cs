using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class BookGetDTO
{
    public int Id { get; set; }
    public string Title { get; set; }

    public bool isDeleted { get; set; }

    public int AuthorId { get; set; }

    public string? AuthorName { get; set; }
    public int? CategoryId { get; set; }
    public string CategoryName { get; set; }
    public int AvailableCount { get; set; }
}
