using AutoMapper;
using Library.DAL.Context;
using Library.DBO;
using Library.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Library.BLL
{
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
                .Include(b => b.Category)
                .ToList();

            return _mapper.Map<List<BookDto>>(books);
        }

       
        public BookDto GetById(int id)
        {
            var book = _context.Books
                .Include(b => b.Author)
                .Include(b => b.Category)
                .FirstOrDefault(b => b.Id == id);

            return _mapper.Map<BookDto>(book);
        }

       
        public void Add(BookCreateDto bookDto)
        {
            if (bookDto.CategoryId != null && !_context.Categories.Any(c => c.Id == bookDto.CategoryId && !c.IsDeleted))
                throw new Exception("Category tapılmadı.");

            var book = _mapper.Map<Book>(bookDto);
            _context.Books.Add(book);
            _context.SaveChanges();
        }

        
        public void Update(BookUpdateDto bookDto)
        {
            var book = _context.Books.FirstOrDefault(b => b.Id == bookDto.Id);
            if (book == null) throw new Exception("Book tapılmadı.");

            if (bookDto.CategoryId != null && !_context.Categories.Any(c => c.Id == bookDto.CategoryId && !c.IsDeleted))
                throw new Exception("Category tapılmadı.");

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

        
        public bool AddCount(int bookId, int count)
        {
            var book = _context.Books.FirstOrDefault(b => b.Id == bookId);
            if (book == null) return false;

            book.AvailableCount += count;
            _context.Books.Update(book);
            _context.SaveChanges();
            return true;
        }
    }
}
