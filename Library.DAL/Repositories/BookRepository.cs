using Library.DAL.Context;
using Library.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.DAL.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly LibraryDbContext _context;

        public BookRepository(LibraryDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Book> GetAll(Dictionary<string, string>? filters = null)
        {
            IQueryable<Book> query = _context.Books.Include(b => b.Author).Include(b => b.Category);

            if (filters != null)
            {
                foreach (var filter in filters)
                {
                    if (filter.Key == "title")
                        query = query.Where(b => b.Title.Contains(filter.Value));

                    if (filter.Key == "authorId" && int.TryParse(filter.Value, out int authorId))
                        query = query.Where(b => b.AuthorId == authorId);
                }
            }

            return query.ToList();
        }

        public IQueryable<Book> GetQueryable()
        {
            return _context.Books
                .Include(b => b.Author)
                .Include(b => b.Category);
        }

        public Book? GetById(int id)
        {
            return _context.Books
                .Include(b => b.Author)
                .Include(b => b.Category)
                .FirstOrDefault(b => b.Id == id);
        }

        public void Add(Book book)
        {
            _context.Books.Add(book);
        }

        public void Update(Book book)
        {
            _context.Books.Update(book);
        }

        public void Delete(Book book)
        {
            _context.Books.Remove(book);
        }

        public bool CategoryExists(int categoryId)
        {
            return _context.Categories.Any(c => c.Id == categoryId);
        }

        public void Save()
        {
            _context.SaveChanges();
        }


        public async Task<IEnumerable<Book>> GetAllAsync()
        {
            return await _context.Books
                .Include(b => b.Author)
                .Include(b => b.Category)
                .ToListAsync();
        }
    }
}
