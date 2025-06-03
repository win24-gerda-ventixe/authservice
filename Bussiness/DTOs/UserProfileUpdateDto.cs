using System.ComponentModel.DataAnnotations;

namespace Business.DTOs;

public class UserProfileUpdateDto
{
    [Required(ErrorMessage = "Name is required.")]
    [MaxLength(30, ErrorMessage = "Name must be at most 30 characters.")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "Surname is required.")]
    [MaxLength(30, ErrorMessage = "Surname must be at most 30 characters.")]
    public string Surname { get; set; } = null!;

    [Phone(ErrorMessage = "Please enter a valid phone number.")]
    public string? PhoneNumber { get; set; }

    [Required(ErrorMessage = "Date of birth is required.")]
    [DataType(DataType.Date, ErrorMessage = "Invalid date format.")]
    public DateTime DateOfBirth { get; set; }

    [MaxLength(50, ErrorMessage = "Country name must be at most 50 characters.")]
    public string? Country { get; set; }

    [MaxLength(50, ErrorMessage = "City name must be at most 50 characters.")]
    public string? City { get; set; }
}

