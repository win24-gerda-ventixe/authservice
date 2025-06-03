using Business.DTOs;

namespace Business.Services;

public interface IAuthService
{
    //Task<bool> LoginAsync(UserSignInDto loginDto);
    Task<(bool Success, string? Token)> LoginAsync(UserSignInDto loginDto);
    Task<bool> AdminLoginAsync(AdminLogInDto loginDto);
    //Task<bool> UserSignUpAsync(UserSignUpDto signUpDto);
    Task<(bool Success, string? ErrorMessage)> UserSignUpAsync(UserSignUpDto dto);
    Task<bool> LogoutAsync();
    Task<bool> UpdateUserProfileAsync(string userId, UserProfileUpdateDto dto);
}

