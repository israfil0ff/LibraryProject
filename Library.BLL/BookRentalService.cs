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
                var user = _context.Users.FirstOrDefault(u => u.Nick == createDto.Nick && u.Password == createDto.Password);
                if (user == null)
                    return ApiResponse<BookRentalDto>.FailResponse("Yanlış nick və ya şifrə. İcarə mümkün deyil.");

                var book = _context.Books.FirstOrDefault(b => b.Id == createDto.BookId);
                if (book == null)
                    return ApiResponse<BookRentalDto>.FailResponse("Kitab tapılmadı.");

                if (book.AvailableCount <= 0)
                    return ApiResponse<BookRentalDto>.FailResponse("Kitab mövcud deyil.");

                bool hasOverdueRental = _context.BookRentals.Any(r =>
                    r.UserId == user.Id &&
                    r.ReturnDate == null &&
                    r.EndDate < DateTime.Now);

                if (hasOverdueRental)
                    return ApiResponse<BookRentalDto>.FailResponse("Gecikmiş kitabı qaytarmadan yeni kitab icarə edə bilməzsiniz.");

                var newRental = new BookRental
                {
                    BookId = book.Id,
                    UserId = user.Id,
                    RentalType = createDto.RentalType,
                    StartDate = DateTime.Now
                };

                decimal pricePerUnit = 0;
                string durationText = "";

                
                switch (newRental.RentalType)
                {
                    case RentalType.Daily:
                        newRental.EndDate = newRental.StartDate.AddDays(createDto.Quantity);
                        pricePerUnit = 2;
                        durationText = $"{createDto.Quantity} gün";
                        break;
                    case RentalType.Weekly:
                        newRental.EndDate = newRental.StartDate.AddDays(7 * createDto.Quantity);
                        pricePerUnit = 13;
                        durationText = $"{createDto.Quantity} həftə";
                        break;
                    case RentalType.Monthly:
                        newRental.EndDate = newRental.StartDate.AddMonths(createDto.Quantity);
                        pricePerUnit = 55;
                        durationText = $"{createDto.Quantity} ay";
                        break;
                    default:
                        return ApiResponse<BookRentalDto>.FailResponse("Yanlış kirayə tipi");
                }

                newRental.Price = pricePerUnit * createDto.Quantity;

                
                book.AvailableCount--;
                book.RentedCount++;

                _context.BookRentals.Add(newRental);
                _context.SaveChanges();

                
                var dto = _mapper.Map<BookRentalDto>(newRental);
                dto.DurationText = durationText;

                return ApiResponse<BookRentalDto>.SuccessResponse("Kitab uğurla icarəyə verildi.", dto);
            }
            catch (Exception ex)
            {
                return ApiResponse<BookRentalDto>.FailResponse("Xəta baş verdi: " + ex.Message);
            }
        }

        
        public ApiResponse<string> ReturnBook(BookReturnDto dto)
        {
            try
            {
                var user = _context.Users.FirstOrDefault(u => u.Nick == dto.Nick && u.Password == dto.Password);
                if (user == null)
                    return ApiResponse<string>.SuccessResponse("Yanlış nick və ya şifrə.");

                var rental = _context.BookRentals
                    .Include(r => r.Book)
                    .FirstOrDefault(r => r.Id == dto.RentalId && r.UserId == user.Id);

                if (rental == null)
                    return ApiResponse<string>.SuccessResponse("Rental tapılmadı və ya bu istifadəçiyə aid deyil.");

                if (DateTime.Now < rental.EndDate)
                    return ApiResponse<string>.SuccessResponse("Kirayə müddəti hələ bitməyib, qaytarmaq mümkün deyil.");

                if (rental.ReturnDate != null)
                    return ApiResponse<string>.SuccessResponse("Kitab artıq qaytarılıb.");

                rental.ReturnDate = DateTime.Now;
                rental.Book.AvailableCount++;
                rental.Book.RentedCount--;

                _context.SaveChanges();

                return ApiResponse<string>.SuccessResponse("Kitab uğurla qaytarıldı.");
            }
            catch (Exception ex)
            {
                return ApiResponse<string>.FailResponse("Xəta baş verdi: " + ex.Message);
            }
        }
    }

    
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
