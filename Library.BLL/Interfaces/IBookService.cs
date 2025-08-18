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

    List<BookGetDTO> GetAll();
    BookGetDTO GetById(int id);
    int Add(BookCreateDto bookDto);       
    int Update(BookUpdateDto bookDto);    
    bool Delete(int id);
    bool AddCount(int bookId, int count);
    

}