using Library.DBO;
using System;
using System.Collections.Generic;

namespace Library.BLL
{
    public interface IBookRentalService
    {
        ApiResponse<List<BookRentalDto>> GetAll();
        ApiResponse<BookRentalDto> RentBook(BookRentalCreateDto createDto);
        ApiResponse<string> ReturnBook(BookReturnDto dto); 
    }
}
