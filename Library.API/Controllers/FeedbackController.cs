using Library.BLL.Interfaces;
using Library.DBO;
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
            _service.Add(dto);
            return Ok(new { message = "Feedback submitted successfully!" });
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var feedbacks = _service.GetAll();
            return Ok(feedbacks);
        }
    }
}
