using Library.BLL;
using Library.DBO;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers;

[ApiController]
[Route("api/[controller]")]
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
            return NotFound();
        return Ok(book); 
    }

    [HttpPost]
    public IActionResult Add(BookCreateDto bookDto)
    {
        _service.Add(bookDto);
        return Ok();
    }

    [HttpPut]
    public IActionResult Update(BookUpdateDto bookDto)
    {
        _service.Update(bookDto);
        return Ok();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        _service.Delete(id);
        return Ok();
    }
}