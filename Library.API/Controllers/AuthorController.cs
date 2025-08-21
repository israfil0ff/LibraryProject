using Library.BLL;
using Library.DBO;
using Library.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Library.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[EnableRateLimiting("per-token")]
public class AuthorController : ControllerBase
{
    private readonly IAuthorService _service;
    private readonly ILogger<AuthorController> _logger;

    public AuthorController(IAuthorService service, ILogger<AuthorController> logger)
    {
        _service = service;
        _logger = logger;
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
        if (author == null)
        {
            return Ok(new { Message = $"Id={id} üçün m?lumat tap?lmad?." });
        }
        return Ok(author);
    }

    [HttpPost]
    public IActionResult Add([FromBody] AuthorCreateDto authorDto)
    {
        var id = _service.Add(authorDto);
        return Ok(new { Id = id }); 
    }

    [HttpPut]
    public IActionResult Update([FromBody] AuthorUpdateDto authorDto)
    {
        var id = _service.Update(authorDto);
        if (id == 0) return NotFound(new { Message = "Author not found" });

        return Ok(new { Id = id }); 
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var success = _service.Delete(id);
        if (!success) return NotFound(new { Message = "Author not found" });

        return Ok(new { Success = success }); 
    }

}
