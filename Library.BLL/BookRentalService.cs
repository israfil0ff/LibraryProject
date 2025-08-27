using AutoMapper;
using AutoMapper.QueryableExtensions;
using Library.DAL.Context;
using Library.DBO;
using Library.Entities;
using Library.Entities.Enums;
using Library.BLL.Exceptions;
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

    public List<BookRentalDto> GetAll()
    {
        var rentals = _context.BookRentals
            .Include(r => r.Book)
            .Include(r => r.User)
            .ToList();

        return _mapper.Map<List<BookRentalDto>>(rentals);
    }

    public BookRentalDto RentBook(BookRentalCreateDto dto)
    {
        var user = _context.Users.FirstOrDefault(u => u.Nick == dto.Nick && u.Password == dto.Password);
        if (user == null)
            throw new AppException(ErrorCode.InvalidCredentials);

        var book = _context.Books.FirstOrDefault(b => b.Id == dto.BookId);
        if (book == null)
            throw new AppException(ErrorCode.BookNotFound);

        if (book.AvailableCount <= 0)
            throw new AppException(ErrorCode.BookNotAvailable);

        bool hasOverdue = _context.BookRentals.Any(r =>
            r.UserId == user.Id && r.ReturnDate == null && r.EndDate < DateTime.Now);

        if (hasOverdue)
            throw new AppException(ErrorCode.HasOverdueRental);

        var rental = new BookRental
        {
            UserId = user.Id,
            BookId = book.Id,
            RentalType = dto.RentalType,
            StartDate = DateTime.Now
        };

        string durationText = dto.RentalType switch
        {
            RentalType.Daily => $"{dto.Quantity} gün",
            RentalType.Weekly => $"{dto.Quantity} həftə",
            RentalType.Monthly => $"{dto.Quantity} ay",
            _ => throw new AppException(ErrorCode.InvalidRentalType)
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

        return rentalDto;
    }

    public string ReturnBook(BookReturnDto dto)
    {
        var user = _context.Users.FirstOrDefault(u => u.Nick == dto.Nick && u.Password == dto.Password);
        if (user == null)
            throw new AppException(ErrorCode.InvalidCredentials);

        var rental = _context.BookRentals
            .Include(r => r.Book)
            .FirstOrDefault(r => r.Id == dto.RentalId && r.UserId == user.Id);

        if (rental == null)
            throw new AppException(ErrorCode.RentalNotFound);

        if (rental.ReturnDate != null)
            throw new AppException(ErrorCode.BookAlreadyReturned);

        if (DateTime.Now < rental.EndDate)
            throw new AppException(ErrorCode.RentalNotExpired);

        rental.ReturnDate = DateTime.Now;
        rental.Book.AvailableCount++;
        rental.Book.RentedCount--;

        _context.SaveChanges();

        return "Kitab uğurla qaytarıldı.";
    }
}
