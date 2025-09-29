using Library.BLL;
using Library.DBO.Reports;
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


        /// Ən çox icarəyə verilən kitabların siyahısı

        [HttpPost("most-rented-books/{top?}")]
        public IActionResult GetMostRentedBooks(int top = 10, [FromBody] BookReportFilterDto filter = null)
        {
            try
            {
                var result = _reportService.GetMostRentedBooks(top, filter);
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


        /// Ən çox kitab götürən istifadəçilər

        [HttpPost("top-users/{top?}")]
        public IActionResult GetTopUsers(int top = 10, [FromBody] UserReportFilterDto filter = null)
        {
            try
            {
                var result = _reportService.GetTopUsers(top, filter);
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


        /// Gecikmiş kitabların siyahısı

        [HttpPost("overdue-books")]
        public IActionResult GetOverdueBooks([FromBody] OverdueReportFilterDto filter = null)
        {
            try
            {
                var result = _reportService.GetOverdueBooks(filter);
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


        /// Aylıq icarə statistikası

        [HttpPost("monthly-stats")]
        public IActionResult GetMonthlyRentalStats([FromBody] MonthlyRentalFilterDto filter)
        {
            try
            {
                var result = _reportService.GetMonthlyRentalStats(filter);
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
