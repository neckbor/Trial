namespace Domain.Entities.Dictionaries;

public class TicketingProvider
{
    public int Id { get; private set; }
    public string Code { get; private set; }
    public string Name { get; private set; }

    private TicketingProvider(int id, string code, string name)
    {
        Id = id;
        Code = code;
        Name = name;
    }
}
