using Microsoft.EntityFrameworkCore;

namespace DataAccess;

internal class DataWareDbContext : DbContext
{
    public DataWareDbContext(DbContextOptions options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder) =>
        modelBuilder.ApplyConfigurationsFromAssembly(AssemblyReference.Assembly);
}
