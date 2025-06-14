using Domain.Primitives;

namespace Domain.Entities.Dictionaries;

public class TicketingProvider : Entity<int>
{
    public string Code { get; private set; }
    public string Name { get; private set; }

    internal TicketingProvider(int id, string code, string name) 
    {
        Id = id;
        Code = code;
        Name = name;
    }
}
