using Library.BLL.Interfaces;
using Library.DBO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Library.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableRateLimiting("fixed")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _service;

        public CategoriesController(ICategoryService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_service.GetAll());
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var category = _service.GetById(id);
            if (category == null)
                return NotFound();

            return Ok(category);
        }

        [HttpPost]
        public IActionResult Create(CategoryCreateDto dto)
        {
            try
            {
                _service.Add(dto);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, CategoryUpdateDto dto)
        {
            if (id != dto.Id)
                return BadRequest("ID uyğun gəlmir.");

            try
            {
                _service.Update(dto);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                _service.Delete(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        [HttpGet("with-books")]
        public IActionResult GetAllWithBooks()
        {
            
            var result = _service.GetAllWithBooks();
            return Ok(result);
        }
    }
}
