using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Claims;
using TaskManagement.Application.Contracts.Services;
using TaskManagement.Application.DTOs.Tasks.Requests;
using TaskManagement.Domain.Enums;
using TaskManagement.Application.DTOs.Common;

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
        try
        {
            var userId = GetCurrentUserId();

            if (userId == null)
            {
                return Unauthorized(new ApiResponse
                {
                    Success = false,
                    Message = "Unable to identify the authenticated user."
                });
            }

            var tasks = await _taskService.GetMyTasksAsync(userId.Value);

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Tasks retrieved successfully.",
                Data = tasks
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);

            return StatusCode(500, new ApiResponse
            {
                Success = false,
                Message = "Unable to retrieve your tasks."
            });
        }
    }

    // GET: api/tasks/user/{userId}
    [HttpGet("user/{userId:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetTasksByUser(int userId) // Tasks of user by admin
    {
        try
        {
            var tasks = await _taskService.GetTasksByUserAsync(userId);

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "User tasks retrieved successfully.",
                Data = tasks
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);

            return StatusCode(500, new ApiResponse
            {
                Success = false,
                Message = "Unable to retrieve tasks for the selected user."
            });
        }
    }

    // GET: api/tasks/{id}
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var userId = GetCurrentUserId();

            if (userId == null)
            {
                return Unauthorized(new ApiResponse
                {
                    Success = false,
                    Message = "Unable to identify the authenticated user."
                });
            }

            var userRole = GetCurrentUserRole();

            var isAdmin = userRole == UserRole.Admin;

            var task = await _taskService.GetByIdAsync(
                id,
                userId.Value,
                isAdmin);

            if (task == null)
            {
                return NotFound(new ApiResponse
                {
                    Success = false,
                    Message = $"Task not found."
                });
            }

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Task retrieved successfully.",
                Data = task
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);

            return StatusCode(500, new ApiResponse
            {
                Success = false,
                Message = "Unable to retrieve the task."
            });
        }
    }

    // POST: api/tasks
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateTaskRequest request)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.Title))
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = "Task title is required."
                });
            }

            if (request.DueDate == default)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = "Task due date is required."
                });
            }

            var userId = GetCurrentUserId();

            if (userId == null)
            {
                return Unauthorized(new ApiResponse
                {
                    Success = false,
                    Message = "Unable to identify the authenticated user."
                });
            }

            var userRole = GetCurrentUserRole();

            var isAdmin = userRole == UserRole.Admin;

            var task = await _taskService.CreateAsync(
                request,
                userId.Value,
                isAdmin);

            return CreatedAtAction(
                nameof(GetById),
                new { id = task.Id },
                new ApiResponse<object>
                {
                    Success = true,
                    Message = "Task created successfully.",
                    Data = task
                });
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);

            return StatusCode(500, new ApiResponse
            {
                Success = false,
                Message = "Unable to create the task."
            });
        }
    }

    // PUT: api/tasks/{id}
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
        int id,
        [FromBody] UpdateTaskRequest request)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.Title))
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = "Task title is required."
                });
            }

            if (request.DueDate == default)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = "Task due date is required."
                });
            }

            var userId = GetCurrentUserId();

            if (userId == null)
            {
                return Unauthorized(new ApiResponse
                {
                    Success = false,
                    Message = "Unable to identify the authenticated user."
                });
            }

            var userRole = GetCurrentUserRole();

            var isAdmin = userRole == UserRole.Admin;

            var updated = await _taskService.UpdateAsync(
                id,
                request,
                userId.Value,
                isAdmin);

            if (!updated)
            {
                return NotFound(new ApiResponse
                {
                    Success = false,
                    Message = $"Task not found or cannot be modified."
                });
            }

            return Ok(new ApiResponse
            {
                Success = true,
                Message = "Task updated successfully."
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);

            return StatusCode(500, new ApiResponse
            {
                Success = false,
                Message = "Unable to update the task."
            });
        }
    }

    // PATCH: api/tasks/{id}/status
    [HttpPatch("{id:int}/status")]
    public async Task<IActionResult> UpdateStatus(
        int id,
        [FromBody] int status)
    {
        try
        {
            if (!Enum.IsDefined(typeof(TaskStatus), status))
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = "Invalid task status."
                });
            }

            var userId = GetCurrentUserId();

            if (userId == null)
            {
                return Unauthorized(new ApiResponse
                {
                    Success = false,
                    Message = "Unable to identify the authenticated user."
                });
            }

            var userRole = GetCurrentUserRole();

            var isAdmin = userRole == UserRole.Admin;

            var updated = await _taskService.UpdateStatusAsync(
                id,
                status,
                userId.Value,
                isAdmin);

            if (!updated)
            {
                return NotFound(new ApiResponse
                {
                    Success = false,
                    Message = $"Task not found or cannot be modified."
                });
            }

            return Ok(new ApiResponse
            {
                Success = true,
                Message = "Task status updated successfully."
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);

            return StatusCode(500, new ApiResponse
            {
                Success = false,
                Message = "Unable to update the task status."
            });
        }
    }

    // DELETE: api/tasks/{id}
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var userId = GetCurrentUserId();

            if (userId == null)
            {
                return Unauthorized(new ApiResponse
                {
                    Success = false,
                    Message = "Unable to identify the authenticated user."
                });
            }

            var userRole = GetCurrentUserRole();

            var isAdmin = userRole == UserRole.Admin;

            var deleted = await _taskService.DeleteAsync(
                id,
                userId.Value,
                isAdmin);

            if (!deleted)
            {
                return NotFound(new ApiResponse
                {
                    Success = false,
                    Message = $"Task not found or cannot be deleted."
                });
            }

            return Ok(new ApiResponse
            {
                Success = true,
                Message = "Task deleted successfully."
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);

            return StatusCode(500, new ApiResponse
            {
                Success = false,
                Message = "Unable to delete the task."
            });
        }
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