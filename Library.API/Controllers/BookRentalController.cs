using Microsoft.AspNetCore.Mvc;
using Library.BLL;
using Library.DBO;

namespace Library.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookRentalController : ControllerBase
    {
        private readonly IBookRentalService _service;

        public BookRentalController(IBookRentalService service)
        {
            _service = service;
        }

        private IActionResult ApiResponse(bool success, string message, object? data = null)
        {
            return Ok(new
            {
                Success = success,
                Message = message,
                Data = data
            });
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var rentals = _service.GetAll();
            return ApiResponse(true, "Kitab icarələri siyahısı", rentals);
        }

        [HttpPost]
        public IActionResult RentBook([FromBody] BookRentalCreateDto createDto)
        {
            try
            {
                var result = _service.RentBook(createDto);

                if (result == null)
                    return ApiResponse(false, "İcarə yaradıla bilmədi.");

                return ApiResponse(true, "Kitab uğurla icarəyə verildi.", result);
            }
            catch (Exception ex)
            {
               
                return ApiResponse(false, "Xəta baş verdi: " + ex.Message);
            }
        }
    }
}
