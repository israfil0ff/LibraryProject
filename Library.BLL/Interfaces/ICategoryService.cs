using Library.DBO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.BLL.Interfaces
{
    public interface ICategoryService
    {
        List<CategoryDto> GetAll();
        CategoryDto GetById(int id);
        ApiResponse<List<CategoryWithBooksDto>> GetAllWithBooks();
        void Add(CategoryCreateDto dto);
        void Update(CategoryUpdateDto dto);
        void Delete(int id);
    }
}
