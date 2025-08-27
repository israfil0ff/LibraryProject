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
        List<CategoryWithBooksDto> GetAllWithBooks(); 
        int Add(CategoryCreateDto dto);
        int Update(CategoryUpdateDto dto);
        bool Delete(int id);
    }
}
