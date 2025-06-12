using Domain.Primitives;

namespace Domain.Entities.Dictionaries;

public class Airline : Entity<int>
{
    public string IATACode { get; private set; }
    public string ICAOCode { get; private set; }
    public string Name { get; private set; }

    public virtual Country Country { get; private set; }
}
