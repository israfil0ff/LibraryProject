using Library.DBO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.BLL
{
    public interface IBookRentalService
    {
        ApiResponse<List<BookRentalDto>> GetAll();
        ApiResponse<BookRentalDto> RentBook(BookRentalCreateDto createDto);
    }
}
