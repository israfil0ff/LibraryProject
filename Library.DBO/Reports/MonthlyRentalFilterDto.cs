using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.DBO.Reports
{
    public class MonthlyRentalFilterDto
    {
        public int Year { get; set; }
        public int? Month { get; set; }
    }
}
