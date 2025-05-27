using Business.DTOs;
using Infrastructure.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Business.Services;

public class AuthService(
    UserManager<UserEntity> userManager,
    SignInManager<UserEntity> signInManager) : IAuthService
{
    private readonly UserManager<UserEntity> _userManager = userManager;
    private readonly SignInManager<UserEntity> _signInManager = signInManager;

    //public async Task<bool> LoginAsync(UserSignInDto loginDto)
    //{
    //    var user = await _userManager.FindByEmailAsync(loginDto.Email);
    //    if (user == null) return false;

    //    var result = await _signInManager.PasswordSignInAsync(
    //        user,
    //        loginDto.Password,
    //        loginDto.RememberMe,
    //        lockoutOnFailure: false
    //    );

    //    return result.Succeeded;
    //}

    public async Task<(bool Success, string? Token)> LoginAsync(UserSignInDto loginDto)
    {
        var user = await _userManager.FindByEmailAsync(loginDto.Email);
        if (user == null) return (false, null);

        var result = await _signInManager.PasswordSignInAsync(
            user,
            loginDto.Password,
            loginDto.RememberMe,
            lockoutOnFailure: false
        );

        if (!result.Succeeded) return (false, null);

        var token = GenerateJwtToken(user);
        return (true, token);
    }


    public async Task<bool> AdminLoginAsync(AdminLogInDto loginDto)
    {
        var user = await _userManager.FindByEmailAsync(loginDto.Email);
        if (user == null) return false;

        if (!await _userManager.IsInRoleAsync(user, "Admin")) return false;

        var result = await _signInManager.PasswordSignInAsync(
            user,
            loginDto.Password,
            loginDto.RememberMe,
            lockoutOnFailure: false
        );

        return result.Succeeded;
    }
    public async Task<(bool Success, string? ErrorMessage)> UserSignUpAsync(UserSignUpDto dto)
    {
        var existingUser = await _userManager.FindByEmailAsync(dto.Email);
        if (existingUser != null)
            return (false, "Email already exists.");

        if (!dto.AcceptTerms)
            return (false, "Terms must be accepted.");

        var user = new UserEntity
        {
            Email = dto.Email,
            UserName = dto.Email,
            Name = dto.Name,
            Surname = dto.Surname
        };

        var result = await _userManager.CreateAsync(user, dto.Password);
        if (!result.Succeeded)
        {
            var errorMessage = string.Join(", ", result.Errors.Select(e => e.Description));
            return (false, errorMessage);
        }

        await _userManager.AddToRoleAsync(user, "User");
        return (true, null);
    }


    //public async Task<bool> UserSignUpAsync(UserSignUpDto dto)
    //{
    //    var existingUser = await _userManager.FindByEmailAsync(dto.Email);
    //    if (existingUser != null || !dto.AcceptTerms)
    //        return false;

    //    var user = new UserEntity
    //    {
    //        Email = dto.Email,
    //        UserName = dto.Email,
    //        Name = dto.Name,
    //        Surname = dto.Surname
    //    };

    //    var result = await _userManager.CreateAsync(user, dto.Password);
    //    if (!result.Succeeded)
    //        return false;

    //    await _userManager.AddToRoleAsync(user, "User");
    //    return true;
    //}


    public async Task<bool> LogoutAsync()
    {
        await _signInManager.SignOutAsync();
        return true;
    }

    public async Task<(bool Success, string? ErrorMessage)> ExternalLoginCallbackAsync(ExternalLoginInfo info)
    {
        if (info == null)
            return (false, "External login info not found.");

        var result = await _signInManager.ExternalLoginSignInAsync(
            info.LoginProvider,
            info.ProviderKey,
            isPersistent: false,
            bypassTwoFactor: true
        );

        if (result.Succeeded)
            return (true, null); // Successful login

        // Try to register new user
        var email = info.Principal.FindFirstValue(ClaimTypes.Email);
        var firstName = info.Principal.FindFirstValue(ClaimTypes.GivenName) ?? "";
        var lastName = info.Principal.FindFirstValue(ClaimTypes.Surname) ?? "";

        var user = new UserEntity
        {
            Email = email,
            UserName = $"ext_{info.LoginProvider.ToLower()}_{email}",
            Name = firstName,
            Surname = lastName
        };

        var identityResult = await _userManager.CreateAsync(user);
        if (!identityResult.Succeeded)
        {
            var errorMessage = string.Join(", ", identityResult.Errors.Select(e => e.Description));
            return (false, errorMessage);
        }

        await _userManager.AddToRoleAsync(user, "User");
        await _userManager.AddLoginAsync(user, info);
        await _signInManager.SignInAsync(user, isPersistent: false);

        return (true, null); // New user created and signed in
    }

    //public async Task<bool> UpdateUserProfileAsync(string userId, UserProfileUpdateDto dto)
    //{
    //    var user = await _userManager.FindByIdAsync(userId);
    //    if (user == null || user.Profile == null)
    //        return false;

    //    user.Profile.Name = dto.Name;
    //    user.Profile.Surname = dto.Surname;
    //    user.Profile.PhoneNumber = dto.PhoneNumber;
    //    user.Profile.DateOfBirth = dto.DateOfBirth;
    //    user.Profile.Country = dto.Country;
    //    user.Profile.City = dto.City;

    //    var result = await _userManager.UpdateAsync(user);
    //    return result.Succeeded;
    //}


    private string GenerateJwtToken(UserEntity user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes("2K9Z!*r#7pLq8V^@mTzA$Nxw3hJu#ZbD"); 

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim("id", user.Id.ToString()), 
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, "User")
            }),


            Expires = DateTime.UtcNow.AddHours(2),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }


}
