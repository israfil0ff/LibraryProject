using AutoMapper;
using Library.DAL.Context;
using Library.DBO;
using Library.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    public List<BookDto> GetAll()
    {
        var books = _context.Books
         .Include(b => b.Author) 
         .ToList();

        return _mapper.Map<List<BookDto>>(books);
    }

    public BookDto GetById(int id)
    {
        var book = _context.Books.FirstOrDefault(b => b.Id == id);
        return _mapper.Map<BookDto>(book);
    }

    public void Add(BookCreateDto bookDto)
    {
        var book = _mapper.Map<Book>(bookDto);
        _context.Books.Add(book);
        _context.SaveChanges();
    }

    public void Update(BookUpdateDto bookDto)
    {
        var book = _context.Books.FirstOrDefault(b => b.Id == bookDto.Id);
        if (book == null) return;

        _mapper.Map(bookDto, book);
        _context.SaveChanges();
    }

    public void Delete(int id)
    {
        var book = _context.Books.FirstOrDefault(b => b.Id == id);
        if (book == null) return;

        _context.Books.Remove(book);
        _context.SaveChanges();
    }
}