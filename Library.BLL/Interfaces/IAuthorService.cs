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
    List<AuthorDto> GetAll();
    AuthorDto? GetById(int id);
    void Add(DBO.AuthorCreateDto authorDto);
    void Update(AuthorUpdateDto authorDto);
    void Delete(int id);

}