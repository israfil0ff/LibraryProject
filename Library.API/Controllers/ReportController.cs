using Library.BLL;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Library.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportsController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportsController(IReportService reportService)
        {
            _reportService = reportService;
        }

        /// <summary>
        /// Ən çox icarəyə verilən kitabların siyahısı
        /// </summary>
        [HttpGet("most-rented-books/{top}")]
        public IActionResult GetMostRentedBooks(int top = 10)
        {
            try
            {
                var result = _reportService.GetMostRentedBooks(top);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Ən çox kitab götürən istifadəçilər
        /// </summary>
        [HttpGet("top-users/{top}")]
        public IActionResult GetTopUsers(int top = 10)
        {
            try
            {
                var result = _reportService.GetTopUsers(top);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Gecikmiş kitabların siyahısı
        /// </summary>
        [HttpGet("overdue-books")]
        public IActionResult GetOverdueBooks()
        {
            try
            {
                var result = _reportService.GetOverdueBooks();
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Aylıq icarə statistikası
        /// </summary>
        [HttpGet("monthly-stats/{year}")]
        public IActionResult GetMonthlyRentalStats(int year)
        {
            try
            {
                var result = _reportService.GetMonthlyRentalStats(year);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
