using Domain.Primitives;

namespace Domain.Entities.Dictionaries;

public class Airport : Entity<long>
{
    public string IATACode { get; private set; }
    public string Name { get; private set; }

    public int CityId { get; private set; }
    public virtual City City { get; private set; }

    private Airport(string IATACode, string name, City city)
    {
        this.IATACode = IATACode;
        Name = name;
        City = city;
    }

    // for unit test
    internal static Airport Create(long id, string IATACode, string name)
    {
        var airport = new Airport(IATACode, name, null);
        airport.Id = id;

        return airport;
    }
}
