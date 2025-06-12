using Domain.Primitives;

namespace Domain.Entities.Dictionaries;

public class Country : Entity<int>
{
    public string Code { get; private set; }
    public string Name { get; private set; }
}
