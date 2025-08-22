using Library.BLL;
using Library.DBO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Library.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[EnableRateLimiting("fixed")]
public class BookController : ControllerBase
{
    private readonly IBookService _service;

    public BookController(IBookService service)
    {
        _service = service;
    }

    [HttpGet]
    public IActionResult GetAll()
        => Ok(ApiResponse.SuccessResponse(_service.GetAll()));

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var book = _service.GetById(id);
        return book == null
            ? NotFound(ApiResponse.FailResponse($"Id={id} üçün məlumat tapılmadı."))
            : Ok(ApiResponse.SuccessResponse(book));
    }

    [HttpPost]
    public IActionResult Add(BookCreateDto bookDto)
    {
        var id = _service.Add(bookDto);
        return Ok(ApiResponse.SuccessResponse(new { Id = id }, "Book created successfully"));
    }

    [HttpPut]
    public IActionResult Update(BookUpdateDto bookDto)
    {
        var id = _service.Update(bookDto);
        return id == 0
            ? NotFound(ApiResponse.FailResponse("Book not found"))
            : Ok(ApiResponse.SuccessResponse(new { Id = id }, "Book updated successfully"));
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var result = _service.Delete(id);
        return !result
            ? NotFound(ApiResponse.FailResponse("Book not found"))
            : Ok(ApiResponse.SuccessResponse(result, "Book deleted successfully"));
    }

    [HttpPost("add-count")]
    public IActionResult AddBookCount([FromBody] AddBookCountDto request)
    {
        if (request.Nick != "admin" || request.Password != "admin")
        {
            return BadRequest(ApiResponse.FailResponse("Yanlış nick və ya password, count əlavə olunmadı."));
        }

        var success = _service.AddCount(request.BookId, request.Count);
        return !success
            ? NotFound(ApiResponse.FailResponse("Kitab tapılmadı."))
            : Ok(ApiResponse.SuccessResponse(null, "Count əlavə olundu."));
    }
}
