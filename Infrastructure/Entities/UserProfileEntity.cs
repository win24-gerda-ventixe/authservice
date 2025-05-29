using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Entities;

public class UserProfileEntity
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public string Name { get; set; } = null!;

    [Required]
    public string Surname { get; set; } = null!;

    public string? PhoneNumber { get; set; }

    [Required]
    public DateTime DateOfBirth { get; set; }

    public string? Country { get; set; }
    public string? City { get; set; }

    [ForeignKey("User")]
    public string UserId { get; set; } = null!;
    public UserEntity User { get; set; } = null!;
}
