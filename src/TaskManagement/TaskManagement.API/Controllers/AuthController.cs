using Microsoft.AspNetCore.Mvc;
using TaskManagement.Application.Contracts.Services;
using TaskManagement.Application.DTOs.Auth.Requests;
using TaskManagement.Application.Services;

namespace TaskManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(
        [FromBody] LoginRequest request)
    {
        var response = await _authService.LoginAsync(request);

        if (response == null)
        {
            return Unauthorized(new
            {
                message = "Invalid email or password."
            });
        }

        return Ok(response);
    }

    [HttpGet("test")]
    public IActionResult Test()
    {
        return Ok(new
        {
            message = "Backend is connected successfully!"
        });
    }
}