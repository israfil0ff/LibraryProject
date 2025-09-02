using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.DBO.Reports
{
    public class OverdueReportDto
    {
        public int RentalId { get; set; }
        public string BookTitle { get; set; }
        public string UserFullName { get; set; }
        public DateTime DueDate { get; set; }
        public int DaysOverdue { get; set; }
    }
}
