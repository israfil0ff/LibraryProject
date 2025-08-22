using AutoMapper;
using AutoMapper.QueryableExtensions;
using Library.DAL.Context;
using Library.DBO;
using Library.Entities;
using Library.Entities.Enums;
using Microsoft.EntityFrameworkCore;

namespace Library.BLL;

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
        var rentals = _context.BookRentals
            .Include(r => r.Book)
            .Include(r => r.User)
            .ToList();

        var data = _mapper.Map<List<BookRentalDto>>(rentals);
        return ApiResponse<List<BookRentalDto>>.SuccessResponse("Kitab icarələri siyahısı", data);
    }

    public ApiResponse<BookRentalDto> RentBook(BookRentalCreateDto dto)
    {
        var user = _context.Users.FirstOrDefault(u => u.Nick == dto.Nick && u.Password == dto.Password);
        if (user == null)
            return ApiResponse<BookRentalDto>.FailResponse("Yanlış nick və ya şifrə.");

        var book = _context.Books.FirstOrDefault(b => b.Id == dto.BookId);
        if (book == null) return ApiResponse<BookRentalDto>.FailResponse("Kitab tapılmadı.");
        if (book.AvailableCount <= 0) return ApiResponse<BookRentalDto>.FailResponse("Kitab mövcud deyil.");

        bool hasOverdue = _context.BookRentals.Any(r =>
            r.UserId == user.Id && r.ReturnDate == null && r.EndDate < DateTime.Now);

        if (hasOverdue) return ApiResponse<BookRentalDto>.FailResponse("Gecikmiş kitabı qaytarmadan yeni kitab icarə edə bilməzsiniz.");

        var rental = new BookRental
        {
            UserId = user.Id,
            BookId = book.Id,
            RentalType = dto.RentalType,
            StartDate = DateTime.Now
        };

        decimal price = 0;
        string durationText = dto.Quantity switch
        {
            _ when dto.RentalType == RentalType.Daily => $"{dto.Quantity} gün",
            _ when dto.RentalType == RentalType.Weekly => $"{dto.Quantity} həftə",
            _ when dto.RentalType == RentalType.Monthly => $"{dto.Quantity} ay",
            _ => throw new ArgumentException("Yanlış kirayə tipi")
        };

        rental.EndDate = dto.RentalType switch
        {
            RentalType.Daily => rental.StartDate.AddDays(dto.Quantity),
            RentalType.Weekly => rental.StartDate.AddDays(7 * dto.Quantity),
            RentalType.Monthly => rental.StartDate.AddMonths(dto.Quantity),
            _ => rental.StartDate
        };

        rental.Price = dto.RentalType switch
        {
            RentalType.Daily => 2 * dto.Quantity,
            RentalType.Weekly => 13 * dto.Quantity,
            RentalType.Monthly => 55 * dto.Quantity,
            _ => 0
        };

        book.AvailableCount--;
        book.RentedCount++;

        _context.BookRentals.Add(rental);
        _context.SaveChanges();

        var rentalDto = _mapper.Map<BookRentalDto>(rental);
        rentalDto.DurationText = durationText;

        return ApiResponse<BookRentalDto>.SuccessResponse("Kitab uğurla icarəyə verildi.", rentalDto);
    }

    public ApiResponse<string> ReturnBook(BookReturnDto dto)
    {
        var user = _context.Users.FirstOrDefault(u => u.Nick == dto.Nick && u.Password == dto.Password);
        if (user == null) return ApiResponse<string>.FailResponse("Yanlış nick və ya şifrə.");

        var rental = _context.BookRentals
            .Include(r => r.Book)
            .FirstOrDefault(r => r.Id == dto.RentalId && r.UserId == user.Id);

        if (rental == null) return ApiResponse<string>.FailResponse("Rental tapılmadı və ya bu istifadəçiyə aid deyil.");
        if (rental.ReturnDate != null) return ApiResponse<string>.FailResponse("Kitab artıq qaytarılıb.");
        if (DateTime.Now < rental.EndDate) return ApiResponse<string>.FailResponse("Kirayə müddəti hələ bitməyib.");

        rental.ReturnDate = DateTime.Now;
        rental.Book.AvailableCount++;
        rental.Book.RentedCount--;

        _context.SaveChanges();

        return ApiResponse<string>.SuccessResponse("Kitab uğurla qaytarıldı.");
    }
}

public class ApiResponse<T>
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }

    public static ApiResponse<T> SuccessResponse(string message, T? data = default)
        => new() { Success = true, Message = message, Data = data };

    public static ApiResponse<T> FailResponse(string message)
        => new() { Success = false, Message = message };
}
