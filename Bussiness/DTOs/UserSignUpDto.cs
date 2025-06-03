using System.ComponentModel.DataAnnotations;

namespace Business.DTOs;

public class UserSignUpDto
{
    [Required(ErrorMessage = "Name is required.")]
    [MaxLength(30, ErrorMessage = "Name must be at most 30 characters.")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "Surname is required.")]
    [MaxLength(30, ErrorMessage = "Surname must be at most 30 characters.")]
    public string Surname { get; set; } = null!;

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email address.")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Password is required.")]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters.")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;

    [Required(ErrorMessage = "Please confirm your password.")]
    [Compare("Password", ErrorMessage = "Passwords do not match.")]
    [DataType(DataType.Password)]
    public string ConfirmPassword { get; set; } = null!;

    [Required(ErrorMessage = "You must accept the terms and conditions.")]
    public bool AcceptTerms { get; set; } 
}
