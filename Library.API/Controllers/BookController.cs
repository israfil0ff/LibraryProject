using Library.BLL;
using Library.BLL.Exceptions;
using Library.DBO;
using Library.DBO.Pagination;
using Library.Entities.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Library.API.Controllers
{
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


        [HttpGet("get-all")]
        public IActionResult GetAll([FromQuery] PaginationRequest request, [FromQuery] string? title, [FromQuery] string? authorName)
        {
            var filters = new Dictionary<string, string>();
            if (!string.IsNullOrWhiteSpace(title))
                filters.Add("title", title);
            if (!string.IsNullOrWhiteSpace(authorName))
                filters.Add("authorName", authorName);

            var books = _service.GetAll(request, filters);
            return Ok(books);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var book = _service.GetById(id)
                ?? throw new AppException(ErrorCode.BookNotFound);

            return Ok(book);
        }

        [HttpPost]
        public IActionResult Add([FromBody] BookCreateDto bookDto)
        {
            var id = _service.Add(bookDto);
            return Ok(new { Id = id });
        }

        [HttpPut]
        public IActionResult Update([FromBody] BookUpdateDto bookDto)
        {
            var id = _service.Update(bookDto);
            if (id == 0)
                throw new AppException(ErrorCode.BookNotFound);

            return Ok(new { Id = id });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var success = _service.Delete(id);
            if (!success)
                throw new AppException(ErrorCode.BookNotFound);

            return Ok(new { Success = true });
        }

        [HttpPost("add-count")]
        public IActionResult AddBookCount([FromBody] AddBookCountDto request)
        {
            if (request.Nick != "admin" || request.Password != "admin")
                throw new AppException(ErrorCode.InvalidCredentials);

            var success = _service.AddCount(request.BookId, request.Count);
            if (!success)
                throw new AppException(ErrorCode.BookNotFound);

            return Ok(new { Message = "Count əlavə olundu." });
        }
    }
}
