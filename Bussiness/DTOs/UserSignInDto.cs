using System.ComponentModel.DataAnnotations;

namespace Business.DTOs;

public class UserSignInDto
{
    [Required(ErrorMessage = "Email field cannot be empty.")]
    [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password field cannot be empty.")]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    public bool RememberMe { get; set; }
}
