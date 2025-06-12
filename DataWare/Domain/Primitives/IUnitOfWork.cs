namespace Domain.Primitives;

internal interface IUnitOfWork
{
    Task SaveChangesAsync();
}
