using Library.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace Library.DBO
{
    public class BookRentalCreateDto
    {
        public readonly int UserId;

        [Required]
        public int BookId { get; set; }

        [Required]
        public RentalType RentalType { get; set; }
        public string Nick { get; set; }
        public string Password { get; set; }
    }
}
