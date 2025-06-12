namespace Domain.Primitives;

public interface IUnitOfWork
{
    Task SaveChangesAsync();
}
