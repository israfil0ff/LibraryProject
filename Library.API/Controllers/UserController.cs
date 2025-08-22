using Library.DAL.Context;
using Library.DBO;
using Library.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Linq;

[ApiController]
[Route("api/[controller]")]
[EnableRateLimiting("fixed")]
public class UserController : ControllerBase
{
    private readonly LibraryDbContext _context;

    public UserController(LibraryDbContext context)
    {
        _context = context;
    }

    [HttpGet("summary")]
    public IActionResult GetUserSummaries()
    {
        var users = _context.Users
            .Select(u => new UserSummaryDto
            {
                Id = u.Id,
                Nick = u.Nick
            })
            .ToList();

        return Ok(ApiResponse.SuccessResponse(users, "İstifadəçilər siyahısı"));
    }

    [HttpPost("register")]
    public IActionResult Register(UserRegisterDto dto)
    {
        if (_context.Users.Any(u => u.Nick == dto.Nick))
            return BadRequest(ApiResponse.FailResponse("Bu nick artıq mövcuddur."));

        var user = new User
        {
            Nick = dto.Nick,
            Password = dto.Password
        };

        _context.Users.Add(user);
        _context.SaveChanges();

        return Ok(ApiResponse.SuccessResponse(new { user.Id, user.Nick }, "İstifadəçi uğurla yaradıldı."));
    }

    [HttpPost("login")]
    public IActionResult Login(UserLoginDto dto)
    {
        var user = _context.Users.FirstOrDefault(u => u.Nick == dto.Nick && u.Password == dto.Password);
        if (user == null)
            return BadRequest(ApiResponse.FailResponse("Yanlış nick və ya şifrə."));

        return Ok(ApiResponse.SuccessResponse(new
        {
            user.Id,
            user.Nick
        }, $"Xoş gəlmisiniz, {user.Nick}!"));
    }
}
