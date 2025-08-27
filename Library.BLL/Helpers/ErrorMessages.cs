using Library.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.BLL.Helpers
{
    public static class ErrorMessages
    {
        private static readonly Dictionary<ErrorCode, string> _messages = new()
        {
            // Book
            { ErrorCode.BookNotFound, "Kitab tapılmadı." },
            { ErrorCode.InvalidBookInput, "Yanlış kitab məlumatı daxil edilib." },
            { ErrorCode.BookNotAvailable, "Kitab mövcud deyil." },
            { ErrorCode.InvalidRentalType, "Yanlış kirayə tipi seçilib." },

            // Author
            { ErrorCode.AuthorNotFound, "Müəllif tapılmadı." },
            { ErrorCode.InvalidAuthorInput, "Yanlış müəllif məlumatı daxil edilib." },

            // Category
            { ErrorCode.CategoryNotFound, "Kateqoriya tapılmadı." },
            { ErrorCode.InvalidCategoryInput, "Yanlış kateqoriya məlumatı daxil edilib." },

            // BookRental
            { ErrorCode.HasOverdueRental, "Gecikmiş kitabı qaytarmadan yeni kitab icarə edə bilməzsiniz." },
            { ErrorCode.RentalNotFound, "Kirayə məlumatı tapılmadı." },
            { ErrorCode.BookAlreadyReturned, "Kitab artıq qaytarılıb." },
            { ErrorCode.RentalNotExpired, "Kirayə müddəti hələ bitməyib." },

            // Feedback
            { ErrorCode.FeedbackNotFound, "Rəy tapılmadı." },
            { ErrorCode.InvalidFeedbackInput, "Yanlış rəy məlumatı daxil edilib." },

            // User
            { ErrorCode.InvalidCredentials, "Yanlış nick və ya şifrə." },

            // Generic
            { ErrorCode.InvalidInput, "Daxil edilən məlumat yanlışdır." },
            { ErrorCode.DatabaseError, "Verilənlər bazası xətası baş verdi." }
        };

        public static string GetMessage(ErrorCode code)
        {
            return _messages.TryGetValue(code, out var message)
                ? message
                : "Naməlum xəta baş verdi.";
        }
    }
}
