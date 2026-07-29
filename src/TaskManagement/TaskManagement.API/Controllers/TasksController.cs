using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Application.Contracts.Services;
using TaskManagement.Application.DTOs.Tasks.Requests;

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

        var task = await _taskService.GetByIdAsync(
            id,
            userId.Value);

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

        var task = await _taskService.CreateAsync(
            request,
            userId.Value);

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

        var updated = await _taskService.UpdateAsync(
            id,
            request,
            userId.Value);

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

        var updated = await _taskService.UpdateStatusAsync(
            id,
            status,
            userId.Value);

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

        var deleted = await _taskService.DeleteAsync(
            id,
            userId.Value);

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
}