using Library.Entities;
using System.Text.Json.Serialization;

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public bool IsDeleted { get; set; } = false;

    public int AuthorId { get; set; }

    [JsonIgnore] 
    public Author? Author { get; set; }
}
