using Microsoft.AspNetCore.Mvc;
using TaskManagement.Application.Contracts.Services;
using TaskManagement.Application.DTOs.Auth.Requests;
using TaskManagement.Application.DTOs.Common;

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
        try
        {
            if (string.IsNullOrWhiteSpace(request.Email) ||
                string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = "Email and password are required."
                });
            }

            var response = await _authService.LoginAsync(request);

            if (response == null)
            {
                return Unauthorized(new ApiResponse
                {
                    Success = false,
                    Message = "Invalid email or password."
                });
            }

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Login successful.",
                Data = response
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);

            return StatusCode(500, new ApiResponse
            {
                Success = false,
                Message = "An unexpected error occurred while processing the login request."
            });
        }
    }
}