using System.ComponentModel.DataAnnotations;

namespace Business.DTOs;

public class UserProfileUpdateDto
{
    [Required]
    public string Name { get; set; } = null!;

    [Required]
    public string Surname { get; set; } = null!;

    [Phone]
    public string? PhoneNumber { get; set; }

    [Required]
    public DateTime DateOfBirth { get; set; }

    public string? Country { get; set; }
    public string? City { get; set; }
}

