using Library.BLL;
using Library.DBO;
using Library.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthorController : ControllerBase
{
    private readonly IAuthorService _service;

    public AuthorController(IAuthorService service)
    {
        _service = service;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var authors = _service.GetAll();
        return Ok(authors);
    }

    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
        var author = _service.GetById(id);
        if (author == null) return NotFound();
        return Ok(author);
    }

    [HttpPost]
    public IActionResult Add([FromBody] DBO.AuthorCreateDto authorDto)
    {
        _service.Add(authorDto);
        return Ok();
    }

    [HttpPut]
    public IActionResult Update([FromBody] AuthorUpdateDto authorDto)
    {
        _service.Update(authorDto);
        return Ok();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        _service.Delete(id);
        return Ok();
    }
}


