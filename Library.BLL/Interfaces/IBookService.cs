using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.DBO;
using Library.Entities;

namespace Library.BLL;
public interface IBookService
{

    List<BookDto> GetAll();
    BookDto GetById(int id);
    void Add(BookCreateDto bookDto);
    void Update(BookUpdateDto bookDto);
    void Delete(int id);

}