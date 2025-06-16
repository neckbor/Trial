using Domain.Primitives;

namespace Domain.Entities.Dictionaries;

public class TicketingProvider : Entity<int>
{
    public static readonly TicketingProvider AirTickets = new(1, "AIRTICKETS", "AirTickets.Fly");
    public static readonly TicketingProvider SkyTickets = new(2, "SKYTICKETS", "SKY Tickets");

    public string Code { get; private set; }
    public string Name { get; private set; }

    private TicketingProvider() { }

    private TicketingProvider(int id, string code, string name) 
    {
        Id = id;
        Code = code;
        Name = name;
    }

    public static IEnumerable<TicketingProvider> GetAll() => [AirTickets, SkyTickets];
}
