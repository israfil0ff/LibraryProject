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

        private IActionResult ApiResponse<T>(ApiResponse<T> response)
        {
            return Ok(new
            {
                Success = response.Success,
                Message = response.Message,
                Data = response.Data
            });
        }

        
        [HttpGet]
        public IActionResult GetAll()
        {
            var rentals = _service.GetAll();
            return ApiResponse(rentals);
        }

        
        [HttpPost("rent")]
        public IActionResult RentBook([FromBody] BookRentalCreateDto createDto)
        {
            var result = _service.RentBook(createDto);
            return ApiResponse(result);
        }

        
        [HttpPost("return")]
        public IActionResult ReturnBook([FromBody] BookReturnDto dto)
        {
            var result = _service.ReturnBook(dto);
            return ApiResponse(result);
        }
    }
}
