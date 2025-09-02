using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.DBO.Reports
{
    public class UserReportDto
    {
        public int UserId { get; set; }
        public string FullName { get; set; }
        public int TotalRentals { get; set; }
    }
}
