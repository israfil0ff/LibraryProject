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

    private IActionResult ApiResponse(bool success, string message, object? data = null)
    {
        return Ok(new
        {
            Success = success,
            Message = message,
            Data = data
        });
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

        return ApiResponse(true, "İstifadəçilər siyahısı", users);
    }

    [HttpPost("register")]
    public IActionResult Register(UserRegisterDto dto)
    {
        if (_context.Users.Any(u => u.Nick == dto.Nick))
            return ApiResponse(false, "Bu nick artıq mövcuddur.");

        var user = new User
        {
            Nick = dto.Nick,
            Password = dto.Password
        };

        _context.Users.Add(user);
        _context.SaveChanges();

        return ApiResponse(true, "İstifadəçi uğurla yaradıldı.");
    }

    [HttpPost("login")]
    public IActionResult Login(UserLoginDto dto)
    {
        var user = _context.Users.FirstOrDefault(u => u.Nick == dto.Nick && u.Password == dto.Password);
        if (user == null)
            return ApiResponse(false, "Yanlış nick və ya şifrə.");

        return ApiResponse(true, $"Xoş gəlmisiniz, {user.Nick}!", new
        {
            user.Id,
            user.Nick
        });
    }
}
