using Domain.Primitives;

namespace Domain.Entities.Dictionaries;

public class Airport : Entity<long>
{
    public string IATACode { get; private set; }
    public string Name { get; private set; }

    public int CityId { get; private set; }
    public virtual City City { get; private set; }
}
