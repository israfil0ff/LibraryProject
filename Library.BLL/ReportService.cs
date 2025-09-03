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

        
        /// Ən çox icarəyə verilən kitabların siyahısı 
       
        public IEnumerable<BookReportDto> GetMostRentedBooks(int top, BookReportFilterDto filter)
        {
            if (top <= 0)
                throw new ArgumentException("Top parametri 0-dan böyük olmalıdır.");

            var query = _context.BookRentals
                .Include(r => r.Book)
                .ThenInclude(b => b.Author)
                .AsQueryable();

            if (filter?.StartDate != null)
                query = query.Where(r => r.StartDate >= filter.StartDate);

            if (filter?.EndDate != null)
                query = query.Where(r => r.StartDate <= filter.EndDate);

            if (filter?.AuthorId != null)
                query = query.Where(r => r.Book.AuthorId == filter.AuthorId);

            if (filter?.CategoryId != null)
                query = query.Where(r => r.Book.CategoryId == filter.CategoryId);

            var result = query
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


        /// Ən çox kitab götürən istifadəçilər 

        public IEnumerable<UserReportDto> GetTopUsers(int top, UserReportFilterDto filter)
        {
            if (top <= 0)
                throw new ArgumentException("Top parametri 0-dan böyük olmalıdır.");

            var query = _context.BookRentals
                .Include(r => r.User)
                .AsQueryable();

            if (filter?.StartDate != null)
                query = query.Where(r => r.StartDate >= filter.StartDate);

            if (filter?.EndDate != null)
                query = query.Where(r => r.StartDate <= filter.EndDate);

            var result = query
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


        /// Gecikmiş kitabların siyahısı 

        public IEnumerable<OverdueReportDto> GetOverdueBooks(OverdueReportFilterDto filter)
        {
            var today = DateTime.UtcNow;

            var query = _context.BookRentals
                .Include(r => r.Book)
                .Include(r => r.User)
                .Where(r => r.EndDate < today && r.ReturnDate == null)
                .AsQueryable();

            if (filter?.UserId != null)
                query = query.Where(r => r.UserId == filter.UserId);

            if (filter?.DueBefore != null)
                query = query.Where(r => r.EndDate <= filter.DueBefore);

            var result = query
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

       
        /// Aylıq icarə statistikası 
       
        public IEnumerable<MonthlyRentalStatDto> GetMonthlyRentalStats(MonthlyRentalFilterDto filter)
        {
            if (filter?.Year == null || filter.Year <= 0)
                throw new ArgumentException("İl düzgün daxil edilməyib.");

            var query = _context.BookRentals
                .Where(r => r.StartDate.Year == filter.Year)
                .AsQueryable();

            if (filter.Month != null)
                query = query.Where(r => r.StartDate.Month == filter.Month);

            var result = query
                .GroupBy(r => r.StartDate.Month)
                .Select(g => new MonthlyRentalStatDto
                {
                    Month = g.Key,
                    TotalRentals = g.Count()
                })
                .OrderBy(x => x.Month)
                .ToList();

            if (!result.Any())
                throw new InvalidOperationException($"{filter.Year}-ci il üçün statistik məlumat tapılmadı.");

            return result;
        }
    }
}
