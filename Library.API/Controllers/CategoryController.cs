using Library.BLL.Exceptions;
using Library.BLL.Interfaces;
using Library.DBO.Pagination;
using Library.DBO;
using Library.Entities.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

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


    [HttpGet("get-all")]
    public IActionResult GetAll([FromQuery] PaginationRequest request, [FromQuery] string? name)
    {
        var filters = new Dictionary<string, string>();
        if (!string.IsNullOrWhiteSpace(name))
            filters.Add("name", name);

        var categories = _service.GetAll(request, filters);
        return Ok(categories);
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var category = _service.GetById(id)
            ?? throw new AppException(ErrorCode.CategoryNotFound);

        return Ok(category);
    }

    [HttpPost]
    public IActionResult Create([FromBody] CategoryCreateDto dto)
    {
        var id = _service.Add(dto);
        return Ok(new { Id = id });
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] CategoryUpdateDto dto)
    {
        if (id != dto.Id)
            throw new AppException(ErrorCode.InvalidInput, "ID uyğun gəlmir.");

        var updatedId = _service.Update(dto);
        return Ok(new { Id = updatedId });
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var success = _service.Delete(id);
        if (!success)
            throw new AppException(ErrorCode.CategoryNotFound);

        return Ok(new { Success = true });
    }


    [HttpGet("with-books")]
    public IActionResult GetAllWithBooks([FromQuery] PaginationRequest request, [FromQuery] string? name)
    {
        var filters = new Dictionary<string, string>();
        if (!string.IsNullOrWhiteSpace(name))
            filters.Add("name", name);

        var result = _service.GetAllWithBooks(request, filters);
        return Ok(result);
    }
}
