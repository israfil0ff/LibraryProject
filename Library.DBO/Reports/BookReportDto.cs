using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.DBO.Reports
{
    public class BookReportDto
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public int RentalCount { get; set; }
    }
}
