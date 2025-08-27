using Library.DBO;
using System;
using System.Collections.Generic;

namespace Library.BLL
{
    public interface IBookRentalService
    {
        List<BookRentalDto> GetAll();
        BookRentalDto RentBook(BookRentalCreateDto createDto);
        string ReturnBook(BookReturnDto dto);
    }
}
