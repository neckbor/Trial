using Domain.Primitives;

namespace DataAccess;

internal sealed class UnitOfWork : IUnitOfWork
{
    private readonly DataWareDbContext _context;

    public UnitOfWork(DataWareDbContext context)
    {
        _context = context;
    }

    public Task SaveChangesAsync()
    {
        return _context.SaveChangesAsync();
    }
}
