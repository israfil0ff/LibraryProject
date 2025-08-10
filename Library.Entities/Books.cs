using Library.Entities;
using System.Text.Json.Serialization;

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
<<<<<<< HEAD
    public bool IsDeleted { get; set; } = false;

=======
>>>>>>> df8759a624d03709649affb3bdaa6dd42546dee2
    public int AuthorId { get; set; }

    [JsonIgnore] 
    public Author? Author { get; set; }
}
