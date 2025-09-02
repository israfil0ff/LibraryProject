using Library.DAL.Context;
using Library.DBO.Reports;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Library.BLL
{
    public class ReportService : IReportService
    {
        private readonly LibraryDbContext _context;

        public ReportService(LibraryDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Ən çox icarəyə verilən kitabların siyahısı
        /// </summary>
        public IEnumerable<BookReportDto> GetMostRentedBooks(int top)
        {
            if (top <= 0)
                throw new ArgumentException("Top parametri 0-dan böyük olmalıdır.");

            var result = _context.BookRentals
                .Include(r => r.Book)
                .GroupBy(r => r.BookId)
                .Select(g => new BookReportDto
                {
                    BookId = g.Key,
                    Title = g.FirstOrDefault().Book.Title,
                    RentalCount = g.Count()
                })
                .OrderByDescending(x => x.RentalCount)
                .Take(top)
                .ToList();

            if (!result.Any())
                throw new InvalidOperationException("Ən çox icarəyə verilən kitab tapılmadı.");

            return result;
        }

        /// <summary>
        /// Ən çox kitab götürən istifadəçilər
        /// </summary>
        public IEnumerable<UserReportDto> GetTopUsers(int top)
        {
            if (top <= 0)
                throw new ArgumentException("Top parametri 0-dan böyük olmalıdır.");

            var result = _context.BookRentals
                .Include(r => r.User)
                .GroupBy(r => r.UserId)
                .Select(g => new UserReportDto
                {
                    UserId = g.Key,
                    FullName = g.FirstOrDefault().User.Nick,
                    TotalRentals = g.Count()
                })
                .OrderByDescending(x => x.TotalRentals)
                .Take(top)
                .ToList();

            if (!result.Any())
                throw new InvalidOperationException("İstifadəçi tapılmadı.");

            return result;
        }

        /// <summary>
        /// Gecikmiş kitabların siyahısı
        /// </summary>
        public IEnumerable<OverdueReportDto> GetOverdueBooks()
        {
            var today = DateTime.UtcNow;

            var result = _context.BookRentals
                .Include(r => r.Book)
                .Include(r => r.User)
                .Where(r => r.EndDate < today && r.ReturnDate == null)
                .Select(r => new OverdueReportDto
                {
                    RentalId = r.Id,
                    BookTitle = r.Book.Title,
                    UserFullName = r.User.Nick,
                    DueDate = r.EndDate,
                    DaysOverdue = EF.Functions.DateDiffDay(r.EndDate, today)
                })
                .ToList();

            if (!result.Any())
                throw new InvalidOperationException("Hazırda gecikmiş kitab yoxdur.");

            return result;
        }

        /// <summary>
        /// Aylıq icarə statistikası
        /// </summary>
        public IEnumerable<MonthlyRentalStatDto> GetMonthlyRentalStats(int year)
        {
            if (year <= 0)
                throw new ArgumentException("İl düzgün daxil edilməyib.");

            var result = _context.BookRentals
                .Where(r => r.StartDate.Year == year)
                .GroupBy(r => r.StartDate.Month)
                .Select(g => new MonthlyRentalStatDto
                {
                    Month = g.Key,
                    TotalRentals = g.Count()
                })
                .OrderBy(x => x.Month)
                .ToList();

            if (!result.Any())
                throw new InvalidOperationException($"{year}-ci il üçün statistik məlumat tapılmadı.");

            return result;
        }
    }
}
