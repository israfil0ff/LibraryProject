using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library.Entities;

namespace Library.DAL.Repositories
{
    public interface IBookRepository
    {
        
        IEnumerable<Book> GetAll(Dictionary<string, string>? filters = null);
        Book? GetById(int id);
        void Add(Book book);
        void Update(Book book);
        void Delete(Book book);
        bool CategoryExists(int categoryId);
        IQueryable<Book> GetQueryable();
        void Save();

        
        Task<IEnumerable<Book>> GetAllAsync();
    }
}
