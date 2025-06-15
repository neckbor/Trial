using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace DataAccess;

internal class TemporaryDbContextFactory : IDesignTimeDbContextFactory<DataWareDbContext>
{
    public DataWareDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "db-connection.json"))
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<DataWareDbContext>()
            .UseNpgsql(configuration.GetConnectionString("DefaultConnection"))
            .EnableDetailedErrors()
            .EnableSensitiveDataLogging()
            .UseSnakeCaseNamingConvention();

        return new DataWareDbContext(optionsBuilder.Options);
    }
}
