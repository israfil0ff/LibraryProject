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
    {
        var books = _service.GetAll();
        return Ok(books);
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var book = _service.GetById(id);
        if (book == null)
        {
            return Ok(new { Message = $"Id={id} üçün məlumat tapılmadı." });
        }
        return Ok(book);
    }

    [HttpPost]
    public IActionResult Add(BookCreateDto bookDto)
    {
        var id = _service.Add(bookDto);
        return Ok(new { Id = id });
    }

    [HttpPut]
    public IActionResult Update(BookUpdateDto bookDto)
    {
        var id = _service.Update(bookDto);
        if (id == 0) return NotFound();
        return Ok(new { Id = id });
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var result = _service.Delete(id);
        if (!result) return NotFound();
        return Ok(new { Success = result });
    }
    [HttpPost("add-count")]
    public IActionResult AddBookCount([FromBody] AddBookCountDto request)
    {
        if (request.Nick != "admin" || request.Password != "admin")
        {
            return Ok(new { message = "Yanlış nick və ya password, count əlavə olunmadı." });
        }

        var success = _service.AddCount(request.BookId, request.Count);
        if (!success)
            return NotFound(new { message = "Kitab tapılmadı." });

        return Ok(new { message = "Count əlavə olundu." });
    }

}
