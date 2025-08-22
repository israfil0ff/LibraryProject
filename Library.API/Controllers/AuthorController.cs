using Library.BLL;
using Library.DBO;
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
    => Ok(ApiResponse.SuccessResponse(_service.GetAll()));

    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
        var author = _service.GetById(id);
        return author == null
            ? NotFound(ApiResponse.FailResponse($"Id={id} üçün məlumat tapılmadı."))
            : Ok(ApiResponse.SuccessResponse(author));
    }

    [HttpPost]
    public IActionResult Add([FromBody] AuthorCreateDto authorDto)
    {
        var id = _service.Add(authorDto);
        return Ok(ApiResponse.SuccessResponse(new { Id = id }, "Author created successfully"));
    }

    [HttpPut]
    public IActionResult Update([FromBody] AuthorUpdateDto authorDto)
    {
        var id = _service.Update(authorDto);
        return id == 0
            ? NotFound(ApiResponse.FailResponse("Author not found"))
            : Ok(ApiResponse.SuccessResponse(new { Id = id }, "Author updated successfully"));
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var success = _service.Delete(id);
        return !success
            ? NotFound(ApiResponse.FailResponse("Author not found"))
            : Ok(ApiResponse.SuccessResponse(success, "Author deleted successfully"));
    }

}
