using Domain.Primitives;

namespace Domain.Entities.Dictionaries;

public class City : Entity<long>
{
    public long Id { get; private set; }
    public string IATACode { get; private set; }
    public string Name { get; private set; }

    public virtual Country Country { get; private set; }
}
