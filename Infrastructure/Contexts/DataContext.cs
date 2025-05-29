//using Infrastructure.Entities;
//using Microsoft.EntityFrameworkCore;

//namespace Infrastructure.Contexts;

//public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
//{
//    public DbSet<UserEntity> Users { get; set; } = null!;
//    public DbSet<UserProfileEntity> UserProfiles { get; set; } = null!;
//}
using Infrastructure.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Contexts;

public class DataContext(DbContextOptions<DataContext> options)
    : IdentityDbContext<UserEntity>(options)
{
    public DbSet<UserProfileEntity> UserProfiles { get; set; } = null!;
}
