using Library.DAL.Context;
using Library.DBO.Reports;
using Library.DBO.HistoryDTOs;
using Library.BLL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Library.BLL
{
    public class ReportService : IReportService
    {
        private readonly LibraryDbContext _context;
        private readonly IHistoryService _historyService;

        public ReportService(LibraryDbContext context, IHistoryService historyService)
        {
            _context = context;
            _historyService = historyService;
        }

        /// Ən çox icarəyə verilən kitabların siyahısı
        public IEnumerable<BookReportDto> GetMostRentedBooks(int top, BookReportFilterDto filter)
        {
            try
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

                _historyService.AddHistory(new HistoryCreateDTO
                {
                    EntityName = "Report",
                    EntityId = 0,
                    Action = "GetMostRentedBooks",
                    Status = "Success",
                    Message = $"Top {top} rented books report generated."
                });

                return result;
            }
            catch (Exception ex)
            {
                _historyService.AddHistory(new HistoryCreateDTO
                {
                    EntityName = "Report",
                    EntityId = 0,
                    Action = "GetMostRentedBooks",
                    Status = "Error",
                    Message = ex.Message
                });
                throw;
            }
        }

        /// Ən çox kitab götürən istifadəçilər
        public IEnumerable<UserReportDto> GetTopUsers(int top, UserReportFilterDto filter)
        {
            try
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

                _historyService.AddHistory(new HistoryCreateDTO
                {
                    EntityName = "Report",
                    EntityId = 0,
                    Action = "GetTopUsers",
                    Status = "Success",
                    Message = $"Top {top} users report generated."
                });

                return result;
            }
            catch (Exception ex)
            {
                _historyService.AddHistory(new HistoryCreateDTO
                {
                    EntityName = "Report",
                    EntityId = 0,
                    Action = "GetTopUsers",
                    Status = "Error",
                    Message = ex.Message
                });
                throw;
            }
        }

        /// Gecikmiş kitabların siyahısı
        public IEnumerable<OverdueReportDto> GetOverdueBooks(OverdueReportFilterDto filter)
        {
            try
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

                _historyService.AddHistory(new HistoryCreateDTO
                {
                    EntityName = "Report",
                    EntityId = 0,
                    Action = "GetOverdueBooks",
                    Status = "Success",
                    Message = "Overdue books report generated."
                });

                return result;
            }
            catch (Exception ex)
            {
                _historyService.AddHistory(new HistoryCreateDTO
                {
                    EntityName = "Report",
                    EntityId = 0,
                    Action = "GetOverdueBooks",
                    Status = "Error",
                    Message = ex.Message
                });
                throw;
            }
        }

        /// Aylıq icarə statistikası
        public IEnumerable<MonthlyRentalStatDto> GetMonthlyRentalStats(MonthlyRentalFilterDto filter)
        {
            try
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

                _historyService.AddHistory(new HistoryCreateDTO
                {
                    EntityName = "Report",
                    EntityId = 0,
                    Action = "GetMonthlyRentalStats",
                    Status = "Success",
                    Message = $"Monthly rental stats generated for {filter.Year}."
                });

                return result;
            }
            catch (Exception ex)
            {
                _historyService.AddHistory(new HistoryCreateDTO
                {
                    EntityName = "Report",
                    EntityId = 0,
                    Action = "GetMonthlyRentalStats",
                    Status = "Error",
                    Message = ex.Message
                });
                throw;
            }
        }
    }
}
