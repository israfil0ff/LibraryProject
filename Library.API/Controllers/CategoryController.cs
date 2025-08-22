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
            => Ok(ApiResponse.SuccessResponse(_service.GetAll()));

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var category = _service.GetById(id);
            return category == null
                ? NotFound(ApiResponse.FailResponse($"Id={id} üçün məlumat tapılmadı."))
                : Ok(ApiResponse.SuccessResponse(category));
        }

        [HttpPost]
        public IActionResult Create(CategoryCreateDto dto)
        {
            try
            {
                var id = _service.Add(dto);
                return Ok(ApiResponse.SuccessResponse(new { Id = id }, "Category created successfully"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse.FailResponse(ex.Message));
            }
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, CategoryUpdateDto dto)
        {
            if (id != dto.Id)
                return BadRequest(ApiResponse.FailResponse("ID uyğun gəlmir."));

            try
            {
                var updatedId = _service.Update(dto);
                return Ok(ApiResponse.SuccessResponse(new { Id = updatedId }, "Category updated successfully"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse.FailResponse(ex.Message));
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var result = _service.Delete(id);
                return result
                    ? Ok(ApiResponse.SuccessResponse(result, "Category deleted successfully"))
                    : NotFound(ApiResponse.FailResponse("Category not found"));
            }
            catch (Exception ex)
            {
                return NotFound(ApiResponse.FailResponse(ex.Message));
            }
        }

        [HttpGet("with-books")]
        public IActionResult GetAllWithBooks()
        {
            var result = _service.GetAllWithBooks();
            return Ok(ApiResponse.SuccessResponse(result));
        }
    }
}
