using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DataAccess.Initializers;

public static class DbInitialzier
{
    public static void ApplyMigration(IServiceScope scope)
    {
        using DataWareDbContext context = scope.ServiceProvider.GetService<DataWareDbContext>();

        context.Database.Migrate();
    }
}
