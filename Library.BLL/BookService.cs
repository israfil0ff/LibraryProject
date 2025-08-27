using AutoMapper;
using Library.DAL.Context;
using Library.DBO;
using Library.Entities;
using Library.Entities.Enums;
using Library.BLL.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Library.BLL;

public class BookService : IBookService
{
    private readonly LibraryDbContext _context;
    private readonly IMapper _mapper;

    public BookService(LibraryDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public List<BookGetDTO> GetAll()
        => _context.Books
            .Include(b => b.Author)
            .Include(b => b.Category)
            .ToList()
            .Select(b => _mapper.Map<BookGetDTO>(b))
            .ToList();

    public BookGetDTO? GetById(int id)
    {
        var book = _context.Books
            .Include(b => b.Author)
            .Include(b => b.Category)
            .FirstOrDefault(b => b.Id == id);

        return book == null ? null : _mapper.Map<BookGetDTO>(book);
    }

    public int Add(BookCreateDto dto)
    {
        if (dto.CategoryId != null && !_context.Categories.Any(c => c.Id == dto.CategoryId && !c.IsDeleted))
            throw new AppException(ErrorCode.CategoryNotFound);

        var book = _mapper.Map<Book>(dto);
        _context.Books.Add(book);
        _context.SaveChanges();

        return book.Id;
    }

    public int Update(BookUpdateDto dto)
    {
        var book = _context.Books.FirstOrDefault(b => b.Id == dto.Id)
            ?? throw new AppException(ErrorCode.BookNotFound);

        if (dto.CategoryId != null && !_context.Categories.Any(c => c.Id == dto.CategoryId && !c.IsDeleted))
            throw new AppException(ErrorCode.CategoryNotFound);

        _mapper.Map(dto, book);
        _context.SaveChanges();

        return book.Id;
    }

    public bool Delete(int id)
    {
        var book = _context.Books.FirstOrDefault(b => b.Id == id);
        if (book == null)
            throw new AppException(ErrorCode.BookNotFound);

        _context.Books.Remove(book);
        _context.SaveChanges();

        return true;
    }

    public bool AddCount(int bookId, int count)
    {
        var book = _context.Books.FirstOrDefault(b => b.Id == bookId)
            ?? throw new AppException(ErrorCode.BookNotFound);

        book.AvailableCount += count;
        _context.SaveChanges();

        return true;
    }
}
