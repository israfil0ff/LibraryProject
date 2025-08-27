using Library.BLL.Interfaces;
using Library.DBO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Library.BLL.Exceptions;
using Library.Entities.Enums;
using Library.BLL;

namespace Library.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableRateLimiting("per-token")]
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
            if (author == null)
                throw new AppException(ErrorCode.AuthorNotFound);

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
            if (id == 0)
                throw new AppException(ErrorCode.AuthorNotFound);

            return Ok(new { Id = id });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var success = _service.Delete(id);
            if (!success)
                throw new AppException(ErrorCode.AuthorNotFound);

            return Ok(new { Success = true });
        }
    }
}
