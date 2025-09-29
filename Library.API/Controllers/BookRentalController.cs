using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Library.BLL;
using Library.DBO;
using Library.DBO.Pagination;

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


        [HttpGet("get-all")]
        public IActionResult GetAll([FromQuery] PaginationRequest request)
        {
            var rentals = _service.GetAll(request);
            return Ok(rentals);
        }

        [HttpPost("rent")]
        public IActionResult RentBook([FromBody] BookRentalCreateDto createDto)
        {
            var rental = _service.RentBook(createDto);
            return Ok(rental);
        }

        [HttpPost("return")]
        public IActionResult ReturnBook([FromBody] BookReturnDto dto)
        {
            var result = _service.ReturnBook(dto);
            return Ok(new { Message = result });
        }
    }
}
