using System.ComponentModel.DataAnnotations;

namespace Business.DTOs;

public class UserSignUpDto
{
    [Required]
    [MaxLength(30, ErrorMessage = "Name must be at most 30 characters.")]
    public string Name { get; set; } = null!;

    [Required]
    [MaxLength(30, ErrorMessage = "Surname must be at most 30 characters.")]
    public string Surname { get; set; } = null!;

    [Required]
    [EmailAddress(ErrorMessage = "Invalid email address.")]
    public string Email { get; set; } = null!;

    [Required]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters.")]
    public string Password { get; set; } = null!;

    [Required]
    [Compare("Password", ErrorMessage = "Passwords do not match.")]
    public string ConfirmPassword { get; set; } = null!;

    [Required]
    public bool AcceptTerms { get; set; } 
}
