using System.Collections.Generic;
using Library.DBO;
using Library.DBO.Pagination;
using Library.Entities;

namespace Library.BLL
{
    public interface IBookService
    {
        Task<IEnumerable<BookGetDTO>> GetAllBooksAsync();
        Task<IEnumerable<BookGetDTO>> GetBooksByTopicAsync(string topic);
        PaginationResponse<BookGetDTO> GetAll(PaginationRequest request, Dictionary<string, string>? filters = null);

        BookGetDTO GetById(int id);
        int Add(BookCreateDto bookDto);
        int Update(BookUpdateDto bookDto);
        bool Delete(int id);
        bool AddCount(int bookId, int count);
    }
}
