using Library.DBO;
using Library.DBO.Pagination;

namespace Library.BLL
{
    public interface IBookRentalService
    {
        PaginationResponse<BookRentalDto> GetAll(PaginationRequest request);
        BookRentalDto RentBook(BookRentalCreateDto createDto);
        string ReturnBook(BookReturnDto dto);
    }
}
