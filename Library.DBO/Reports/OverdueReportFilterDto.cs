using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.DBO.Reports
{
    public class OverdueReportFilterDto
    {
        public int? UserId { get; set; }
        public DateTime? DueBefore { get; set; }
    }
}
