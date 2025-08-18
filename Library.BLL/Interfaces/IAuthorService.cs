using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.DBO;

using Library.Entities;

namespace Library.BLL;
public interface IAuthorService
{
    List<AuthorGetDTO> GetAll();
    AuthorGetDTO? GetById(int id);
    int Add(DBO.AuthorCreateDto authorDto);
    int Update(AuthorUpdateDto authorDto);
    bool Delete(int id);

}