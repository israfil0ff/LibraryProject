using Microsoft.AspNetCore.Mvc;
using Library.BLL;
using Library.DBO;
using Microsoft.AspNetCore.RateLimiting;

namespace Library.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableRateLimiting("fixed")]
    public class BookRentalController : ControllerBase
    {
        private readonly IBookRentalService _service;

        public BookRentalController(IBookRentalService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var rentals = _service.GetAll();
            return Ok(ApiResponse.SuccessResponse(rentals));
        }

        [HttpPost("rent")]
        public IActionResult RentBook([FromBody] BookRentalCreateDto createDto)
        {
            var result = _service.RentBook(createDto);

            return result.Success
                ? Ok(ApiResponse.SuccessResponse(result.Data, result.Message ?? "Book rented successfully"))
                : BadRequest(ApiResponse.FailResponse(result.Message ?? "Failed to rent book"));
        }

        [HttpPost("return")]
        public IActionResult ReturnBook([FromBody] BookReturnDto dto)
        {
            var result = _service.ReturnBook(dto);

            return result.Success
                ? Ok(ApiResponse.SuccessResponse(result.Data, result.Message ?? "Book returned successfully"))
                : BadRequest(ApiResponse.FailResponse(result.Message ?? "Failed to return book"));
        }
    }
}
