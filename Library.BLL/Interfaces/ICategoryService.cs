using Library.DBO;
using Library.DBO.Pagination;
using System.Collections.Generic;

namespace Library.BLL.Interfaces
{
    public interface ICategoryService
    {

        PaginationResponse<CategoryDto> GetAll(PaginationRequest request, Dictionary<string, string>? filters = null);
        PaginationResponse<CategoryWithBooksDto> GetAllWithBooks(PaginationRequest request, Dictionary<string, string>? filters = null);

        CategoryDto GetById(int id);
        int Add(CategoryCreateDto dto);
        int Update(CategoryUpdateDto dto);
        bool Delete(int id);
    }
}
