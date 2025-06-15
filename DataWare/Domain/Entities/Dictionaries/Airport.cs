using Domain.Primitives;

namespace Domain.Entities.Dictionaries;

public class Airport : Entity<long>
{
    public static readonly Airport CDG = new(1, "CDG", "Аэропорт Шарль-Де-Голь", City.Paris);
    public static readonly Airport LAX = new(2, "LAX", "Международный аэропорт Лос-Анджелес", City.LosAngeles);
    public static readonly Airport DBX = new(3, "DBX", "Международный аэропорт Дубая", City.Dubai);
    public static readonly Airport LHR = new(4, "LHR", "Лондонский аэропорт Хитроу", City.London);
    public static readonly Airport TBS = new(5, "TBS", "Международный аэропорт Тбилиси имени Шота Руставели", City.Tbilisi);

    public string IATACode { get; private set; }
    public string Name { get; private set; }

    public long CityId { get; private set; }
    public virtual City City { get; private set; }

    private Airport(long id, string IATACode, string name, City city)
    {
        Id = id;
        this.IATACode = IATACode;
        Name = name;
        City = city;
    }

    public static IEnumerable<Airport> GetAll() => [CDG, LAX, DBX, LHR, TBS];
}
