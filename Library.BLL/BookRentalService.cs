using AutoMapper;
using Library.DAL.Context;
using Library.DBO;
using Library.Entities;
using Library.Entities.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Library.BLL
{
    public class BookRentalService : IBookRentalService
    {
        private readonly LibraryDbContext _context;
        private readonly IMapper _mapper;

        public BookRentalService(LibraryDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public ApiResponse<List<BookRentalDto>> GetAll()
        {
            try
            {
                var rentals = _context.BookRentals
                    .Include(r => r.Book)
                    .Include(r => r.User)
                    .ToList();

                var data = _mapper.Map<List<BookRentalDto>>(rentals);
                return ApiResponse<List<BookRentalDto>>.SuccessResponse("Kitab icarələri siyahısı", data);
            }
            catch (Exception ex)
            {
                return ApiResponse<List<BookRentalDto>>.FailResponse("Xəta baş verdi: " + ex.Message);
            }
        }

        public ApiResponse<BookRentalDto> RentBook(BookRentalCreateDto createDto)
        {
            try
            {
                bool alreadyRented = _context.BookRentals.Any(r =>
                    r.BookId == createDto.BookId &&
                    r.UserId == createDto.UserId &&
                    r.ReturnDate == null
                );

                if (alreadyRented)
                    return ApiResponse<BookRentalDto>.FailResponse("Bu kitab artıq sizə icarəyə verilib və hələ qaytarılmayıb.");

                var user = _context.Users
                    .FirstOrDefault(u => u.Nick == createDto.Nick && u.Password == createDto.Password);

                if (user == null)
                    return ApiResponse<BookRentalDto>.FailResponse("Yanlış nick və ya şifrə. İcarə mümkün deyil.");

                var book = _context.Books.FirstOrDefault(b => b.Id == createDto.BookId);

                if (book == null)
                    return ApiResponse<BookRentalDto>.FailResponse($"Book with Id {createDto.BookId} not found.");

                if (book.AvailableCount <= 0)
                    return ApiResponse<BookRentalDto>.FailResponse("Kitab mövcud deyil.");

                bool hasActiveRental = _context.BookRentals.Any(r =>
                    r.BookId == createDto.BookId &&
                    r.UserId == user.Id &&
                    r.ReturnDate == null);

                if (hasActiveRental)
                    return ApiResponse<BookRentalDto>.FailResponse("Eyni kitabı artıq aktiv olaraq icarəyə almısınız.");

                bool hasOverdueRental = _context.BookRentals.Any(r =>
                    r.UserId == user.Id &&
                    r.ReturnDate == null &&
                    r.EndDate < DateTime.Now);

                if (hasOverdueRental)
                    return ApiResponse<BookRentalDto>.FailResponse("Gecikmiş kitabı qaytarmadan yeni kitab icarə edə bilməzsiniz.");

                var newRental = new BookRental
                {
                    BookId = createDto.BookId,
                    RentalType = createDto.RentalType,
                    StartDate = DateTime.Now,
                    UserId = user.Id
                };

                switch (newRental.RentalType)
                {
                    case RentalType.Daily:
                        newRental.EndDate = newRental.StartDate.AddDays(1);
                        newRental.Price = 2;
                        break;
                    case RentalType.Weekly:
                        newRental.EndDate = newRental.StartDate.AddDays(7);
                        newRental.Price = 13;
                        break;
                    case RentalType.Monthly:
                        newRental.EndDate = newRental.StartDate.AddMonths(1);
                        newRental.Price = 55;
                        break;
                    default:
                        return ApiResponse<BookRentalDto>.FailResponse("Invalid rental type");
                }

                book.AvailableCount--;

                _context.BookRentals.Add(newRental);
                _context.SaveChanges();

                var dto = _mapper.Map<BookRentalDto>(newRental);
                return ApiResponse<BookRentalDto>.SuccessResponse("Kitab uğurla icarəyə verildi.", dto);
            }
            catch (Exception ex)
            {
                return ApiResponse<BookRentalDto>.FailResponse("Xəta baş verdi: " + ex.Message);
            }
        }
    }

    // Standart cavab modeli
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T? Data { get; set; }

        public static ApiResponse<T> SuccessResponse(string message, T? data = default) =>
            new ApiResponse<T> { Success = true, Message = message, Data = data };

        public static ApiResponse<T> FailResponse(string message) =>
            new ApiResponse<T> { Success = false, Message = message };
    }
}
