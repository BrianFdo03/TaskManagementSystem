using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Application.Contracts.Services;
using TaskManagement.Application.DTOs.Common;

namespace TaskManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DashboardController : ControllerBase
{
    private readonly IDashboardService _dashboardService;

    public DashboardController(
        IDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    [HttpGet]
    public async Task<IActionResult> GetDashboard()
    {
        try
        {
            var userIdClaim = User.FindFirst(
                ClaimTypes.NameIdentifier);

            if (userIdClaim == null ||
                !int.TryParse(userIdClaim.Value, out var userId))
            {
                return Unauthorized(new ApiResponse
                {
                    Success = false,
                    Message = "Unable to identify the authenticated user."
                });
            }

            var isAdmin = User.IsInRole("Admin");

            var dashboard =
                await _dashboardService.GetDashboardAsync(
                    userId,
                    isAdmin);

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Dashboard data retrieved successfully.",
                Data = dashboard
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);

            return StatusCode(500, new ApiResponse
            {
                Success = false,
                Message = "Unable to load dashboard data."
            });
        }
    }
}