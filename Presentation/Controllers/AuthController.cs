﻿using Business.DTOs;
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

    //[HttpGet("profile")]
    //[Authorize]
    //public async Task<IActionResult> GetProfile()
    //{
    //    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
    //    if (userId == null)
    //        return Unauthorized();

    //    var user = await _userManager.FindByIdAsync(userId);
    //    if (user == null || user.Profile == null)
    //        return NotFound("User profile not found.");

    //    return Ok(new
    //    {
    //        user.Profile.Name,
    //        user.Profile.Surname,
    //        user.Profile.PhoneNumber,
    //        user.Profile.DateOfBirth,
    //        user.Profile.Country,
    //        user.Profile.City
    //    });
    //}

    //[HttpPut("profile")]
    //[Authorize]
    //public async Task<IActionResult> UpdateProfile([FromBody] UserProfileUpdateDto dto)
    //{
    //    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
    //    if (userId == null)
    //        return Unauthorized();

    //    var success = await _authService.UpdateUserProfileAsync(userId, dto);
    //    return success ? Ok("Profile updated.") : BadRequest("Failed to update profile.");
    //}

    //[HttpPost("external-login-callback")]
    //public async Task<IActionResult> ExternalLoginCallback()
    //{
    //    var info = await HttpContext.AuthenticateAsync("ExternalScheme"); 
    //    var result = await _authService.ExternalLoginCallbackAsync(info?.Principal as ExternalLoginInfo);
    //    return result.Success ? Ok("External login successful.") : BadRequest(result.ErrorMessage);
    //}
}
