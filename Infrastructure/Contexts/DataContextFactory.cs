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
            "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\Brodins\\Desktop\\ventixe\\Backend\\EventService\\Infrastructure\\Contexts\\local_eventservice_database.mdf;Integrated Security=True;Connect Timeout=30;Encrypt=True",
            sqlOptions => sqlOptions.MigrationsAssembly("Infrastructure")
        );

        optionsBuilder.LogTo(Console.WriteLine, LogLevel.Warning)
                      .EnableSensitiveDataLogging(false);

        return new DataContext(optionsBuilder.Options);
    }
}
