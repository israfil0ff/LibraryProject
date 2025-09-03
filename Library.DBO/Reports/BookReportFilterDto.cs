using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.DBO.Reports
{
    public class BookReportFilterDto
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? AuthorId { get; set; }
        public int? CategoryId { get; set; }
    }
}
