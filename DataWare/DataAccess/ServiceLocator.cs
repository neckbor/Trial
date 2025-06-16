using DataAccess.Repositories;
using Domain.Primitives;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DataAccess;

public static class ServiceLocator
{
    public static void RegisterDataAccessServices(IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureDbContext(configuration);

        services.RegisterRepositories();

        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }

    private static IServiceCollection ConfigureDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<DataWareDbContext>((sp, options) =>
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            if (connectionString.StartsWith("postgresql://"))
            {
                var uri = new Uri(connectionString);
                var userInfo = uri.UserInfo.Split(':');
                connectionString = $"Host={uri.Host};Port={uri.Port};Database={uri.AbsolutePath.TrimStart('/')};Username={userInfo[0]};Password={userInfo[1]}";
            }

            options.UseNpgsql(connectionString, sqlBuilder =>
            {
                sqlBuilder.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
            });

            options.ConfigureWarnings(warnings =>
            {
                warnings.Ignore(CoreEventId.RedundantIndexRemoved);
            });

            options.UseSnakeCaseNamingConvention();
        });

        return services;
    }
}
