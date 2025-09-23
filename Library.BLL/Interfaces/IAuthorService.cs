using System.Collections.Generic;
using Library.DBO;
using Library.DBO.Pagination;

namespace Library.BLL
{
    public interface IAuthorService
    {
        
        PaginationResponse<AuthorGetDTO> GetAll(PaginationRequest request, Dictionary<string, string>? filters = null);

        AuthorGetDTO GetById(int id);
        int Add(AuthorCreateDto authorDto);
        int Update(AuthorUpdateDto authorDto);
        bool Delete(int id);
        Task<IEnumerable<AuthorGetDTO>> GetAllAuthorsAsync();
    }
}
