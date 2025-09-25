using Library.BLL.Interfaces;
using Library.DBO.FileDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Library.API.Controllers
{
    [ApiController]
    [Route("api/files")]
    [Authorize] // bütün metodlar üçün login tələb olunsun
    public class FileController : ControllerBase
    {
        private readonly IFileService _fileService;

        public FileController(IFileService fileService)
        {
            _fileService = fileService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")] // yalnız admin bütün faylları görə bilər
        public async Task<IActionResult> GetAll()
        {
            var files = await _fileService.GetAllAsync();
            return Ok(files);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var file = await _fileService.GetByIdAsync(id);
            if (file == null) return NotFound();

            // User yalnız öz faylını görə bilsin
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!User.IsInRole("Admin") && file.UserId != userId)
            {
                return Forbid(); // icazə verilmir
            }

            return Ok(file);
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload([FromForm] FileUploadDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // UserId-ni servise ötürürük
            var file = await _fileService.UploadAsync(dto, userId);
            return CreatedAtAction(nameof(GetById), new { id = file.Id }, file);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var deleted = await _fileService.DeleteAsync(id, userId, User.IsInRole("Admin"));
            if (!deleted) return NotFound();

            return NoContent();
        }
    }
}
