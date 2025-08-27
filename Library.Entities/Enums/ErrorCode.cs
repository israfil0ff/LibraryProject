using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Entities.Enums
{
    public enum ErrorCode
    {
        // Book
        BookNotFound = 1001,
        InvalidBookInput = 1002,
        BookNotAvailable = 1003,
        InvalidRentalType = 1004,

        // Author
        AuthorNotFound = 2001,
        InvalidAuthorInput = 2002,

        // Category
        CategoryNotFound = 3001,
        InvalidCategoryInput = 3002,

        // BookRental
        HasOverdueRental = 4001,
        RentalNotFound = 4002,
        BookAlreadyReturned = 4003,
        RentalNotExpired = 4004,

        // Feedback
        FeedbackNotFound = 5001,
        InvalidFeedbackInput = 5002,

        // User
        InvalidCredentials = 6001,

        // Generic
        InvalidInput = 9001,
        DatabaseError = 9002

    }

}
