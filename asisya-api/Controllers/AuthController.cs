using Microsoft.AspNetCore.Mvc;
using asisya_api.Services;

namespace asisya_api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly JwtService _jwt;

    public AuthController(JwtService jwt)
    {
        _jwt = jwt;
    }

    [HttpPost("login")]
    public IActionResult Login(LoginDto dto)
    {
        if (dto.Username == "admin" && dto.Password == "1234")
        {
            var token = _jwt.GenerateToken(dto.Username);

            return Ok(new { token });
        }

        return Unauthorized();
    }
}

public class LoginDto
{
    public string Username { get; set; }
    public string Password { get; set; }
}