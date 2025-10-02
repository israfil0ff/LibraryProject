using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.DBO.HistoryDTOs
{
    public class HistoryReadDTO : HistoryCreateDTO
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
