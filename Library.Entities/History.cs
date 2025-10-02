using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Entities
{
    public class History
    {
        public int Id { get; set; }
        public string EntityName { get; set; } 
        public int? EntityId { get; set; } 
        public string Action { get; set; } 
        public string? OldValue { get; set; } 
        public string? NewValue { get; set; } 
        public string Status { get; set; } 
        public string? Message { get; set; } 
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string? CreatedBy { get; set; } 
    }
}

