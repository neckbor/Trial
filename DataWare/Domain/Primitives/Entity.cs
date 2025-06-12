namespace Domain.Primitives;

public abstract class Entity<TKey> : IEquatable<Entity<TKey>>
    where TKey : IComparable
{
    public TKey Id { get; protected set; }

    protected Entity(TKey id) => Id = id;

    protected Entity() { }

    public bool Equals(Entity<TKey>? other)
    {
        if (other is null)
        {
            return false;
        }

        if (other.GetType() != GetType())
        {
            return false;
        }

        return Id.Equals(other.Id);
    }

    public override bool Equals(object? obj)
    {
        if (obj is null)
        {
            return false;
        }

        if (obj.GetType() != GetType())
        {
            return false;
        }

        if (obj is not Entity<TKey> other)
        {
            return false;
        }

        return Id.Equals(other.Id);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(GetType(), Id.GetHashCode());
    }
}
