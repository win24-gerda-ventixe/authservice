using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Entities;

public class UserEntity : IdentityUser
{
    [Required]
    [MaxLength(100)]
    [ProtectedPersonalData]
    public string Name { get; set; } = null!;

    [Required]
    [MaxLength(100)]
    [ProtectedPersonalData]
    public string Surname { get; set; } = null!;

    //public UserProfileEntity? Profile { get; set; } 

}
