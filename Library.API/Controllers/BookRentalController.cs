using Microsoft.AspNetCore.Mvc;
using Library.BLL.Interfaces;
using Library.DBO;
using Microsoft.AspNetCore.RateLimiting;
using Library.BLL;

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
            return Ok(result);
        }
    }
}
