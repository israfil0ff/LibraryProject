using Library.BLL.Interfaces;
using Library.DBO.HistoryDTOs;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HistoryController : ControllerBase
    {
        private readonly IHistoryService _historyService;

        public HistoryController(IHistoryService historyService)
        {
            _historyService = historyService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var histories = _historyService.GetAll();
            return Ok(histories);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var history = _historyService.GetById(id);
            if (history == null)
                return NotFound();

            return Ok(history);
        }
    }
}
