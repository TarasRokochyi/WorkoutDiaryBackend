using System.Net;
using BLL.DTO;
using BLL.DTO.Identity;
using BLL.Services.Contracts;
using DAL.Models;
using DAL.UOW;
using WorkoutDiary.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WorkoutDiary.Controller;


[ApiController]
[Route("/api/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    
    private IUserService _userService;

    public UserController(ILogger<UserController> logger, IUserService userService)
    {
        _logger = logger;
        _userService = userService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserResponseDTO>>> GetUser()
    {
        var userId = HttpContext.GetUserId();
        var result = await _userService.GetByIdAsync(userId);
        return Ok(result);
    }

    [HttpPut]
    public async Task<ActionResult<UserResponseDTO>> UpdateUser(UserRequestDTO user)
    {
        var userId = HttpContext.GetUserId();
        var result = await _userService.UpdateUserAsync(userId, user);
        return Ok(result);
    }
    
    [HttpPut("update-password")]
    public async Task<IActionResult> UpdatePassword(UpdatePasswordRequest request)
    {
        var userId = HttpContext.GetUserId();
        try
        {
            var result = await _userService.UpdatePasswordAsync(userId, request);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

    }

    [HttpDelete]
    public async Task<ActionResult> DeleteUser()
    {
        var userId = HttpContext.GetUserId();
        await _userService.DeleteUserAsync(userId);
        return NoContent();
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> RegisterAsync(RegisterModel model)
    {
        var result = await _userService.RegisterAsync(model);
        return Ok(result);
    }
    
    
    [HttpPost("token")]
    [AllowAnonymous]
    public async Task<IActionResult> GetTokenAsync(TokenRequestModel model)
    {
        var result = await _userService.GetTokenAsync(model);
        //if(result.IsAuthenticated == true)
            return Ok(result);
        //return BadRequest(result);
    }

    [HttpPost("addrole")]
    public async Task<IActionResult> AddRoleAsync(AddRoleModel model)
    {
        var result = await _userService.AddRoleAsync(model);
        return Ok(result);
    }
    
    [HttpPost("refresh-token")]
    [AllowAnonymous]
    public async Task<IActionResult> RefreshToken(RefreshTokenRequestDTO refreshToken)
    {
        var response = await _userService.RefreshTokenAsync(refreshToken.RefreshToken);
        if (!response.IsAuthenticated)
        {
            return Unauthorized();
        }
        return Ok(response);
    }

    [HttpPost("revoke-token")]
    public async Task<IActionResult> RevokeToken([FromBody] RevokeTokenRequest model)
    {
        // accept token from request body or cookie
        var token = model.Token ?? Request.Cookies["refreshToken"];

        if (string.IsNullOrEmpty(token))
            return BadRequest(new { message = "Token is required" });

        var response = await _userService.RevokeTokenAsync(token);

        if (!response)
            return NotFound(new { message = "Token not found" });

        return Ok(new { message = "Token revoked" });
    }
}
