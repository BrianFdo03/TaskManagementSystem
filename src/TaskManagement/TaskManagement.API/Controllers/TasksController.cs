using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Claims;
using TaskManagement.Application.Contracts.Services;
using TaskManagement.Application.DTOs.Tasks.Requests;
using TaskManagement.Domain.Enums;

namespace TaskManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TasksController : ControllerBase
{
    private readonly ITaskService _taskService;

    public TasksController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    // GET: api/tasks
    [HttpGet]
    public async Task<IActionResult> GetMyTasks() // Tasks of own emp
    {
        var userId = GetCurrentUserId();

        if (userId == null)
            return Unauthorized();

        var tasks = await _taskService.GetMyTasksAsync(userId.Value);

        return Ok(tasks);
    }

    // GET: api/tasks/user/{userId}
    [HttpGet("user/{userId:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetTasksByUser(int userId) // Tasks of user by admin
    {
        var tasks = await _taskService.GetTasksByUserAsync(userId);

        return Ok(tasks);
    }

    // GET: api/tasks/{id}
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var userId = GetCurrentUserId();

        if (userId == null)
            return Unauthorized();

        var userRole = GetCurrentUserRole();

        var isAdmin = userRole == UserRole.Admin;

        var task = await _taskService.GetByIdAsync(
            id,
            userId.Value,
            isAdmin);

        if (task == null)
            return NotFound();

        return Ok(task);
    }

    // POST: api/tasks
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateTaskRequest request)
    {
        var userId = GetCurrentUserId();

        if (userId == null)
            return Unauthorized();

        var userRole = GetCurrentUserRole();

        var isAdmin =
            userRole == UserRole.Admin;

        var task = await _taskService.CreateAsync(
            request,
            userId.Value,
            isAdmin);

        return CreatedAtAction(
            nameof(GetById),
            new { id = task.Id },
            task);
    }

    // PUT: api/tasks/{id}
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
        int id,
        [FromBody] UpdateTaskRequest request)
    {
        var userId = GetCurrentUserId();

        if (userId == null)
            return Unauthorized();

        var userRole = GetCurrentUserRole();

        var isAdmin = userRole == UserRole.Admin;

        var updated = await _taskService.UpdateAsync(
            id,
            request,
            userId.Value, 
            isAdmin);

        if (!updated)
            return NotFound();

        return NoContent();
    }

    // PATCH: api/tasks/{id}/status
    [HttpPatch("{id:int}/status")]
    public async Task<IActionResult> UpdateStatus(
        int id,
        [FromBody] int status)
    {
        var userId = GetCurrentUserId();

        if (userId == null)
            return Unauthorized();

        var userRole = GetCurrentUserRole();

        var isAdmin = userRole == UserRole.Admin;

        var updated = await _taskService.UpdateStatusAsync(
            id,
            status,
            userId.Value,
            isAdmin);

        if (!updated)
            return NotFound();

        return NoContent();
    }

    // DELETE: api/tasks/{id}
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var userId = GetCurrentUserId();

        if (userId == null)
            return Unauthorized();

        var userRole = GetCurrentUserRole();

        var isAdmin = userRole == UserRole.Admin;

        var deleted = await _taskService.DeleteAsync(
            id,
            userId.Value,
            isAdmin);

        if (!deleted)
            return NotFound();

        return NoContent();
    }

    private int? GetCurrentUserId()
    {
        var claim = User.FindFirst(
            ClaimTypes.NameIdentifier);

        if (claim == null)
            return null;

        if (!int.TryParse(
            claim.Value,
            out var userId))
        {
            return null;
        }

        return userId;
    }

    private UserRole? GetCurrentUserRole()
    {
        var claim = User.FindFirst(
            ClaimTypes.Role);

        if (claim == null)
            return null;

        if (Enum.TryParse<UserRole>(
        claim.Value,
        true,
        out var userRole))
        {
            return userRole;
        }

        return null;
    }
}