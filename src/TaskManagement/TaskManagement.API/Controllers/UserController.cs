using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Application.Contracts.Services;
using TaskManagement.Application.DTOs.Common;
using TaskManagement.Application.DTOs.Users.Requests;
using TaskManagement.Application.Services;

namespace TaskManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    //GET: api/user
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var users = await _userService.GetAllAsync();

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Users retrieved successfully.",
                Data = users
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);

            return StatusCode(500, new ApiResponse
            {
                Success = false,
                Message = "Unable to retrieve users."
            });
        }
    }


    // GET: api/users/5
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var user = await _userService.GetByIdAsync(id);

            if (user == null)
            {
                return NotFound(new ApiResponse
                {
                    Success = false,
                    Message = $"User not found."
                });
            }

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "User retrieved successfully.",
                Data = user
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);

            return StatusCode(500, new ApiResponse
            {
                Success = false,
                Message = "Unable to retrieve the user."
            });
        }
    }

    // POST: api/users
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateUserRequest request)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = "User name is required."
                });
            }

            if (string.IsNullOrWhiteSpace(request.Email))
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = "User email is required."
                });
            }

            if (string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = "User password is required."
                });
            }

            var id = await _userService.CreateAsync(request);

            return CreatedAtAction(
                nameof(GetById),
                new { id },
                new ApiResponse<object>
                {
                    Success = true,
                    Message = "User created successfully.",
                    Data = new { id }
                });
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);

            return StatusCode(500, new ApiResponse
            {
                Success = false,
                Message = "Unable to create the user."
            });
        }
    }

    // PUT: api/users/5
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
        int id,
        [FromBody] UpdateUserRequest request)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = "User name is required."
                });
            }

            if (string.IsNullOrWhiteSpace(request.Email))
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = "User email is required."
                });
            }

            var updated = await _userService.UpdateAsync(id, request);

            if (!updated)
            {
                return NotFound(new ApiResponse
                {
                    Success = false,
                    Message = $"User not found."
                });
            }

            return Ok(new ApiResponse
            {
                Success = true,
                Message = "User updated successfully."
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);

            return StatusCode(500, new ApiResponse
            {
                Success = false,
                Message = "Unable to update the user."
            });
        }
    }

    // DELETE: api/users/5
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var deleted = await _userService.DeleteAsync(id);

            if (!deleted)
            {
                return NotFound(new ApiResponse
                {
                    Success = false,
                    Message = $"User with ID {id} was not found."
                });
            }

            return Ok(new ApiResponse
            {
                Success = true,
                Message = "User deleted successfully."
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);

            return StatusCode(500, new ApiResponse
            {
                Success = false,
                Message = "Unable to delete the user."
            });
        }
    }
}