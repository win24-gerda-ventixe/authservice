//using Business.DTOs;
//using Business.Services;
//using Infrastructure.Entities;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using System.Security.Claims;

//namespace Presentation.Controllers;

//[ApiController]
//[Route("api/[controller]")]
//public class AuthController(IAuthService authService, UserManager<UserEntity> userManager) : ControllerBase
//{
//    private readonly IAuthService _authService = authService;
//    private readonly UserManager<UserEntity> _userManager = userManager;

//    //[HttpPost("login")]
//    //public async Task<IActionResult> Login([FromBody] UserSignInDto dto)
//    //{
//    //    var result = await _authService.LoginAsync(dto);
//    //    return result ? Ok("Login successful.") : Unauthorized("Invalid credentials.");
//    //}
//    [HttpPost("login")]
//    public async Task<IActionResult> Login([FromBody] UserSignInDto dto)
//    {
//        try
//        {
//            var (success, token) = await _authService.LoginAsync(dto);
//            if (!success)
//                return Unauthorized(new { message = "Invalid credentials" });

//            return Ok(new { token });
//        }
//        catch (Exception ex)
//        {
//            Console.WriteLine($"Login error: {ex.Message}"); 
//            return StatusCode(500, new { message = "Server error occurred during login" });
//        }
//    }



//    //[HttpPost("register")]
//    //public async Task<IActionResult> Register([FromBody] UserSignUpDto dto)
//    //{
//    //    if (!ModelState.IsValid)
//    //    {
//    //        var errors = ModelState.Values
//    //            .SelectMany(v => v.Errors)
//    //            .Select(e => e.ErrorMessage)
//    //            .ToList();

//    //        return BadRequest(new { message = "Validation failed", errors });
//    //    }

//    //    var result = await _authService.UserSignUpAsync(dto);
//    //    return result ? Ok("Registration successful.") : BadRequest("Could not register user.");
//    //}
//    [HttpPost("register")]
//    public async Task<IActionResult> Register([FromBody] UserSignUpDto dto)
//    {
//        var (success, error) = await _authService.UserSignUpAsync(dto);
//        if (!success)
//            return BadRequest(error); 

//        return Ok();
//    }




//    [HttpPost("logout")]
//    [Authorize]
//    public async Task<IActionResult> Logout()
//    {
//        await _authService.LogoutAsync();
//        return Ok("User logged out.");
//    }

//    //[HttpPost("external-login-callback")]
//    //public async Task<IActionResult> ExternalLoginCallback()
//    //{
//    //    var info = await HttpContext.AuthenticateAsync("ExternalScheme"); // Placeholder
//    //    var result = await _authService.ExternalLoginCallbackAsync(info?.Principal as ExternalLoginInfo);
//    //    return result.Success ? Ok("External login successful.") : BadRequest(result.ErrorMessage);
//    //}

//    //[HttpPut("profile")]
//    //[Authorize]
//    //public async Task<IActionResult> UpdateProfile([FromBody] UserProfileUpdateDto dto)
//    //{
//    //    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
//    //    if (userId == null)
//    //        return Unauthorized();

//    //    var success = await _authService.UpdateUserProfileAsync(userId, dto);
//    //    return success ? Ok("Profile updated.") : BadRequest("Failed to update profile.");
//    //}

//    [Authorize(Roles = "Admin")]
//    [HttpGet("users")]
//    public async Task<IActionResult> GetAllUsers()
//    {
//        var users = await _userManager.Users.ToListAsync();

//        var result = users.Select(user => new
//        {
//            user.Id,
//            user.Email,
//            user.UserName,
//            user.Name,
//            user.Surname
//        });

//        return Ok(result);
//    }

//}


using Business.DTOs;
using Business.Services;
using Infrastructure.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService authService, UserManager<UserEntity> userManager) : ControllerBase
{
    private readonly IAuthService _authService = authService;
    private readonly UserManager<UserEntity> _userManager = userManager;

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserSignInDto dto)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

            return BadRequest(new { message = "Invalid input", errors });
        }

        try
        {
            var (success, token) = await _authService.LoginAsync(dto);
            if (!success)
                return Unauthorized(new { message = "Invalid credentials" });

            return Ok(new { token });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Login ERROR] {ex}");

            return StatusCode(500, new
            {
                message = "Server error occurred during login",
                error = ex.Message,
                stackTrace = ex.StackTrace
            });
        }
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserSignUpDto dto)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

            return BadRequest(new { message = "Validation failed", errors });
        }

        var (success, error) = await _authService.UserSignUpAsync(dto);
        if (!success)
            return BadRequest(new { message = error });

        return Ok(new { message = "Registration successful." });
    }

    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await _authService.LogoutAsync();
        return Ok(new { message = "User logged out." });
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("users")]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _userManager.Users.ToListAsync();

        var result = users.Select(user => new
        {
            user.Id,
            user.Email,
            user.UserName,
            user.Name,
            user.Surname
        });

        return Ok(result);
    }
}
