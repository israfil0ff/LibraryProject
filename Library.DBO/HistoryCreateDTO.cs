using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.DBO.HistoryDTOs
{
    public class HistoryCreateDTO
    {
        public string EntityName { get; set; }
        public int? EntityId { get; set; }
        public string Action { get; set; }
        public string? OldValue { get; set; }
        public string? NewValue { get; set; }
        public string Status { get; set; }
        public string? Message { get; set; }
        public string? CreatedBy { get; set; }
    }
}
