using Library.BLL;
using Library.BLL.Exceptions;
using Library.BLL.Interfaces;
using Library.DBO;
using Library.Entities.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FeedbackController : ControllerBase
    {
        private readonly IFeedbackService _service;

        public FeedbackController(IFeedbackService service)
        {
            _service = service;
        }

        [HttpPost]
        public IActionResult Add([FromBody] FeedbackCreateDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Comment))
                throw new AppException(ErrorCode.InvalidFeedbackInput, "Feedback mətn boş ola bilməz.");

            _service.Add(dto);
            return Ok(new { Success = true, Message = "Feedback uğurla göndərildi." });
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var feedbacks = _service.GetAll();
            return Ok(feedbacks);
        }
    }
}
