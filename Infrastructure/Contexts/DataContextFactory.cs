using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Contexts;

public class DataContextFactory : IDesignTimeDbContextFactory<DataContext>
{
    public DataContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<DataContext>();

        optionsBuilder.UseSqlServer(
            "Server=tcp:win24-sqlserver.database.windows.net,1433;Initial Catalog=ventixedatabase;Persist Security Info=False;User ID=SqlAdmin;Password=Labaislaptasslaptazodis1!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;",
            sqlOptions => sqlOptions.MigrationsAssembly("Infrastructure")
        );

        optionsBuilder.LogTo(Console.WriteLine, LogLevel.Warning)
                      .EnableSensitiveDataLogging(false);

        return new DataContext(optionsBuilder.Options);
    }
}
