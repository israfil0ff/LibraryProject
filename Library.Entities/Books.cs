using Library.Entities;
using System.Text.Json.Serialization;

public class Book : AuditableEntity
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;

    public bool IsDeleted { get; set; } = false;

    public int AuthorId { get; set; }

    public int AvailableCount { get; set; }
    public int RentedCount { get; set; } = 0; 

    public int? CategoryId { get; set; }  
    public virtual Category Category { get; set; }

    [JsonIgnore] 
    public Author? Author { get; set; }
}
